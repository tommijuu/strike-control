using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public GameObject Builder;
    RaycastHit targetHit;
    public List<Transform> selectedUnits = new List<Transform>();
    bool isDragging = false;
    Vector3 mousePosition;
    public GameObject firstClicked;

    public static float largestUnitInSelection = 1f;

    private void OnGUI()
    {
        if (isDragging)
        {
            Rect rect = ScreenHelper.GetScreenRect(mousePosition, Input.mousePosition);
            ScreenHelper.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.1f));
            ScreenHelper.DrawScreenRectBorder(rect, 1, Color.green);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!GameManager.instance.inUse && !GameManager.instance.paused && !GameManager.instance.IsGameOver)
        {
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Input.GetMouseButtonDown(0))
            {
                mousePosition = Input.mousePosition;

                if (Physics.Raycast(camRay, out targetHit))
                {
                    firstClicked = targetHit.transform.gameObject;
                    isDragging = true;

                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (Physics.Raycast(camRay, out targetHit))
                {
                    if (firstClicked.GetInstanceID() == targetHit.transform.gameObject.GetInstanceID())
                    {

                        if (targetHit.transform.CompareTag("PlayerUnit"))
                        {
                            SelectUnit(targetHit.transform, Input.GetKey(KeyCode.LeftShift));
                        }
                        else if (targetHit.transform.CompareTag("EnemyUnit"))
                        {
                            targetHit.transform.GetComponent<HealthBar>().ShowOrHide();
                        }
                        else if (targetHit.transform.CompareTag("Headquarters") || targetHit.transform.CompareTag("Building"))
                        {
                            targetHit.transform.GetComponent<HealthBar>().ShowOrHide();
                        }
                        else if (!targetHit.transform.CompareTag("PlayerUnit") && !Input.GetKey(KeyCode.LeftShift))
                        {
                            DeSelectUnits();
                        }
                    } else { DeSelectUnits(); }
                }
                else
                {
                    DeSelectUnits();
                }



                foreach (Collider selectableObject in FindObjectsOfType<Collider>())
                {
                    if (IsWithinSelectionBounds(selectableObject.transform))
                    {
                        if (selectableObject.transform.CompareTag("PlayerUnit"))
                            SelectUnit(selectableObject.transform, true);

                    }
                }

                isDragging = false;
            }

            if (Input.GetMouseButtonDown(1) && (selectedUnits.Count > 0))
            {
                UnitsTarget();
            }

        }
        else
        {
            isDragging = false;
        }
    }

    private void SelectUnit(Transform unit, bool isMultiSelect = false)
    {
        if (!isMultiSelect)
        {
            DeSelectUnits();
        }

        selectedUnits.Add(unit);
        unit.GetComponent<HealthBar>().HealthBarObject.SetActive(true);

        if (unit.gameObject.GetComponent<Turret>())
        {
            Transform[] allChildren = unit.GetComponentsInChildren<Transform>();
            foreach (Transform child in allChildren)
            {
                if (child.GetComponent<cakeslice.Outline>())
                {
                    child.GetComponent<cakeslice.Outline>().eraseRenderer = false;
                }
            }
        }
        else
        {
            Transform[] allChildren = unit.GetComponentsInChildren<Transform>();
            foreach (Transform child in allChildren)
            {
                if (child.GetComponent<cakeslice.Outline>())
                {
                    child.GetComponent<cakeslice.Outline>().eraseRenderer = false;
                }
            }
        }

        //largest unit
        float temp = 1f;
        largestUnitInSelection = 1f;
        for (int i = 0; i < selectedUnits.Count; i++)
        {
            temp = selectedUnits[i].GetComponent<PlayerUnitController>().UnitSize;
            if (temp > largestUnitInSelection)
                largestUnitInSelection = temp;
        }
    }

    private void DeSelectUnits()
    {
        for (int i = 0; i < selectedUnits.Count; i++)
        {
            if (selectedUnits[i].gameObject.GetComponent<Turret>())
            {
                Transform[] allChildren = selectedUnits[i].GetComponentsInChildren<Transform>();
                foreach (Transform child in allChildren)
                {
                    if (child.GetComponent<cakeslice.Outline>())
                    {
                        child.GetComponent<cakeslice.Outline>().eraseRenderer = true;
                    }
                }
            } else
            {
                Transform[] allChildren = selectedUnits[i].GetComponentsInChildren<Transform>();
                foreach (Transform child in allChildren)
                {
                    if (child.GetComponent<cakeslice.Outline>())
                    {
                        child.GetComponent<cakeslice.Outline>().eraseRenderer = true;
                    }
                }
            }


            selectedUnits[i].GetComponent<HealthBar>().HealthBarObject.SetActive(false);
        }
        selectedUnits.Clear();
    }

    private bool IsWithinSelectionBounds(Transform transform)
    {
        if (!isDragging)
        {
            return false;
        }

        Camera camera = Camera.main;
        Bounds viewportBounds = ScreenHelper.GetViewportBounds(camera, mousePosition, Input.mousePosition);
        return viewportBounds.Contains(camera.WorldToViewportPoint(transform.position));
    }

    private void UnitsTarget()
    {
        mousePosition = Input.mousePosition;

        Ray targetRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(targetRay, out targetHit))
        {
            if (targetHit.transform.CompareTag("EnemyUnit"))
            {
                Attack(targetHit.collider.gameObject);
            }
            else
            {
                MoveUnits(targetHit.point);
            }
        }
    }

    private void MoveUnits(Vector3 movePoint)
    {
        if (selectedUnits.Count == 1)
        {
            selectedUnits[0].GetComponent<UnitController>().SetDestination(targetHit.point);
        }
        else
        {
            List<Vector3> positionList = PositionListGenerator.GetPositionListAround(targetHit.point, new float[] { 2f, 4f, 6f, 8.5f }, new int[] { 5, 10, 20, 30 });
            for (int i = 0; i < selectedUnits.Count; i++)
            {
                selectedUnits[i].GetComponent<UnitController>().SetDestination(positionList[i]);
            }
        }
    }

    private void Attack(GameObject target)
    {
        Vector3 average = new Vector3();
        for (int i = 0; i < selectedUnits.Count; i++)
        {
            average.x += selectedUnits[i].position.x;
            average.z += selectedUnits[i].position.z;
        }
        average.x /= selectedUnits.Count;
        average.z /= selectedUnits.Count;

        float dt = 7f;
        float d = Mathf.Sqrt(Mathf.Pow(average.x - target.transform.position.x, 2) + Mathf.Pow(average.z - target.transform.position.z, 2));
        float t = dt / d;
        Vector3 targetPoint = new Vector3((1 - t) * target.transform.position.x + t * average.x, transform.position.y, (1 - t) * target.transform.position.z + t * average.z);

        List<Vector3> positionList = PositionListGenerator.GetPositionListAround(targetPoint, new float[] { 2f, 4f, 6f, 8.5f }, new int[] { 5, 10, 20, 30 });

        for (int i = 0; i < selectedUnits.Count; i++)
        {
            selectedUnits[i].gameObject.GetComponent<UnitController>().SetTargetAndDestination(target, positionList[i]);
            if(selectedUnits[i].gameObject.GetComponent<UnitController>().currentState != UnitController.State.Combat)
            {
                selectedUnits[i].gameObject.GetComponent<UnitController>().ChangeState(UnitController.State.AttackCommand);
            }
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class Sell : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Texture2D cursorArrow;

    private GameObject building;

    private Button button;

    public UnityEvent onLeft;
    public UnityEvent onRight;
    public UnityEvent onMiddle;

    Ray ray;
    RaycastHit rayHit;

    private GameSettings gameSettings;

    void Start()
    {
        gameSettings = GameObject.Find("GameSettings").GetComponent<GameSettings>();
    }

    void Update()
    {
        if (GameManager.instance.inSell)
        {
            if (GameManager.instance.paused || GameManager.instance.IsGameOver)
            {
                CancelSell();
            }
            if (Input.GetMouseButtonDown(0))
            {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out rayHit))
                {

                    building = rayHit.collider.gameObject;
                    Debug.Log(building);

                    if ((building != null && !building.CompareTag("Ground") && !building.CompareTag("PlayerUnit") && !building.CompareTag("EnemyUnit") && !building.CompareTag("ResourceNode") && !building.CompareTag("Headquarters")) || building.name.Equals("TurretModel(Clone)") || building.name.Equals("HeavyTurretModel(Clone)"))
                    {
                        name = building.GetComponent<BuildingManager>().name;
                        foreach (Button menuIcon in building.GetComponent<BuildingManager>().manageGame.menuIcons)
                        {
                            if (menuIcon.transform.Find(name))

                                button = menuIcon;
                        }
                        PlayerResources.instance.Money += button.GetComponent<BuildBuilding>().cost / 2;
                        Destroy(building.GetComponent<HealthBar>().HealthBarObject);
                        Destroy(building);
                        CancelSell();
                    }
                    else
                    {
                        Audiomanager.instance.PlaySound(5, 1f);
                    }
                }

            }

            if (Input.GetMouseButtonUp(1))
            {
                CancelSell();
            }

        }
    }

    public void ChangeCursor()
    {
        Audiomanager.instance.PlaySound(4, 1f);
        Cursor.SetCursor(cursorArrow, Vector2.zero, CursorMode.ForceSoftware);
        GameManager.instance.inSell = true;
    }

    public void CancelSell()
    {
        if (!GameManager.instance.paused && !GameManager.instance.IsGameOver)
        {
            Audiomanager.instance.PlaySound(4, 1f);
        }
        Cursor.SetCursor(gameSettings.cursorArrow, Vector2.zero, CursorMode.ForceSoftware);
        GameManager.instance.inSell = false;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (!GameManager.instance.inUse)
            {
                onLeft.Invoke();
            }
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (!GameManager.instance.inUse)
            {
                Debug.Log("play sound");
                Audiomanager.instance.PlaySound(5, 1f);
            }
        }
        if (eventData.button == PointerEventData.InputButton.Middle)
        {

        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        CostInfo.ShowTooltip_Static("Sell buildings\nfor half of the building cost");
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        CostInfo.HideTooltip_Static();
    }
}

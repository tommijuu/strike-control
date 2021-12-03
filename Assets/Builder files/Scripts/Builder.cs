using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour
{


    private Vector3 NewScale;
    public GameObject building;
    public float gridSize, radius;
    Vector3 gridPosition;

    void Start()
    {
        transform.localScale = building.transform.localScale;
        NewScale = transform.localScale + new Vector3(-0.01f, -0.01f, -0.01f);
    }


    void Update()
    {

        Plane plane = new Plane(Vector3.up, new Vector3(0, 0.5f, 0));
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance;
        if (plane.Raycast(ray, out distance))
        {
            transform.position = ray.GetPoint(distance);

            PseudoGrid();

            if (!SpaceCheck() && !DistanceCheck())
            {

                GetComponent<Renderer>().material.color = Color.green;
                if (Input.GetMouseButtonDown(0))
                {
                    BuildBuilding();
                }

            }
            else
            {
                GetComponent<Renderer>().material.color = Color.red;
            }

            if (Input.GetMouseButtonDown(1))
            {
                Destroy(gameObject);

            }

        }
    }

    private void BuildBuilding()
    {
        Instantiate(building, transform.position, Quaternion.identity);
    }

    private void PseudoGrid()
    {
        gridPosition.x = Mathf.Round(transform.position.x / gridSize) * gridSize;
        gridPosition.z = Mathf.Round(transform.position.z / gridSize) * gridSize;
        gridPosition.y = 0.5f;

        transform.position = gridPosition;
    }

    private bool SpaceCheck()
    {
        Collider[] hitColliders = Physics.OverlapBox(transform.position, NewScale / 2, Quaternion.identity);
        int i = 0;

        while (i < hitColliders.Length)
        {

            Debug.Log("Hit : " + hitColliders[i].name + i);
            i++;
            return true;
        }

        return false;
    }

    private bool DistanceCheck()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        int i = 0;

        while (i < hitColliders.Length)
        {
            if (hitColliders[i].tag == "Base")
            {
                return false;
            }
            i++;
        }
        return true;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, NewScale);

    }

}

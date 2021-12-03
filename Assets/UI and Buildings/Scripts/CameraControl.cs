using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class CameraControl : MonoBehaviour
{
    public GameObject draggedBuilding;
    public GameManager manageGame;
    public LayerMask layerMask;
   
    // Start is called before the first frame update
    void Start()
    {
        manageGame = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (draggedBuilding)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                draggedBuilding.transform.position = hit.point;
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (draggedBuilding.gameObject.name.Equals("PowerPlantModel(Clone)"))
                {
                    manageGame.buildingMenu.transform.GetChild(0).transform.GetChild(0). //searches the button's bool done and puts false
                        GetComponent<BuildBuilding>().done = false;

                    manageGame.buildingMenu.transform.GetChild(0).transform.GetChild(0). //searches the button's progress and resets it
                        transform.GetChild(2).GetComponent<Image>().fillAmount = 0;
                }

                if (draggedBuilding.gameObject.name.Equals("BarracksModel(Clone)"))
                {
                    manageGame.buildingMenu.transform.GetChild(0).transform.GetChild(1). //searches the button's bool done and puts false
                        GetComponent<BuildBuilding>().done = false;

                    manageGame.buildingMenu.transform.GetChild(0).transform.GetChild(1). //searches the button's progress and resets it
                        transform.GetChild(2).GetComponent<Image>().fillAmount = 0;

                    if (FindGameObjectsWithSameName(draggedBuilding.GetComponent<BuildingManager>().name + "Model(Clone)").Length <= 1)
                    {
                        foreach (Button unit in manageGame.buildingMenu.transform.GetChild(0).
                            transform.GetChild(1).GetComponent<BuildBuilding>().unlockingProduction)
                        {
                            //Adding production buttons to menu
                            manageGame.AddProductionButton(unit);
                        }
                    }
                }

                if (draggedBuilding.gameObject.name.Equals("OilRefineryModel(Clone)"))
                {
                    manageGame.buildingMenu.transform.GetChild(0).transform.GetChild(2). //searches the button's bool done and puts false
                        GetComponent<BuildBuilding>().done = false;

                    manageGame.buildingMenu.transform.GetChild(0).transform.GetChild(2). //searches the button's progress and resets it
                        transform.GetChild(2).GetComponent<Image>().fillAmount = 0;

                    if (FindGameObjectsWithSameName(draggedBuilding.GetComponent<BuildingManager>().name + "Model(Clone)").Length <= 1)
                    {
                        foreach (Button unit in manageGame.buildingMenu.transform.GetChild(0).
                            transform.GetChild(2).GetComponent<BuildBuilding>().unlockingProduction)
                        {
                            //Adding production buttons to menu
                            manageGame.AddProductionButton(unit);
                        }
                    }
                }
                draggedBuilding = null;
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider.gameObject.CompareTag("Building"))
                {
                    hit.collider.gameObject.GetComponent<BuildingManager>().RightClickAction(); //spawns next units to rightclicked building
                }
            }
        }
    }


    public GameObject[] FindGameObjectsWithSameName(string name)
    {
        GameObject[] allobjs = Object.FindObjectsOfType(typeof(GameObject)) as GameObject[];
        List<GameObject> likeNames = new List<GameObject>();
        foreach (GameObject obj in allobjs)
        {
            if (obj.name == name)
            {
                likeNames.Add(obj);
            }
        }
        return likeNames.ToArray();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Builder2 : MonoBehaviour
{

    private Vector3 NewScale;
    public GameObject building;
    public GameManager manageGame;
    public float gridSize, radius;
    public LayerMask layerMask;


    [HideInInspector]
    public GameObject[] Buildings;
    [HideInInspector]
    public GameObject[] OilRefineries;
    [HideInInspector]
    public GameObject Base;
    [HideInInspector]
    public GameObject builtBuilding;
    //[HideInInspector]
    //public int buildingNumber;
    [HideInInspector]
    public string buildingName;


    void Start()
    {
        manageGame = GameObject.Find("GameManager").GetComponent<GameManager>();

        ConstructionAreaStart();
        BuilderSize();
              
    }


    void Update()
    {
        //Plane plane = new Plane(Vector3.up, new Vector3(0, 0, 0));
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //float distance;
        //if (plane.Raycast(ray, out distance))
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            //transform.position = ray.GetPoint(distance);
            transform.position = hit.point;

            PseudoGrid();          

            if (!SpaceCheck() && !DistanceCheck())
            //if (!SpaceCheck())
            {

                GetComponent<Renderer>().material.SetColor("_Color", new Color(0, 1, 0, 0.8f));
                //GetComponent<Renderer>().material.color = Color.green;

                //if (Input.GetMouseButtonDown(0))
                //{
                //    BuildBuilding();
                //}

                if (Input.GetMouseButtonDown(0))
                {
                    //if (building.gameObject.name.Equals("PowerPlantModel(Clone)"))
                    if (building.gameObject.name.Equals("PowerPlantModel"))
                    {
                        //buildingNumber = 0;
                        buildingName = "PowerPlant(Clone)";
                        BuildBuilding();
                        //manageGame.buildingMenu.transform.GetChild(0).transform.GetChild(0).
                        manageGame.buildingMenu.transform.GetChild(0).transform.Find(buildingName).//searches the button's bool done and puts false
                            GetComponent<BuildBuilding>().done = false;

                        // manageGame.buildingMenu.transform.GetChild(0).transform.GetChild(0).
                        manageGame.buildingMenu.transform.GetChild(0).transform.Find(buildingName). //searches the button's progress and resets it
                            transform.GetChild(2).GetComponent<Image>().fillAmount = 0;

                        EndBuilding();

                    }

                    //if (building.gameObject.name.Equals("BarracksModel(Clone)"))
                    if (building.gameObject.name.Equals("BarracksModel"))
                    {
                        //buildingNumber = 1;
                        buildingName = "Barracks(Clone)";
                        BuildBuilding();
                        // manageGame.buildingMenu.transform.GetChild(0).transform.GetChild(1).
                        manageGame.buildingMenu.transform.GetChild(0).transform.Find(buildingName). //searches the button's bool done and puts false
                            GetComponent<BuildBuilding>().done = false;

                        // manageGame.buildingMenu.transform.GetChild(0).transform.GetChild(1).
                        manageGame.buildingMenu.transform.GetChild(0).transform.Find(buildingName). //searches the button's progress and resets it
                            transform.GetChild(2).GetComponent<Image>().fillAmount = 0;

                        if (FindGameObjectsWithSameName(building.GetComponent<BuildingManager>().name + "Model(Clone)").Length <= 1)
                        {
                            foreach (Button unit in manageGame.buildingMenu.transform.GetChild(0).
                                // transform.GetChild(1).GetComponent<BuildBuilding>().production)
                                transform.Find(buildingName).GetComponent<BuildBuilding>().unlockingProduction)
                            {
                                //Adding production buttons to menu
                                manageGame.AddProductionButton(unit);
                            }

                            
                            Button vehicleFactory = manageGame.buildingMenu.transform.GetChild(0).
                                transform.Find(buildingName).GetComponent<BuildBuilding>().unlockingBuildings[0];
                            manageGame.AddBuildingButton(vehicleFactory);
                        }
                        
                        EndBuilding();

                    }

                    //if (building.gameObject.name.Equals("OilRefineryModel(Clone)"))
                    if (building.gameObject.name.Equals("OilRefineryModel"))
                    {
                        //buildingNumber = 2;
                        buildingName = "OilRefinery(Clone)";
                        BuildBuilding();
                        // manageGame.buildingMenu.transform.GetChild(0).transform.GetChild(2).
                        manageGame.buildingMenu.transform.GetChild(0).transform.Find(buildingName). //searches the button's bool done and puts false
                            GetComponent<BuildBuilding>().done = false;

                        //manageGame.buildingMenu.transform.GetChild(0).transform.GetChild(2).
                        manageGame.buildingMenu.transform.GetChild(0).transform.Find(buildingName). //searches the button's progress and resets it
                            transform.GetChild(2).GetComponent<Image>().fillAmount = 0;

                        if (FindGameObjectsWithSameName(building.GetComponent<BuildingManager>().name + "Model(Clone)").Length <= 1)
                        {
                            foreach (Button building in manageGame.buildingMenu.transform.GetChild(0).
                                // transform.GetChild(2).GetComponent<BuildBuilding>().production)
                                transform.Find(buildingName).GetComponent<BuildBuilding>().unlockingBuildings)
                            {
                                //Adding production buttons to menu
                                manageGame.AddBuildingButton(building);
                            }
                            Button gatherer = manageGame.buildingMenu.transform.GetChild(0).
                                transform.Find(buildingName).GetComponent<BuildBuilding>().unlockingProduction[0];
                            manageGame.AddProductionButton(gatherer);

                            //Button barracks = manageGame.buildingMenu.transform.GetChild(0).
                            //    transform.Find(buildingName).GetComponent<BuildBuilding>().unlockingBuildings[0];
                            //manageGame.AddBuildingButton(barracks);
                        }
                       

                        EndBuilding();


                    }

                    if (building.gameObject.name.Equals("VehicleFactoryModel"))
                    {
                        //buildingNumber = 3;
                        buildingName = "Vehicle Factory(Clone)";
                        BuildBuilding();
                        // manageGame.buildingMenu.transform.GetChild(0).transform.GetChild(3).
                        manageGame.buildingMenu.transform.GetChild(0).transform.Find(buildingName). //searches the button's bool done and puts false
                            GetComponent<BuildBuilding>().done = false;

                        // manageGame.buildingMenu.transform.GetChild(0).transform.GetChild(3).
                        manageGame.buildingMenu.transform.GetChild(0).transform.Find(buildingName). //searches the button's progress and resets it
                            transform.GetChild(2).GetComponent<Image>().fillAmount = 0;

                        if (FindGameObjectsWithSameName(building.GetComponent<BuildingManager>().name + "Model(Clone)").Length <= 1)
                        {
                            foreach (Button unit in manageGame.buildingMenu.transform.GetChild(0).
                                // transform.GetChild(3).GetComponent<BuildBuilding>().production)
                                transform.Find(buildingName).GetComponent<BuildBuilding>().unlockingProduction)
                            {
                                //Adding production buttons to menu
                                manageGame.AddProductionButton(unit);
                            }
                        }


                        EndBuilding();


                    }

                    if (building.gameObject.name.Equals("TurretModel"))
                    {
                        //buildingNumber = 3;
                        buildingName = "Turret(Clone)";
                        BuildBuilding();
                        // manageGame.buildingMenu.transform.GetChild(0).transform.GetChild(3).
                        manageGame.buildingMenu.transform.GetChild(0).transform.Find(buildingName). //searches the button's bool done and puts false
                            GetComponent<BuildBuilding>().done = false;

                        // manageGame.buildingMenu.transform.GetChild(0).transform.GetChild(3).
                        manageGame.buildingMenu.transform.GetChild(0).transform.Find(buildingName). //searches the button's progress and resets it
                            transform.GetChild(2).GetComponent<Image>().fillAmount = 0;

                       

                        EndBuilding();


                    }

                    if (building.gameObject.name.Equals("HeavyTurretModel"))
                    {
                        //buildingNumber = 3;
                        buildingName = "HeavyTurret(Clone)";
                        BuildBuilding();
                        // manageGame.buildingMenu.transform.GetChild(0).transform.GetChild(3).
                        manageGame.buildingMenu.transform.GetChild(0).transform.Find(buildingName). //searches the button's bool done and puts false
                            GetComponent<BuildBuilding>().done = false;

                        // manageGame.buildingMenu.transform.GetChild(0).transform.GetChild(3).
                        manageGame.buildingMenu.transform.GetChild(0).transform.Find(buildingName). //searches the button's progress and resets it
                            transform.GetChild(2).GetComponent<Image>().fillAmount = 0;



                        EndBuilding();


                    }
                }
            

                
                

            }
            else
            {
                GetComponent<Renderer>().material.SetColor("_Color", new Color(1, 0, 0, 0.8f));
                //GetComponent<Renderer>().material.color = Color.red;
            }

            if (Input.GetMouseButtonDown(1))
            {
                EndBuilding();
            }
        }

    }
    private void BuilderSize()
    {
        transform.localScale = building.transform.localScale;
        NewScale = transform.localScale * 0.99f;
    }
    public void BuildBuilding()
    {
        builtBuilding = Instantiate(building, transform.position, transform.rotation);
        //builtBuilding.GetComponent<BuildingManager>().MakeBuildingPrimary(buildingNumber);
        if (builtBuilding.GetComponent<BuildingManager>().building.GetComponent<BuildBuilding>().unlockingProduction.Length > 0) //not all buildings unlock units
        {
            builtBuilding.GetComponent<BuildingManager>().MakeBuidlingPrimary(buildingName);
        }
        //MakeBuildingPrimary(buildingNumber);
    }



    public void EndBuilding()
        {
            //GameObject.Find("ConstructionArea").SetActive(false);
            ConstructionAreaInactive();
            //building = null;
            Destroy(gameObject);
        }

    public void PseudoGrid()
        {
            Vector3 gridPosition;
            gridPosition.x = Mathf.Round(transform.position.x / gridSize) * gridSize;
            gridPosition.z = Mathf.Round(transform.position.z / gridSize) * gridSize;
            gridPosition.y = Mathf.Round(transform.position.y / gridSize) * gridSize + 0.05f;

            //transform.position.y + 0.05f;
            //gridPosition.y = 0.05f;

            transform.position = gridPosition;
        }

    public bool SpaceCheck()
    {
        Collider[] hitColliders = Physics.OverlapBox(transform.position, NewScale / 2, transform.rotation);
        int i = 0;

        while (i < hitColliders.Length)
        {

            Debug.Log("Hit : " + hitColliders[i].name + i);
            i++;
            return true;
        }

        return false;
    }

    

    public bool DistanceCheck()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        int i = 0;

        while (i < hitColliders.Length)
        {
            if (hitColliders[i].tag == "Headquarters")
            {
                float distance = hitColliders[i].transform.Find("ConstructionArea").GetComponent<ConstructionArea>().radius;
                if (distance >= Vector3.Distance(hitColliders[i].transform.position, transform.position))
                {
                    return false;
                }
                //return true;    
            }
            //i++;
            if (hitColliders[i].tag == "Building")
            {
                float distance = hitColliders[i].transform.Find("ConstructionArea").GetComponent<ConstructionArea>().radius;
                if (distance >= Vector3.Distance(hitColliders[i].transform.position, transform.position))
                {
                    return false;
                }
                //return true;
            }
            /*
            if (hitColliders[i].tag == "OilRefinery")
            {
                float distance = hitColliders[i].transform.Find("ConstructionArea").GetComponent<ConstructionArea>().radius;
                if (distance >= Vector3.Distance(hitColliders[i].transform.position, transform.position))
                {
                    return false;
                }
                //return true;
            }*/
            i++;
            
        }
        return true;
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
    /*
    public void MakeBuildingPrimary(int i)
        {
        //goes through available unit buttons and makes it the spawn point
        
            foreach (Button unit in manageGame.buildingMenu.transform.GetChild(0).
                transform.GetChild(i).GetComponent<BuildBuilding>().production)
            {
                    unit.GetComponent<BuildUnit>().unit.creationPlace = transform.position;
            }
        
        }
    */
    public void ConstructionAreaStart()
    {
        Base = GameObject.Find("HeadQuartersModel").transform.Find("ConstructionArea").gameObject;//.SetActive(true);
        radius = Base.GetComponent<ConstructionArea>().radius;

        Buildings = GameObject.FindGameObjectsWithTag("Building");
        OilRefineries = GameObject.FindGameObjectsWithTag("OilRefinery");

        ConstructionAreaActive();
    }
    public void ConstructionAreaActive()
    {
        Base.SetActive(true);
        
        int i = 0;
        while (i < Buildings.Length)
        {
            Buildings[i].transform.Find("ConstructionArea").gameObject.SetActive(true);
            i++;
        }
        i = 0;
        while (i < OilRefineries.Length)
        {
            OilRefineries[i].transform.Find("ConstructionArea").gameObject.SetActive(true);
            i++;
        }
    }

    public void ConstructionAreaInactive()
    {
        Base.SetActive(false);
        

        //GameObject[] Buildings = GameObject.FindGameObjectsWithTag("Building");
        int i = 0;
        while (i < Buildings.Length)
        {
            Buildings[i].transform.Find("ConstructionArea").gameObject.SetActive(false);
            i++;
        }
        i = 0;
        while (i < OilRefineries.Length)
        {
            OilRefineries[i].transform.Find("ConstructionArea").gameObject.SetActive(false);
            i++;
        }
    }


    public void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, NewScale);

        }

    }


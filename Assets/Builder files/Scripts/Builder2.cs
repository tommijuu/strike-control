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
    public BuildingManager[] buildingManagers;
    [HideInInspector]
    public GameObject[] Buildings;
    [HideInInspector]
    public GameObject[] OilRefineries;
    [HideInInspector]
    public GameObject Base;
    [HideInInspector]
    public GameObject builtBuilding;
    [HideInInspector]
    public string buildingName;


    void Start()
    {
        manageGame = GameObject.Find("GameManager").GetComponent<GameManager>();

        ConstructionAreaStart();
        BuilderSize();
        manageGame.builderActive = true;
              
    }


    void Update()
    {
        if (manageGame.paused || manageGame.IsGameOver)
        {
            CancelBuilding();
        }

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            transform.position = hit.point;

            PseudoGrid();          

            if (!SpaceCheck() && !DistanceCheck() && transform.position.y == 0.1f)
            {

                GetComponent<Renderer>().material.SetColor("_Color", new Color(0, 1, 0, 0.8f));

                if (Input.GetMouseButtonDown(0))
                {
                    if (building.gameObject.name.Equals("PowerPlantModel"))
                    {
                        buildingName = "PowerPlant(Clone)";
                        BuildBuilding();
                        manageGame.buildingMenu.transform.GetChild(0).transform.Find(buildingName).//searches the button's bool done and puts false
                            GetComponent<BuildBuilding>().done = false;

                        manageGame.buildingMenu.transform.GetChild(0).transform.Find(buildingName). //searches the button's progress and resets it
                            transform.GetChild(2).GetComponent<Image>().fillAmount = 0;

                        EndBuilding();

                    }
                    if (building.gameObject.name.Equals("BarracksModel"))
                    {
                        buildingName = "Barracks(Clone)";
                        BuildBuilding();
                        manageGame.buildingMenu.transform.GetChild(0).transform.Find(buildingName). //searches the button's bool done and puts false
                            GetComponent<BuildBuilding>().done = false;

                        // manageGame.buildingMenu.transform.GetChild(0).transform.GetChild(1).
                        manageGame.buildingMenu.transform.GetChild(0).transform.Find(buildingName). //searches the button's progress and resets it
                            transform.GetChild(2).GetComponent<Image>().fillAmount = 0;

                        if (FindGameObjectsWithSameName(building.GetComponent<BuildingManager>().name + "Model(Clone)").Length <= 1)
                        {
                            foreach (Button unit in manageGame.buildingMenu.transform.GetChild(0).
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

                    if (building.gameObject.name.Equals("OilRefineryModel"))
                    {
                        buildingName = "OilRefinery(Clone)";
                        BuildBuilding();
                        manageGame.buildingMenu.transform.GetChild(0).transform.Find(buildingName). //searches the button's bool done and puts false
                            GetComponent<BuildBuilding>().done = false;

                        manageGame.buildingMenu.transform.GetChild(0).transform.Find(buildingName). //searches the button's progress and resets it
                            transform.GetChild(2).GetComponent<Image>().fillAmount = 0;

                        if (FindGameObjectsWithSameName(building.GetComponent<BuildingManager>().name + "Model(Clone)").Length <= 1)
                        {
                            foreach (Button building in manageGame.buildingMenu.transform.GetChild(0).
                                transform.Find(buildingName).GetComponent<BuildBuilding>().unlockingBuildings)
                            {
                                //Adding production buttons to menu
                                manageGame.AddBuildingButton(building);
                            }
                            Button gatherer = manageGame.buildingMenu.transform.GetChild(0).
                                transform.Find(buildingName).GetComponent<BuildBuilding>().unlockingProduction[0];
                            manageGame.AddProductionButton(gatherer);
                        }
                       

                        EndBuilding();


                    }

                    if (building.gameObject.name.Equals("VehicleFactoryModel"))
                    {
                        buildingName = "VehicleFactory(Clone)";
                        BuildBuilding();
                        manageGame.buildingMenu.transform.GetChild(0).transform.Find(buildingName). //searches the button's bool done and puts false
                            GetComponent<BuildBuilding>().done = false;

                        manageGame.buildingMenu.transform.GetChild(0).transform.Find(buildingName). //searches the button's progress and resets it
                            transform.GetChild(2).GetComponent<Image>().fillAmount = 0;

                        if (FindGameObjectsWithSameName(building.GetComponent<BuildingManager>().name + "Model(Clone)").Length <= 1)
                        {
                            foreach (Button building in manageGame.buildingMenu.transform.GetChild(0).
                                transform.Find(buildingName).GetComponent<BuildBuilding>().unlockingBuildings)
                            {
                                //Adding production buttons to menu
                                manageGame.AddBuildingButton(building);
                            }
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
                        buildingName = "Turret(Clone)";
                        BuildBuilding();
                        manageGame.buildingMenu.transform.GetChild(0).transform.Find(buildingName). //searches the button's bool done and puts false
                            GetComponent<BuildBuilding>().done = false;

                        manageGame.buildingMenu.transform.GetChild(0).transform.Find(buildingName). //searches the button's progress and resets it
                            transform.GetChild(2).GetComponent<Image>().fillAmount = 0;

                       

                        EndBuilding();


                    }

                    if (building.gameObject.name.Equals("HeavyTurretModel"))
                    {
                        buildingName = "HeavyTurret(Clone)";
                        BuildBuilding();
                        manageGame.buildingMenu.transform.GetChild(0).transform.Find(buildingName). //searches the button's bool done and puts false
                            GetComponent<BuildBuilding>().done = false;

                        manageGame.buildingMenu.transform.GetChild(0).transform.Find(buildingName). //searches the button's progress and resets it
                            transform.GetChild(2).GetComponent<Image>().fillAmount = 0;



                        EndBuilding();


                    }

                    if (building.gameObject.name.Equals("BattleLabModel"))
                    {
                        buildingName = "BattleLab(Clone)";
                        BuildBuilding();

                        manageGame.buildingMenu.transform.GetChild(0).transform.Find(buildingName). //searches the button's bool done and puts false
                            GetComponent<BuildBuilding>().done = false;

                        manageGame.buildingMenu.transform.GetChild(0).transform.Find(buildingName). //searches the button's progress and resets it
                            transform.GetChild(2).GetComponent<Image>().fillAmount = 0;
                        
                        if (FindGameObjectsWithSameName(building.GetComponent<BuildingManager>().name + "Model(Clone)").Length <= 1)
                        {
                            foreach (Button building in manageGame.buildingMenu.transform.GetChild(0).
                                transform.Find(buildingName).GetComponent<BuildBuilding>().unlockingBuildings)
                            {
                                //Adding production buttons to menu
                                manageGame.AddBuildingButton(building);
                            }
                        }

                        EndBuilding();
                    }
                }
            

                
                

            }
            else
            {
                GetComponent<Renderer>().material.SetColor("_Color", new Color(1, 0, 0, 0.8f));
                if (Input.GetMouseButtonDown(0))
                {
                    Audiomanager.instance.PlaySound(5, 1f);
                }
            }
            
            if (Input.GetMouseButtonDown(1))
            {
                Audiomanager.instance.PlaySound(4, 1f);
                CancelBuilding();
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
        Audiomanager.instance.PlaySound(4, 1f);
        builtBuilding = Instantiate(building, transform.position, transform.rotation);
        if (builtBuilding.GetComponent<BuildingManager>().building.GetComponent<BuildBuilding>().unlockingProduction.Length > 0) //not all buildings unlock units
        {
            builtBuilding.GetComponent<BuildingManager>().MakeBuidlingPrimary(buildingName);
        }
    }



    public void EndBuilding()
        {
            ConstructionAreaInactive2();
            Destroy(gameObject);
        }

    public void CancelBuilding()
    {
        manageGame.buildingsInProgress = true;
        ConstructionAreaInactive2();
        Destroy(gameObject);

    }

    public void PseudoGrid()
        {
            Vector3 gridPosition;
            gridPosition.x = Mathf.Round(transform.position.x / gridSize) * gridSize;
            gridPosition.z = Mathf.Round(transform.position.z / gridSize) * gridSize;
            gridPosition.y = Mathf.Round(transform.position.y / gridSize) * gridSize + 0.1f;
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
            if (hitColliders[i].CompareTag("Headquarters"))
            {
                float distance = hitColliders[i].transform.Find("ConstructionArea").GetComponent<ConstructionArea>().radius;
                if (distance >= Vector3.Distance(hitColliders[i].transform.position, transform.position))
                {
                    return false;
                } 
            }
            if (hitColliders[i].CompareTag("Building"))
            {
                float distance = hitColliders[i].transform.Find("ConstructionArea").GetComponent<ConstructionArea>().radius;
                if (distance >= Vector3.Distance(hitColliders[i].transform.position, transform.position))
                {
                    return false;
                }
            }
            if (hitColliders[i].name.Contains("TurretModel"))
            {
                float distance = hitColliders[i].transform.Find("ConstructionArea").GetComponent<ConstructionArea>().radius;
                if (distance >= Vector3.Distance(hitColliders[i].transform.position, transform.position))
                {
                    return false;
                }
            }
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

    public void ConstructionAreaStart()
    {
        Base = GameObject.Find("HeadQuartersModel").transform.Find("ConstructionArea").gameObject;
        radius = Base.GetComponent<ConstructionArea>().radius;

        buildingManagers = FindObjectsOfType(typeof(BuildingManager)) as BuildingManager[];
        ConstructionAreaActive2();
    }

    public void ConstructionAreaActive2()
    {
        if (!(Base == null))
        {
            Base.SetActive(true);
        }

        foreach (BuildingManager item in buildingManagers)
        {
            if(!(item == null))
            {
                item.gameObject.transform.Find("ConstructionArea").gameObject.SetActive(true);
            }

        }
    }

    public void ConstructionAreaInactive2()
    {
        if(!(Base == null))
        {
            Base.SetActive(false);
        }

        foreach(BuildingManager item in buildingManagers)
        {
            if (!(item == null))
            {
                item.gameObject.transform.Find("ConstructionArea").gameObject.SetActive(false);
            }
            
        }
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
    }

    public void ConstructionAreaInactive()
    {
        Base.SetActive(false);
        
        int i = 0;
        while (i < Buildings.Length)
        {
            if (Buildings[i] == null)
            {

            }
            else
            {
                Buildings[i].transform.Find("ConstructionArea").gameObject.SetActive(false);
            }
            
            i++;
        }
        i = 0;
        while (i < OilRefineries.Length)
        {
            OilRefineries[i].transform.Find("ConstructionArea").gameObject.SetActive(false);
            i++;
        }
    }

    private void OnDestroy()
    {
        manageGame.builderActive = false;
    }
}


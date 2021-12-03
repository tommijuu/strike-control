using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingManager : MonoBehaviour
{
    public Button building;
    public new string name;
    public GameManager manageGame;
    public BuildingStats buildingStats;
    //public int buildingNumber;
    [HideInInspector]
    public string buildingName;

    public QueueManager queueManager;

    void Awake()
    {
        manageGame = GameObject.Find("GameManager").GetComponent<GameManager>();
        queueManager = manageGame.buildingMenu.transform.GetChild(1).GetComponent<QueueManager>();
    }
    void Start()
    {
        buildingName = name + "(Clone)";
        if (buildingStats.powerCost < 0) //powerplant has minus powercost
        {
            PlayerResources.instance.Power -= buildingStats.powerCost;
            if(PlayerResources.instance.LowPower && (PlayerResources.instance.Power >= PlayerResources.instance.PowerConsumption))
            {
                PlayerResources.instance.PowerRestored();
            }
        }
        else
        {
            PlayerResources.instance.PowerConsumption += buildingStats.powerCost;
            if (!PlayerResources.instance.LowPower && (PlayerResources.instance.Power < PlayerResources.instance.PowerConsumption))
            {
                PlayerResources.instance.PowerDown();
            }
        }
    }

    public void RightClickAction()
    {
        //MakeBuildingPrimary(buildingNumber);
        MakeBuidlingPrimary(buildingName);
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
    
    public void MakeBuidlingPrimary(string i)
    {
        foreach (Button unit in manageGame.buildingMenu.transform.GetChild(0).
            transform.Find(i).GetComponent<BuildBuilding>().unlockingProduction)
        {
            foreach (Transform child in transform)
            {
                if (child.name.Equals("SpawnHere"))
                {
                    Debug.Log("miksi");
                    unit.GetComponent<BuildUnit>().unit.creationPlace = child.position;
                    break;
                }
            }
        }        
    }
    
    private void OnDestroy()
    {

        if (buildingStats.powerCost < 0)
        {
            PlayerResources.instance.Power += buildingStats.powerCost;
            if (PlayerResources.instance.Power >= PlayerResources.instance.PowerConsumption)
            {
                PlayerResources.instance.PowerRestored();
            }
        }
        else
        {
            PlayerResources.instance.PowerConsumption -= buildingStats.powerCost;
            if (PlayerResources.instance.Power < PlayerResources.instance.PowerConsumption)
            {
                PlayerResources.instance.PowerDown();
            }
        }
        if (FindGameObjectsWithSameName(name + "Model(Clone)").Length == 0)
        {
            Debug.Log(name+" is last building of its type");
            /*
            BuildBuilding lastbuilding = manageGame.buildingMenu.transform.GetChild(0).transform.Find(buildingName).GetComponent<BuildBuilding>();
            
            //if (manageGame.buildingMenu.transform.GetChild(0).transform.Find(buildingName).GetComponent<BuildBuilding>().inProgress)
            if (lastbuilding.inProgress || lastbuilding.done || lastbuilding.paused)
            {
                manageGame.buildingMenu.transform.GetChild(0).transform.Find(buildingName).GetComponent<BuildBuilding>().CancelBuilding();



                manageGame.buildingMenu.transform.GetChild(0).transform.Find(buildingName).
                    GetComponent<BuildBuilding>().SetButtonsInteractable(true);
            }
            */
            
            foreach (Button unit in manageGame.buildingMenu.transform.GetChild(0).transform.Find(buildingName).
                GetComponent<BuildBuilding>().unlockingProduction)
            {
                /*
                GameObject lastproductionbutton = GameObject.Find(unit.name + "(Clone)");
                BuildUnit lastproductionbuttonscript = lastproductionbutton.GetComponent<BuildUnit>();

                if (lastproductionbuttonscript.inProgress || lastproductionbuttonscript.paused || lastproductionbuttonscript.done)
                {
                    //lastproductionbuttonscript.EmptyQueue();
                    lastproductionbuttonscript.CancelProduction();
                }
                
                public void EmptyQueue(){

                    while (unitQueue.Count > 0)
                    {
                        PlayerResources.instance.Money += Mathf.RoundToInt((unit.cost * (counter / unit.productionDuration)));
                        unitQueue.Dequeue();
                        counter = 0;
                        progress.fillAmount = 0;
                    }
                }
                
                */
                manageGame.RemoveProductionButton(unit);
                if (unit.tag.Equals("InfantryButton"))
                {
                    queueManager.TerminateQueue(queueManager.infantryQueue);
                }
                else if (unit.tag.Equals("TankButton"))
                {
                    queueManager.TerminateQueue(queueManager.tankQueue);
                }
                else
                {
                    queueManager.TerminateQueue(queueManager.otherQueue);
                }
            }
            foreach (Button unit in manageGame.buildingMenu.transform.GetChild(0).transform.Find(buildingName).
                GetComponent<BuildBuilding>().unlockingBuildings)
            {
                GameObject lastbuildingbutton = GameObject.Find(unit.name + "(Clone)");
                BuildBuilding lastbuildingbuttonscript = lastbuildingbutton.GetComponent<BuildBuilding>();
                Debug.Log(unit.name + "  inProgress = " + lastbuildingbuttonscript.inProgress + " | paused = " + lastbuildingbuttonscript.paused + " | done = " + lastbuildingbuttonscript.done);
                if (lastbuildingbuttonscript.inProgress || lastbuildingbuttonscript.paused || lastbuildingbuttonscript.done)
                {
                    Debug.Log("cancel "+unit.name+" construction");
                    //lastbuildingunbuttonscript.paused = false;
                    lastbuildingbuttonscript.CancelBuilding();
                }
                manageGame.RemoveBuildingButton(unit);
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

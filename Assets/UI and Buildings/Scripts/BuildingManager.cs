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
    
    [HideInInspector]
    public string buildingName;

    private GameObject effectEmpire;
    private GameObject effectUnion;

    public GameObject empireEffect;
    public GameObject unionEffect;

    private bool hasProduction = false;
    private bool oilRefinery = false;
    private GameObject primaryBuilding;

    public bool isPrimary = false;


    void Awake()
    {
        manageGame = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    void Start()
    {
        if (name.Equals("OilRefinery"))
        {
            oilRefinery = true;
        }

        if (building.GetComponent<BuildBuilding>().unlockingProduction.Length > 0 || building.GetComponent<BuildBuilding>().unlockingBuildings.Length > 0)
        {
            hasProduction = true;
            ParticleSystem ef = empireEffect.GetComponent<ParticleSystem>();
            var empireShape = ef.shape;
            empireShape.scale = new Vector3(transform.localScale.z, transform.localScale.x, 0f);
            ParticleSystem uf = unionEffect.GetComponent<ParticleSystem>();
            var unionShape = uf.shape;
            unionShape.scale = new Vector3(transform.localScale.z, transform.localScale.x, 0f);
        }

        if (building.GetComponent<BuildBuilding>().unlockingProduction.Length > 0)
        {
            primaryBuilding = transform.Find("PrimarySelected").gameObject;
        }
            
        buildingName = name + "(Clone)";
        PlayerResources.instance.ChangePowerOnBuild(buildingStats.powerCost);
    }

    void Update()
    {
        if (hasProduction)
        {
            if (manageGame.unionSpecialPowerActive)
            {
                if (!effectUnion)
                {
                    effectUnion = Instantiate(unionEffect, new Vector3(transform.position.x, 2f, transform.position.z), unionEffect.transform.rotation);
                }
            }
            else
            {
                if (effectUnion)
                {
                    Destroy(effectUnion);
                }
            }
        }
        if (oilRefinery)
        {
            if (manageGame.empireSpecialPowerActive)
            {
                if (!effectEmpire)
                {
                    effectEmpire = Instantiate(empireEffect, new Vector3(transform.position.x, 2f, transform.position.z), empireEffect.transform.rotation);
                }
            }
            else
            {
                if (effectEmpire)
                {
                    Destroy(effectEmpire);
                }
            }
        }

        if (building.GetComponent<BuildBuilding>().unlockingProduction.Length > 0)
        {
            if (isPrimary)
            {
                if (!primaryBuilding.activeSelf)
                {
                    primaryBuilding.SetActive(true);
                }
            }
            else
            {
                if (primaryBuilding.activeSelf)
                {
                    primaryBuilding.SetActive(false);
                }
            }
        }
            
        
    }

    public void RightClickAction()
    {
        if (building.GetComponent<BuildBuilding>().unlockingProduction.Length > 0)
        {
            if (!isPrimary)
            {
                Audiomanager.instance.PlaySound(4, 1f);
            }
            MakeBuidlingPrimary(buildingName);
        }
    }
    
    public void MakeBuidlingPrimary(string i)
    {
        
        foreach (GameObject building in FindGameObjectsWithSameName(name + "Model(Clone)"))
        {
            building.GetComponent<BuildingManager>().isPrimary = false;
        }
        isPrimary = true;
        
        foreach (Button unit in manageGame.buildingMenu.transform.GetChild(0).
            transform.Find(i).GetComponent<BuildBuilding>().unlockingProduction)
        {
            foreach (Transform child in transform)
            {
                if (child.name.Equals("SpawnHere"))
                {
                    unit.GetComponent<BuildUnit>().unit.creationPlace = child.position;
                    break;
                }
            }
        }        
    }
    
    private void OnDestroy()
    {

        PlayerResources.instance.ChangePowerOnDestroy(buildingStats.powerCost);

        if (FindGameObjectsWithSameName(name + "Model(Clone)").Length == 0)
        {   
            foreach (Button unit in manageGame.buildingMenu.transform.GetChild(0).transform.Find(buildingName).
                GetComponent<BuildBuilding>().unlockingProduction)
            {
                manageGame.RemoveProductionButton(unit);
            }
            
            foreach (Button unit in manageGame.buildingMenu.transform.GetChild(0).transform.Find(buildingName).
                GetComponent<BuildBuilding>().unlockingBuildings)
            {
                GameObject lastbuildingbutton = GameObject.Find(unit.name + "(Clone)");
                if (!(lastbuildingbutton.GetComponent<BuildBuilding>() == null))
                {
                    BuildBuilding lastbuildingbuttonscript = lastbuildingbutton.GetComponent<BuildBuilding>();
                   
                    if (lastbuildingbuttonscript.inProgress || lastbuildingbuttonscript.paused || lastbuildingbuttonscript.done)
                    {
                        lastbuildingbuttonscript.CancelBuilding();
                    }
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

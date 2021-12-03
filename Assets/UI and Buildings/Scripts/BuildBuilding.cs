using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class BuildBuilding : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public UnityEvent onLeft;
    public UnityEvent onRight;
    public UnityEvent onMiddle;

    public Image progress;

    public GameManager manageGame;

    public float counter;

    public GameObject model;

    public Button[] unlockingProduction; //to store unit buttons for units the building produces (dragged from unity)

    public Button[] unlockingBuildings;

    public int cost;

    public bool inProgress;
    public bool paused;
    public bool done;

    public int buildingDuration;
    public int lowPowerTimeModifier;

    public GameObject Builder;
    public GameObject ActiveBuilder;

    [HideInInspector]
    public string name;

    public BuildingStats buildingStats;

    void Start()
    {
        manageGame = GameObject.Find("GameManager").GetComponent<GameManager>();
    }


    void Update()
    {

        if (PlayerResources.instance.Power >= PlayerResources.instance.PowerConsumption)
        {
            lowPowerTimeModifier = 1;
        }
        else
        {
            lowPowerTimeModifier = 2;
        }

        if (inProgress)
        {
            manageGame.buildingsInProgress = true;
            manageGame.buildingsInProgressSound = true;

            if (PlayerResources.instance.Money > 0)
            {
                if (counter < buildingDuration)
                {
                    gameObject.GetComponent<Button>().interactable = true;
                    counter += Time.deltaTime * manageGame.USPConstructionTrainingModifier / lowPowerTimeModifier;
                }
                else
                {
                    counter = 0;
                }

                progress.fillAmount = Mathf.Lerp(0, 1, counter / buildingDuration);

                int substracted = ChangeMoney(cost);
                PlayerResources.instance.Money -= substracted;

                if (counter >= buildingDuration)
                {
                    inProgress = false;
                    manageGame.buildingsInProgressSound = false;
                    done = true;
                    gameObject.GetComponent<Button>().interactable = true;
                    ShowReadyText();
                }
            }
        }
    }


    public void StartBuilding()
    {
        if(gameObject.GetComponent<Button>().interactable == false)
        {
            manageGame.buildingMenu.transform.Find("WarningText").gameObject.GetComponent<Animator>().Play("textFadeIn", 0, 0.25f);
        }

        if (paused == false)
        {
            SetButtonsInteractable(false);

            if (done == true && !GameObject.Find("Builder2(Clone)") && manageGame.buildingsInProgress)
            {
                Builder.GetComponent<Builder2>().building = model;
                ActiveBuilder = Instantiate(Builder, new Vector3(0, -5, 0), model.transform.rotation);
                manageGame.buildingsInProgress = false;
                manageGame.buildingsInProgressSound = false;
                SetButtonsInteractable(true);
            }
            else if (inProgress == false && !GameObject.Find("Builder2(Clone)") && !manageGame.buildingsInProgress)
            {
                //Start bulding again
                counter = 0;
                inProgress = true;
                manageGame.buildingsInProgress = true;
                manageGame.buildingsInProgressSound = true;
                SetButtonsInteractable(false);
            }
            else if (inProgress == true)
            {
                PauseBuilding();
            }
        }
        else
        {
            ResumeBuilding();
        }
    }

    public void CancelBuilding()
    {
        PlayerResources.instance.Money += Mathf.RoundToInt((cost * (counter / buildingDuration)));
        inProgress = false;
        counter = 0;
        progress.fillAmount = 0;
        done = false;
        if (ActiveBuilder)
        {
            ActiveBuilder.transform.GetComponent<Builder2>().EndBuilding();
        }
        manageGame.buildingsInProgress = false;
        manageGame.buildingsInProgressSound = false;
        SetButtonsInteractable(true);
    }

    public void ResumeBuilding()
    {
        manageGame.buildingsInProgress = true;
        manageGame.buildingsInProgressSound = true;
        inProgress = true;
        paused = false;
        SetButtonsInteractable(false);
    }

    public void PauseBuilding()
    {
        inProgress = false;
        paused = true;
        manageGame.buildingsInProgress = false;
        manageGame.buildingsInProgressSound = false;
        SetButtonsInteractable(true);
    }

    public void SetButtonsInteractable(bool value) //also disables interaction depending on the given bool
    {
        for (int i = 0; i < manageGame.buildingButtons.Count; i++)
        {
            if (!manageGame.buildingMenu.transform.GetChild(0).transform.GetChild(i).GetComponent<Button>().name.Equals("SpecialPower(Clone)"))
            {
                manageGame.buildingMenu.transform.GetChild(0).transform.GetChild(i).GetComponent<Button>().interactable = value;
            }

            
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (!GameManager.instance.inUse)
            {
                if (gameObject.GetComponent<Button>().interactable == false)
                {
                    Audiomanager.instance.PlaySound(5, 1f);
                }
                else
                {
                    Audiomanager.instance.PlaySound(4, 1f);
                    onLeft.Invoke();
                }
            }
            
        }

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (!GameManager.instance.inUse)
            {
                if (gameObject.GetComponent<Button>().interactable == false)
                {
                    Audiomanager.instance.PlaySound(5, 1f);
                }
                else
                {
                    if (inProgress || done)
                    {
                        Audiomanager.instance.PlaySound(4, 1f);
                        onRight.Invoke();
                    }
                    else
                    {
                        Audiomanager.instance.PlaySound(5, 1f);
                    }
                }
            }
        }

        if (eventData.button == PointerEventData.InputButton.Middle)
        {
            onMiddle.Invoke();
        }
    }


    //To give CostInfo script the cost of the building
    public void OnPointerEnter(PointerEventData eventData)
    {
        //CostInfo.ShowTooltip_Static(GetComponentInChildren<Text>().text + "\n" + cost.ToString());
        if (buildingStats.powerCost >= 0)
        {
            CostInfo.ShowTooltip_Static(GetComponentInChildren<Text>().text + "\nPower cost: " + buildingStats.powerCost);
        }
        else
        {
            CostInfo.ShowTooltip_Static(GetComponentInChildren<Text>().text + "\nPower gain: " + -buildingStats.powerCost); //minus to show positive value
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        CostInfo.HideTooltip_Static();
    }

    public int ChangeMoney(int amount)
    {
        int result = Mathf.RoundToInt((amount * (counter / buildingDuration)) * manageGame.USPConstructionTrainingModifier / lowPowerTimeModifier) -
            Mathf.RoundToInt(amount * ((counter - Time.deltaTime) / buildingDuration) * manageGame.USPConstructionTrainingModifier / lowPowerTimeModifier);
        return result;
    }

    private void OnDestroy()
    {
        PlayerResources.instance.Money += Mathf.RoundToInt(cost * (counter / buildingDuration));
    }

    private void ShowReadyText()
    {
        manageGame.buildingMenu.transform.Find("ReadyText").gameObject.GetComponent<Animator>().Play("textFadeIn", 0, 0.25f);
    }
}

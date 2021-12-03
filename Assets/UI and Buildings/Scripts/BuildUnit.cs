using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class BuildUnit : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector]
    public Text queueCountText;

    public UnityEvent onLeft;
    public UnityEvent onRight;
    public UnityEvent onMiddle;

    public Unit unit;

    public QueueManager queueManager;

    public GameObject unitPrefab;

    public Image progress;

    public GameManager manageGame;
    public float counter;

    public bool inProgress;
    public bool done;
    public bool paused;

    public List<Button> unlocking;

    private Queue<GameObject> localQueue = new Queue<GameObject>();

    private int substracted = 0;

    public Coroutine localCoroutine;

    void Awake()
    {
        unitPrefab = unit.model;
    }

    // Start is called before the first frame update
    void Start()
    {
        manageGame = GameObject.Find("GameManager").GetComponent<GameManager>();
        queueCountText = gameObject.transform.GetChild(3).GetComponent<Text>();
        queueManager = manageGame.buildingMenu.transform.GetChild(1).GetComponent<QueueManager>();

        foreach (Button button in manageGame.buildingButtons)
        {
            unlocking.AddRange(button.GetComponent<BuildBuilding>().unlockingProduction); //converting Button[] unlockingProduction to list
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (inProgress)
        {
            manageGame.unitsInProgress = true;
            if (PlayerResources.instance.Money > 0)
            {
                if (counter < unit.productionDuration)
                {
                    counter += Time.deltaTime * manageGame.USPConstructionTrainingModifier;
                }
                else
                {
                    counter = 0;
                }
                progress.fillAmount = Mathf.Lerp(0, 1, counter / unit.productionDuration); //progress icon fill
                substracted = ChangeMoney(unit.cost);

                PlayerResources.instance.Money -= substracted;

                if (counter >= unit.productionDuration)
                {
                    inProgress = false;
                    manageGame.unitsInProgress = false;
                    done = true;
                    counter = 0;
                    progress.fillAmount = 0;
                    //Instantiates unit when not in queue and progress is done

                    InstantiateUnit(localQueue.Dequeue());
                }
            }
        }

        if (localQueue.Count > 0)
        {
            queueCountText.text = localQueue.Count.ToString();
        }
        else
        {
            queueCountText.text = " ";
        }
    }

    public void InstantiateUnit(GameObject unitPrefab)
    {
        Instantiate(unitPrefab, new Vector3(unit.creationPlace.x, unit.creationPlace.y,
           unit.creationPlace.z), Quaternion.identity);
    }

    
    public void StartProduction()
    {
        if (paused == false)
        {
            localQueue.Enqueue(unitPrefab);

            
            inProgress = true;
            if (inProgress == true)
            {
                if(localCoroutine == null)
                {
                    localCoroutine = StartCoroutine(LocalTrainingQueue(unitPrefab));
                }
            }
            else
            {
                ResumeProduction();
            }
        }
        else
        {
            ResumeProduction();
        }
    }

    public void CancelProduction()
    {
        PlayerResources.instance.Money += Mathf.RoundToInt((unit.cost * (counter / unit.productionDuration)));

        localQueue.Dequeue();

        inProgress = false;
        manageGame.unitsInProgress = false;
        counter = 0;
        progress.fillAmount = 0;
        done = false;
    }

    IEnumerator LocalTrainingQueue(GameObject unitPrefab)
    {
        WaitForSeconds trainingTime = new WaitForSeconds(unit.productionDuration);
        while (localQueue.Count > 0)
        {
            inProgress = true; //makes progress icon in Update() work for those in queue
            yield return trainingTime;

        }
        localCoroutine = null;
    }
    
    public void ResumeProduction()
    {
        inProgress = true;
        paused = false;
    }

    public void PauseProduction()
    {
        inProgress = false;
        paused = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (!GameManager.instance.inUse)
            {
                Audiomanager.instance.PlaySound(4, 1f);
                onLeft.Invoke();
            }

        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (!GameManager.instance.inUse)
            {
                onRight.Invoke();
            }

        }
        if (eventData.button == PointerEventData.InputButton.Middle)
        {
            onMiddle.Invoke();
        }
    }

    //To give CostInfo script the cost of the unit
    public void OnPointerEnter(PointerEventData eventData)
    {
        CostInfo.ShowTooltip_Static(GetComponentInChildren<Text>().text + "\nCost: " + unit.cost.ToString());
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        CostInfo.HideTooltip_Static();
    }

    public int ChangeMoney(int amount)
    {
        int result = Mathf.RoundToInt(amount * (counter / unit.productionDuration) * manageGame.USPConstructionTrainingModifier) -
            Mathf.RoundToInt(amount * ((counter - Time.deltaTime) / unit.productionDuration) * manageGame.USPConstructionTrainingModifier);
        return result;
    }

    private void OnDestroy()
    {
        PlayerResources.instance.Money += Mathf.RoundToInt(unit.cost * (counter / unit.productionDuration));
    }
}

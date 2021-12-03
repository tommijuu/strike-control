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
                    done = true;
                    counter = 0;
                    progress.fillAmount = 0;
                    //Instantiates unit when not in queue and progress is done

                    if (queueManager.infantryQueue.Count > 0 && gameObject.tag.Equals("InfantryButton"))
                    {
                        InstantiateUnit(queueManager.infantryQueue.Dequeue());
                    }else if (queueManager.tankQueue.Count > 0 && gameObject.tag.Equals("TankButton"))
                    {
                        InstantiateUnit(queueManager.tankQueue.Dequeue());
                    }
                    else
                    {
                        InstantiateUnit(queueManager.otherQueue.Dequeue());
                    }
                    localQueue.Dequeue();

                }
            }
        }
        //if (gameObject.tag.Equals("InfantryButton"))
        //{
        //    queueCountText.text = queueManager.infantryQueue.Count.ToString(); //shows unit queue count on button
        //}else if (gameObject.tag.Equals("TankButton"))
        //{
        //    queueCountText.text = queueManager.tankQueue.Count.ToString(); //shows unit queue count on button
        //}
        //else
        //{
        //    queueCountText.text = queueManager.otherQueue.Count.ToString();
        //}

        if (localQueue.Count > 0)
        {
            queueCountText.text = localQueue.Count.ToString();
        }
        else
        {
            queueCountText.text = " ";
        }
        
        //if (queueManager.infantryQueue.Count <= 0) //so it doesn't display the zero when there isn't a queue
        //{
        //    queueCountText.text = " ";
        //}
        //if(queueManager.tankQueue.Count <= 0)
        //{
        //    queueCountText.text = " ";
        //}
    }

    public void InstantiateUnit(GameObject unitPrefab)
    {
        Instantiate(unitPrefab, new Vector3(unit.creationPlace.x + 3, unit.creationPlace.y + 1,
           unit.creationPlace.z), Quaternion.identity);
    }

    //public void SpawnUnit(GameObject temp)
    //{
    //    Instantiate(temp, new Vector3(unit.creationPlace.x + 3, unit.creationPlace.y + 1,
    //       unit.creationPlace.z), Quaternion.identity);
    //}


    public void StartProduction()
    {
        if (paused == false)
        {
            if (gameObject.tag.Equals("InfantryButton"))
            {
                queueManager.infantryQueue.Enqueue(unitPrefab);
            }
            else if (gameObject.tag.Equals("TankButton"))
            {
                queueManager.tankQueue.Enqueue(unitPrefab);
            }
            else
            {
                queueManager.otherQueue.Enqueue(unitPrefab);
            }
            
            localQueue.Enqueue(unitPrefab);
            
            //PlayerResources.instance.Money -= unit.cost; //substracts the queued units' costs but not in the fancy manner
            inProgress = true;
            if (inProgress == true)
            {
                
                if (queueManager.infantryCoroutine == null && gameObject.tag.Equals("InfantryButton"))
                {
                    queueManager.infantryCoroutine = StartCoroutine(queueManager.InfantryTrainingQueue(gameObject.GetComponent<Button>()));
                }

                if (queueManager.tankCoroutine == null && gameObject.tag.Equals("TankButton"))
                {
                    queueManager.tankCoroutine = StartCoroutine(queueManager.TankTrainingQueue(gameObject.GetComponent<Button>()));
                }

                if (queueManager.otherCoroutine == null)
                {
                    queueManager.otherCoroutine = StartCoroutine(queueManager.OtherTrainingQueue(gameObject.GetComponent<Button>()));
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

        inProgress = false;
        counter = 0;
        progress.fillAmount = 0;
        done = false;

        if (gameObject.tag.Equals("InfantryButton"))
        {
            queueManager.infantryQueue.Dequeue();
        }
        else if (gameObject.tag.Equals("TankButton"))
        {
            queueManager.tankQueue.Dequeue();
        }
        else
        {
            queueManager.otherQueue.Dequeue();
        }

        localQueue.Dequeue();

    }

    public void TerminateQueues()
    {
        //TODO: Return the $$$$$ and figure out the right place to call this

        if(queueManager.infantryQueue.Count > 0)
        {
            while(queueManager.infantryQueue.Count > 0)
                queueManager.infantryQueue.Dequeue();
        }
        if(queueManager.tankQueue.Count > 0)
        {
            while (queueManager.tankQueue.Count > 0)
                queueManager.tankQueue.Dequeue();
        }
        if(queueManager.otherQueue.Count > 0)
        {
            while (queueManager.otherQueue.Count > 0)
                queueManager.otherQueue.Dequeue();
        }
    }

    //IEnumerator UnitTrainingQueue()
    //{
    //    WaitForSeconds trainingTime = new WaitForSeconds(unit.productionDuration);
    //    while (infantryQueue.Count > 0)
    //    {
    //        inProgress = true; //makes progress icon in Update() work for those in queue
    //        yield return trainingTime;
    //        queueCountText.text = (infantryQueue.Count - 1).ToString(); //update text when a queued unit has spawned
            
    //    }
    //    infantryCoroutine = null;
    //}

    //IEnumerator InfantryTrainingQueue()
    //{
    //    WaitForSeconds trainingTime = new WaitForSeconds(unit.productionDuration);
    //    while (infantryQueue.Count > 0)
    //    {
    //        inProgress = true; //makes progress icon in Update() work for those in queue
    //        yield return trainingTime;
    //        queueCountText.text = (infantryQueue.Count - 1).ToString(); //update text when a queued unit has spawned

    //    }
    //    infantryCoroutine = null;
    //}

    //IEnumerator TankTrainingQueue()
    //{
    //    WaitForSeconds trainingTime = new WaitForSeconds(unit.productionDuration);
    //    while (infantryQueue.Count > 0)
    //    {
    //        inProgress = true; //makes progress icon in Update() work for those in queue
    //        yield return trainingTime;
    //        queueCountText.text = (infantryQueue.Count - 1).ToString(); //update text when a queued unit has spawned

    //    }
    //    tankCoroutine = null;
    //}

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
        CostInfo.ShowTooltip_Static(GetComponentInChildren<Text>().text + "\n" + unit.cost.ToString());
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


/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class BuildUnit : MonoBehaviour, IPointerClickHandler
{
    public UnityEvent onLeft;
    public UnityEvent onRight;
    public UnityEvent onMiddle;

    public Unit unit;

    public Image progress;

    public GameManager manageGame;
    public float counter;

    public bool inProgress;
    public bool done;
    public bool paused;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            onLeft.Invoke();
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            onRight.Invoke();
        }
        if (eventData.button == PointerEventData.InputButton.Middle)
        {
            onMiddle.Invoke();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        manageGame = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inProgress)
        {
            if (manageGame.money > 0)
            {
                if (counter < unit.productionDuration)
                {
                    counter += Time.deltaTime;
                }
                else
                {
                    counter = 0;
                }

                progress.fillAmount = Mathf.Lerp(0, 1, counter / unit.productionDuration);
                int substracted = ChangeMoney(unit.cost);

                manageGame.money -= substracted;

                if (counter >= unit.productionDuration)
                {
                    inProgress = false;
                    done = true;
                    counter = 0;
                    progress.fillAmount = 0;
                    //INSTANTIATING UNIT
                    InstantiateUnit();
                }
            }
        }
    }

    public void InstantiateUnit()
    {
        Instantiate(unit.model, new Vector3(unit.creationPlace.x + 3, unit.creationPlace.y + 1,
           unit.creationPlace.z), Quaternion.identity);
    }

    public void StartProduction()
    {
        Debug.Log("Start Production");
        if (paused == false)
        {
            if (inProgress == true)
            {
                PauseProduction();
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
        manageGame.money += Mathf.RoundToInt(unit.cost * (counter / unit.productionDuration));
        inProgress = false;

        counter = 0;
        progress.fillAmount = 0;
        done = false;
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

    public int ChangeMoney(int amount)
    {
        int result = Mathf.RoundToInt(amount * (counter / unit.productionDuration)) -
            Mathf.RoundToInt(amount * ((counter - Time.deltaTime) / unit.productionDuration));
        return result;
    }
}
*/

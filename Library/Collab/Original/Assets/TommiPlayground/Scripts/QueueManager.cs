using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QueueManager : MonoBehaviour
{
   [HideInInspector]
    public Queue<GameObject> infantryQueue = new Queue<GameObject>();
    [HideInInspector]
    public Queue<GameObject> machineGunnerQueue = new Queue<GameObject>();
    [HideInInspector]
    public Queue<GameObject> bazookaQueue = new Queue<GameObject>();
    [HideInInspector]
    public Queue<GameObject> jeepQueue = new Queue<GameObject>();

    [HideInInspector]
    public Queue<GameObject> tankQueue = new Queue<GameObject>();
    [HideInInspector]
    public Queue<GameObject> lightTankQueue = new Queue<GameObject>();
    [HideInInspector]
    public Queue<GameObject> heavyTankQueue = new Queue<GameObject>();

    [HideInInspector]
    public Queue<GameObject> otherQueue = new Queue<GameObject>();

    [HideInInspector]
    public Coroutine infantryCoroutine;

    [HideInInspector]
    public Coroutine tankCoroutine;
    [HideInInspector]
    public Coroutine otherCoroutine;
    [HideInInspector]
    public GameManager manageGame;

    public Coroutine machineGunnerCoroutine;
    public Coroutine bazookaCoroutine;
    public Coroutine jeepCoroutine;

    public Coroutine lightTankCoroutine;
    public Coroutine heavyTankCoroutine;

    void Start()
    {
        manageGame = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        if (infantryQueue.Count > 0 || tankQueue.Count > 0 || otherQueue.Count > 0)
        {
            manageGame.unitsInProgress = true;
        }
        else
        {
            manageGame.unitsInProgress = false;
        }
    }
    //public IEnumerator InfantryTrainingQueue(Button button)
    //{

    //    WaitForSeconds trainingTime = new WaitForSeconds(button.GetComponent<BuildUnit>().unit.productionDuration);
    //    while (infantryQueue.Count > 0)
    //    {
    //        button.GetComponent<BuildUnit>().inProgress = true; //makes progress icon in Update() work for those in queue

    //        yield return trainingTime;
    //        if (button.name.Equals("MachineGunner(Clone)"))
    //        {
    //            button.GetComponent<BuildUnit>().queueCountText.text = (machineGunnerQueue.Count - 1).ToString();
    //        }
    //        if (button.name.Equals("Bazooka(Clone)"))
    //        {
    //            button.GetComponent<BuildUnit>().queueCountText.text = (bazookaQueue.Count - 1).ToString();
    //        }
    //        if (button.name.Equals("Jeep(Clone)"))
    //        {
    //            button.GetComponent<BuildUnit>().queueCountText.text = (jeepQueue.Count - 1).ToString();
    //        }

    //    }
    //    infantryCoroutine = null;
    //}

    public IEnumerator TankTrainingQueue(Button button)
    {
        WaitForSeconds trainingTime = new WaitForSeconds(button.GetComponent<BuildUnit>().unit.productionDuration);
        while (tankQueue.Count > 0)
        {
            button.GetComponent<BuildUnit>().inProgress = true; //makes progress icon in Update() work for those in queue
            yield return trainingTime;
            if (button.name.Equals("LightTank(Clone)"))
            {
                button.GetComponent<BuildUnit>().queueCountText.text = (lightTankQueue.Count - 1).ToString();
            }
            if (button.name.Equals("HeavyTank(Clone)"))
            {
                button.GetComponent<BuildUnit>().queueCountText.text = (heavyTankQueue.Count - 1).ToString();
            }

        }
        tankCoroutine = null;
    }

    public IEnumerator OtherTrainingQueue(Button button)
    {
        WaitForSeconds trainingTime = new WaitForSeconds(button.GetComponent<BuildUnit>().unit.productionDuration);
        while (otherQueue.Count > 0)
        {
            button.GetComponent<BuildUnit>().inProgress = true; //makes progress icon in Update() work for those in queue
            yield return trainingTime;
            button.GetComponent<BuildUnit>().queueCountText.text = (otherQueue.Count - 1).ToString(); //update text when a queued unit has spawned

        }
        otherCoroutine = null;
    }


    public IEnumerator MachineGunnerQueue(Button button, GameObject unitPrefab)
    {
        WaitForSeconds trainingTime = new WaitForSeconds(button.GetComponent<BuildUnit>().unit.productionDuration);
        
        while (machineGunnerQueue.Count > 0)
        {
            button.GetComponent<BuildUnit>().inProgress = true; //makes progress icon in Update() work for those in queue
            
            yield return trainingTime;
            infantryQueue.Enqueue(unitPrefab);
        }

        
        machineGunnerCoroutine = null;
        
    }

    public IEnumerator BazookaQueue(Button button, GameObject unitPrefab)
    {
        WaitForSeconds trainingTime = new WaitForSeconds(button.GetComponent<BuildUnit>().unit.productionDuration);
        
        while (bazookaQueue.Count > 0)
        {
            button.GetComponent<BuildUnit>().inProgress = true; //makes progress icon in Update() work for those in queue
            
            yield return trainingTime;
            infantryQueue.Enqueue(unitPrefab);

        }

        
        bazookaCoroutine = null;
        
    }

    public IEnumerator JeepQueue(Button button, GameObject unitPrefab)
    {

        WaitForSeconds trainingTime = new WaitForSeconds(button.GetComponent<BuildUnit>().unit.productionDuration);
        
        while (jeepQueue.Count > 0)
        {
            button.GetComponent<BuildUnit>().inProgress = true; //makes progress icon in Update() work for those in queue
            
            yield return trainingTime;
            infantryQueue.Enqueue(unitPrefab);


        }

        
        jeepCoroutine = null;
        
    }

    //public IEnumerator LightTankQueue(Button button, GameObject unit)
    //{
    //    WaitForSeconds trainingTime = new WaitForSeconds(button.GetComponent<BuildUnit>().unit.productionDuration);
       
    //    while (lightTankQueue.Count > 0)
    //    {
    //        button.GetComponent<BuildUnit>().inProgress = true; //makes progress icon in Update() work for those in queue
            
    //        yield return trainingTime;
            

    //    }
        
        
    //    lightTankCoroutine = null;
        
    //}

    //public IEnumerator HeavyTankQueue(Button button, GameObject unit)
    //{
    //    WaitForSeconds trainingTime = new WaitForSeconds(button.GetComponent<BuildUnit>().unit.productionDuration);
        
    //    while (heavyTankQueue.Count > 0)
    //    {
    //        button.GetComponent<BuildUnit>().inProgress = true; //makes progress icon in Update() work for those in queue
            
    //        yield return trainingTime;
            
    //    }
        
        
    //    heavyTankCoroutine = null;
        
    //}
}

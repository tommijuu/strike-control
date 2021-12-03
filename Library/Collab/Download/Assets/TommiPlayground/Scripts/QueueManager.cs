using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QueueManager : MonoBehaviour
{
 
    //Gathererqueue???

    public Queue<GameObject> infantryQueue = new Queue<GameObject>();

    public Queue<GameObject> tankQueue = new Queue<GameObject>();

    public Queue<GameObject> otherQueue = new Queue<GameObject>();

    public Coroutine infantryCoroutine;

    public Coroutine tankCoroutine;

    public Coroutine otherCoroutine;

    public GameManager manageGame;

    void Start()
    {
        manageGame = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        if(infantryQueue.Count > 0 || tankQueue.Count > 0 || otherQueue.Count > 0)
        {
            manageGame.unitsInProgressSound = true;
        }
        else
        {
            manageGame.unitsInProgressSound = false;
        }
    }
    public IEnumerator InfantryTrainingQueue(Button button)
    {
        WaitForSeconds trainingTime = new WaitForSeconds(button.GetComponent<BuildUnit>().unit.productionDuration);
        while (infantryQueue.Count > 0)
        {
            button.GetComponent<BuildUnit>().inProgress = true; //makes progress icon in Update() work for those in queue
            yield return trainingTime;
            button.GetComponent<BuildUnit>().queueCountText.text = (infantryQueue.Count - 1).ToString(); //update text when a queued unit has spawned

        }
        infantryCoroutine = null;
    }

    public IEnumerator TankTrainingQueue(Button button)
    {
        WaitForSeconds trainingTime = new WaitForSeconds(button.GetComponent<BuildUnit>().unit.productionDuration);
        while (tankQueue.Count > 0)
        {
            button.GetComponent<BuildUnit>().inProgress = true; //makes progress icon in Update() work for those in queue
            yield return trainingTime;
            button.GetComponent<BuildUnit>().queueCountText.text = (tankQueue.Count - 1).ToString(); //update text when a queued unit has spawned

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
}

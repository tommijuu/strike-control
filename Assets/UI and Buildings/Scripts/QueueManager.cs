using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QueueManager : MonoBehaviour
{
    //THIS WAS ORIGINALLY FOR BUILDING SPECIFIC QUEUES, NOW HANDLING ONLY PROGRESS SOUNDS (should probably be for example in gamemanager)

    public GameManager manageGame;

    void Start()
    {
        manageGame = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        if(manageGame.unitsInProgress)
        {
            manageGame.unitsInProgressSound = true;
        }
        else
        {
            manageGame.unitsInProgressSound = false;
        }
    }
}

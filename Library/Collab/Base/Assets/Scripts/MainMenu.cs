using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    //CustomGame script uses these to set up the game

    [HideInInspector]
    public GameObject lastFactionDescription;
    [HideInInspector]
    public GameObject lastDifficultyDescription;

    public int playerFaction;
    public int playerDifficulty;

    public LevelManager levelManager;

    private void Awake()
    {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();

        if(levelManager.isFullscreen == true)
        {
            Screen.fullScreen = true;
        }
        else
        {
            Screen.fullScreen = false;
        }
    }

    public void StartMission()
    {
        Debug.Log("Player chose faction: " + playerFaction);
        Debug.Log("Player chose difficulty: " + playerDifficulty);
        SceneManager.LoadScene("MainScene");
    }

    public void Campaign()
    {
        Debug.Log("Campaign not available yet");
    }

    public void Quit()
    {
        Debug.Log("Git gud");
        Application.Quit();
    }

   
    
}

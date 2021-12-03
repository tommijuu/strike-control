using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private bool isGameOver = false;

    public Button[] menuIcons;

    [HideInInspector]
    public List<Button> buildingButtons;

    public Canvas buildingMenu;

    public PauseMenu pauseMenu;

    public Text gameOverText;

    public bool IsGameOver { get { return isGameOver; } set { value = isGameOver; } }

    public bool buildingsInProgress;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        pauseMenu = GameObject.Find("CanvasPause").GetComponent<PauseMenu>();
        pauseMenu.Resume();
        //Instantiating building buttons
        //foreach (Button menuIcon in menuIcons)
        //{
        //    Button buildingButton = Instantiate(menuIcon);
        //    buildingButton.transform.SetParent(buildingMenu.transform.GetChild(0), false);
        //    //worldPosition set to false makes child keep its local orientation and not stretch
        //}
        Button oilRefinery = menuIcons[0];
        AddBuildingButton(oilRefinery);

        Button powerPlant = menuIcons[1];
        AddBuildingButton(powerPlant);
    }

    public void AddBuildingButton(Button button)
    {
        buildingButtons.Add(button);
        Button buildingButton = Instantiate(button);
        buildingButton.transform.SetParent(buildingMenu.transform.GetChild(0), false);
    }

    //Instantiating unit buttons
    public void AddProductionButton(Button button)
    {
        Button unitButton = Instantiate(button);
        unitButton.transform.SetParent(buildingMenu.transform.GetChild(1), false);
        //worldPosition set to false makes child keep its local orientation and not stretch
    }

    public void RemoveProductionButton(Button button)
    {
        GameObject RemoveProduction = GameObject.Find(button.name + "(Clone)");
        Destroy(RemoveProduction);
    }

    public void RemoveBuildingButton(Button button)
    {
        buildingButtons.Remove(button);
        GameObject RemoveBuilding = GameObject.Find(button.name + "(Clone)");
        Destroy(RemoveBuilding);
    }

    public void GameOver(bool isWin)
    {
        if (isWin)
        {
            gameOverText.text = "Mission Complete";
        }
        else
        {
            gameOverText.text = "Mission Failed";
        }

        //gameOverText.gameObject.SetActive(true);
        isGameOver = true;
        Time.timeScale = 0;
    }
}
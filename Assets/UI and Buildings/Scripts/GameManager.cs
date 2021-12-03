using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public DisplayScore displayScore;
    private DatabaseAccess databaseAccess;
    private FactionManager fm;
    private LevelManager lm;
    private WaveManager wm;
    private GameSettings gs;

    public int score;
    public string playerFactionString;
    public string difficultyString;

    private bool isGameOver = false;

    public Button[] menuIcons;

    [HideInInspector]
    public List<Button> buildingButtons; //to keep track of building buttons that are currently in the game

    [HideInInspector]
    public List<Button> unitButtons; //to keep track of unit buttons that are currently in the game

    public GameObject audioListener;

    public Canvas buildingMenu;

    public PauseMenu pauseMenu;
    public CameraController cameraController;
    public GameObject gameOverPanel;

    public Text gameOverText;

    public bool IsGameOver { get { return isGameOver; } set { value = isGameOver; } }

    public bool buildingsInProgress;
    public bool unitsInProgress;

    public bool buildingsInProgressSound;
    public bool unitsInProgressSound;

    public bool inSell;
    public bool inRepair;
    public bool builderActive;

    public bool inUse;

    public bool paused;

    public bool empireSpecialPowerActive;
    public int ESPMoneyModifier;

    public bool unionSpecialPowerActive;
    public int USPConstructionTrainingModifier;

    public QueueManager queueManager;

    public float limiterReset = 0.1f;
    private List<string> activeSound = new List<string>();

    private string filePath;
    string saveUrl = "http://localhost:5000/highscore/create";

    // Start is called before the first frame update
    void Start()
    {


        instance = this;
        filePath = Path.Combine(Application.dataPath, "save.txt");
        score = 0;
        displayScore = GameObject.Find("DynamicUI").GetComponent<DisplayScore>();
        databaseAccess = GameObject.FindGameObjectWithTag("DatabaseAccess").GetComponent<DatabaseAccess>();
        fm = GameObject.FindGameObjectWithTag("FactionManager").GetComponent<FactionManager>();
        lm = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        wm = GameObject.FindGameObjectWithTag("WaveManager").GetComponent<WaveManager>();
        gs = GameObject.FindGameObjectWithTag("GameSettings").GetComponent<GameSettings>();

        cameraController = GameObject.Find("CameraController").GetComponent<CameraController>();
        pauseMenu = GameObject.Find("CanvasPause").GetComponent<PauseMenu>();
        pauseMenu.Resume();
        queueManager = buildingMenu.transform.Find("UnitPanel").GetComponent<QueueManager>();

        Button oilRefinery = menuIcons[0];
        AddBuildingButton(oilRefinery);

        Button powerPlant = menuIcons[1];
        AddBuildingButton(powerPlant);

        if (Audiomanager.instance.IsMainSceneMusicMuted)
        {
            Audiomanager.instance.IsMainSceneMusicMuted = false;
            StartCoroutine(Audiomanager.instance.MusicPlayList());
        }

        //Setting up the strings of faction and difficulty for saving the leaderboards
        if (fm.PlayerFactionChoice == 0)
        {
            playerFactionString = "Empire";
        }
        else if (fm.PlayerFactionChoice == 1)
        {
            playerFactionString = "Union";
        }
        else
        {
            playerFactionString = "Cult";
        }

        if (lm.difficulty == 0)
        {
            difficultyString = "Easy";
        }
        else if (lm.difficulty == 1)
        {
            difficultyString = "Normal";
        }
        else if (lm.difficulty == 2)
        {
            difficultyString = "Hard";
        }
        else
        {
            difficultyString = "Impossible";
        }


    }

    private void Update()
    {
        if (inSell || inRepair || builderActive)
        {
            inUse = true;
        }
        else
        {
            inUse = false;
        }

        if (empireSpecialPowerActive)
        {
            ESPMoneyModifier = 2;
        }
        else
        {
            ESPMoneyModifier = 1;
        }

        if (unionSpecialPowerActive)
        {
            USPConstructionTrainingModifier = 2;
        }
        else
        {
            USPConstructionTrainingModifier = 1;
        }
    }


    public void AddBuildingButton(Button button)
    {
        buildingButtons.Add(button);
        Button buildingButton = Instantiate(button);
        //worldPosition set to false makes child keep its local orientation and not stretch
        buildingButton.transform.SetParent(buildingMenu.transform.GetChild(0), false);
    }

    //Instantiating unit buttons
    public void AddProductionButton(Button button)
    {
        unitButtons.Add(button);
        Button unitButton = Instantiate(button);
        //worldPosition set to false makes child keep its local orientation and not stretch
        unitButton.transform.SetParent(buildingMenu.transform.GetChild(1), false);
    }

    public void RemoveProductionButton(Button button)
    {
        unitButtons.Remove(button);
        GameObject RemoveProduction = GameObject.Find(button.name + "(Clone)");

        unitsInProgress = false;
        Destroy(RemoveProduction);
    }

    public void RemoveBuildingButton(Button button)
    {
        buildingButtons.Remove(button);
        GameObject RemoveBuilding = GameObject.Find(button.name + "(Clone)");
        Destroy(RemoveBuilding);
    }

    public void GameWin(bool isWin)
    {
        gameOverPanel.SetActive(true);
        cameraController.CameraGameOver();

        if (isWin)
        {
            gameOverText.text = "Mission Complete";
            isGameOver = true;
            //Save username, difficulty, wavecount and score to database here upon victory
            HighScore highScore = new HighScore
            {
                UserName = gs.playerName,
                Faction = playerFactionString,
                Difficulty = difficultyString,
                Wave = wm.WaveCount,
                Score = score
            };
            databaseAccess.SaveScore(highScore);
            Time.timeScale = 0;
        }
        else
        {
            gameOverText.text = "Mission Failed";
            StartCoroutine(MissionFailedDelay());
        }
    }

    IEnumerator MissionFailedDelay()
    {
        yield return new WaitForSeconds(0.4f);
        isGameOver = true;
        //Save username, difficulty, wavecount and score to database here upon defeat
        HighScore highScore = new HighScore
        {
            UserName = gs.playerName,
            Faction = playerFactionString,
            Difficulty = difficultyString,
            Wave = wm.WaveCount,
            Score = score
        };
        databaseAccess.SaveScore(highScore);
        Time.timeScale = 0;
    }


    public bool SoundBuffer(string name)
    {
        if (activeSound.Contains(name))
        {
            return false;
        }
        activeSound.Add(name);
        StartCoroutine(SoundlimiterReset());
        return true;
    }

    IEnumerator SoundlimiterReset()
    {
        yield return new WaitForSeconds(limiterReset);
        activeSound.Clear();
    }
}
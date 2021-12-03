using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

    public static bool isPaused = false;

    public GameObject pauseMenuUI;
    public GameObject optionsMenu;

    public GameSettings gameSettings;

    private void Awake()
    {
        if (FindObjectOfType<LevelManager>())
        {
            gameSettings = GameObject.Find("GameSettings").GetComponent<GameSettings>();

            if (gameSettings.isFullscreen == true)
            {
                Screen.fullScreen = true;
            }
            else
            {
                Screen.fullScreen = false;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Audiomanager.instance.PlaySound(4, 1f);
                Resume();
            }
            else
            {
                Audiomanager.instance.PlaySound(4, 1f);
                Pause();
            }
        }
    }

    public void Resume()
    {
        GameManager.instance.paused = false;
        pauseMenuUI.SetActive(false);
        isPaused = false;
        if (!GameManager.instance.IsGameOver)
            Time.timeScale = 1f;

        pauseMenuUI.transform.GetChild(0).gameObject.SetActive(true);
        optionsMenu.SetActive(false);
    }

    void Pause()
    {
        GameManager.instance.paused = true;
        pauseMenuUI.SetActive(true);
        isPaused = true;

        if (!GameManager.instance.IsGameOver)
            Time.timeScale = 0f;
    }

    public void LoadMenu()
    {
        Pause();
    }

    public void ReloadScene()
    {
        isPaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainScene");
    }

    public void QuitGame()
    {
        Audiomanager.instance.StopMusic();
        Destroy(GameObject.Find("AudioManager"));
        LevelManager.instance.FactionChoice = 0;
        LevelManager.instance.WaveCountChoice = 5;
        SceneManager.LoadScene("MainMenu");
    }
}

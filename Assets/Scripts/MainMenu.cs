using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{
    //CustomGame script uses these to set up the game

    [HideInInspector]
    public GameObject lastFactionDescription;
    [HideInInspector]
    public GameObject lastDifficultyDescription;

    public GameObject inputField;

    public int playerFaction;
    public int playerDifficulty;

    public GameSettings gameSettings;

    public SetSettings setSettings;

    public AudioMixer mainMixer;

    void Awake()
    {
        mainMixer = setSettings.mainMixer;
        if (FindObjectOfType<GameSettings>())
        {
            gameSettings = GameObject.Find("GameSettings").GetComponent<GameSettings>();

            if (gameSettings.isFullscreen == true)
            {
                Screen.fullScreen = true;
                gameSettings.isFullscreen = true;
            }
            else
            {
                Screen.fullScreen = false;
                gameSettings.isFullscreen = false;
            }
        }
    }

    void Start()
    {
        if (gameSettings.isFirstPlay)
        {
            mainMixer.SetFloat("MasterVol", Mathf.Log10(1) * 20);
            mainMixer.SetFloat("MusicVol", Mathf.Log10(0.25f) * 20);
            mainMixer.SetFloat("VoiceActingVol", Mathf.Log10(0.75f) * 20);
            mainMixer.SetFloat("EffectsVol", Mathf.Log10(0.75f) * 20);

            Screen.fullScreen = true;
            gameSettings.isFullscreen = true;
        }
        else //not first time playing
        {
            setSettings.masterSlider.value = gameSettings.masterVolume;
            setSettings.musicSlider.value = gameSettings.musicVolume;
            setSettings.voiceActingSlider.value = gameSettings.voiceActingVolume;
            setSettings.soundEffectsSlider.value = gameSettings.soundEffectsVolume;
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

    public void SavePlayerName()
    {
        gameSettings.playerName = inputField.GetComponent<Text>().text;
    }
}

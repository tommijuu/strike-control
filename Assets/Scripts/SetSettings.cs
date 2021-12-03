using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetSettings : MonoBehaviour
{
    private float masterFloat, musicFloat, voiceActingFloat, soundEffectsFloat;

    public Slider masterSlider, musicSlider, voiceActingSlider, soundEffectsSlider;
    public AudioMixer mainMixer;

    public bool isFullscreen;
    public GameSettings gameSettings;

    public Toggle fullToggle, windowToggle;

    void Awake()
    {
        gameSettings = GameObject.Find("GameSettings").GetComponent<GameSettings>();
        isFullscreen = gameSettings.isFullscreen;
    }

    void Start()
    {
        if (gameSettings.isFirstPlay)
        {
            masterFloat = 1f;
            musicFloat = 0.25f;
            voiceActingFloat = 0.75f;
            soundEffectsFloat = 0.75f;

            isFullscreen = true;
            fullToggle.isOn = isFullscreen;
            windowToggle.isOn = !isFullscreen;
            Screen.fullScreen = isFullscreen;

            masterSlider.value = masterFloat;
            musicSlider.value = musicFloat;
            voiceActingSlider.value = voiceActingFloat;
            soundEffectsSlider.value = soundEffectsFloat;
        }
        else
        {
            masterSlider.value = gameSettings.masterVolume;
            musicSlider.value = gameSettings.musicVolume;
            voiceActingSlider.value = gameSettings.voiceActingVolume;
            soundEffectsSlider.value = gameSettings.soundEffectsVolume;

            if (isFullscreen == true)
            {
                fullToggle.isOn = isFullscreen;
                windowToggle.isOn = !isFullscreen;
                Screen.fullScreen = isFullscreen;
            }
            else //isFullscreen = false
            {
                windowToggle.isOn = !isFullscreen;
                fullToggle.isOn = isFullscreen;
                Screen.fullScreen = isFullscreen;
            }
        }
    }

    public void SetBool(string name, bool isFullScreen)
    {
        PlayerPrefs.SetInt(name, isFullScreen ? 1 : 0);
    }

    public bool GetBool(string name)
    {
        return PlayerPrefs.GetInt(name) == 1 ? true : false;
    }

    public void SetMasterVolume(float sliderValue)
    {
        mainMixer.SetFloat("MasterVol", Mathf.Log10(sliderValue) * 20);
    }

    public void SetMusicVolume(float sliderValue)
    {
        mainMixer.SetFloat("MusicVol", Mathf.Log10(sliderValue) * 20);
    }

    public void SetVoiceActingVolume(float sliderValue)
    {
        mainMixer.SetFloat("VoiceActingVol", Mathf.Log10(sliderValue) * 20);
    }

    public void SetEffectsVolume(float sliderValue)
    {
        mainMixer.SetFloat("EffectsVol", Mathf.Log10(sliderValue) * 20);
    }

    public void SaveSettings()
    {
        gameSettings.masterVolume = masterSlider.value;
        gameSettings.musicVolume = musicSlider.value;
        gameSettings.voiceActingVolume = voiceActingSlider.value;
        gameSettings.soundEffectsVolume = soundEffectsSlider.value;
        gameSettings.isFirstPlay = false;
        gameSettings.isFullscreen = isFullscreen;
    }

    void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            SaveSettings();
        }
    }

    public void FullScreen()
    {
        Screen.fullScreen = true;
        gameSettings.isFullscreen = true;
        isFullscreen = true;
    }

    public void WindowedFullScreen()
    {
        Screen.fullScreen = false;
        gameSettings.isFullscreen = false;
        isFullscreen = false;
    }
}

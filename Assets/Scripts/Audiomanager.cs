using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Audiomanager : MonoBehaviour
{
    public static Audiomanager instance;

    public AudioSource soundSource;
    public AudioSource musicSource;
    public AudioSource constantAudioSource;
    public AudioSource specialAudioSource;
    public AudioSource PausedUISource;

    public GameManager manageGame;

    public float resetter = 0.2f;
    bool CombatSoundLimiter = true;

    public bool IsActive = false;

    //arrays
    public AudioClip[] audioClips;
    public AudioClip[] musicTracks;

    private AudioClip currentTrack;

    private List<AudioClip> currentlyPlaying = new List<AudioClip>();

    public float SoundSliderValue { get; set; } = 1f;
    public float MusicSliderValue { get; set; } = 1f;

    private bool isMainSceneMusicMuted = true;
    private bool soundIsPaused = false;

    public int constructionCounter = 0;

    public bool IsMainSceneMusicMuted { get { return isMainSceneMusicMuted; } set { isMainSceneMusicMuted = value; } }

    // Start is called before the first frame update
    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        manageGame = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (manageGame.paused || manageGame.IsGameOver)
        {
            if (!soundIsPaused)
            {
                soundSource.Pause();
                constantAudioSource.Pause();
                specialAudioSource.Pause();
                soundIsPaused = true;
            }
            
        }
        else
        {
            if (soundIsPaused)
            {
                soundSource.UnPause();
                constantAudioSource.UnPause();
                specialAudioSource.UnPause();
                soundIsPaused = false;
            }
            
        }
        if (Input.GetKeyUp(KeyCode.M))
        {
            if (isMainSceneMusicMuted)
            {
                isMainSceneMusicMuted = false;
                StartCoroutine(MusicPlayList());
            }
            else
            {
                isMainSceneMusicMuted = true;
                StopCoroutine(MusicPlayList());
                StopMusic();
            }
        }

       if (!manageGame.paused && PlayerResources.instance.Money > 0 && (manageGame.buildingsInProgressSound || manageGame.unitsInProgressSound))
        {
            if (!constantAudioSource.isPlaying)
            {
                constantAudioSource.Play();
            }
        }
        else
        {
            if (constantAudioSource.isPlaying)
            {
                constantAudioSource.Stop();
            }
        }

    }

    public void PlaySoundCombat2(AudioClip[] array, float volume)
    {
        int index = Random.Range(0, array.Length);
        soundSource.PlayOneShot(array[index], volume);
    }

    public void PlaySoundCombat(AudioClip[] array, float volume)
    {
        IsActive = true;

        for (int i = 0; i < array.Length; i++)
        {
            if (currentlyPlaying.Contains(array[i]))
            {
                Debug.Log("already playing");
                IsActive = false;
                return;
            }

            Debug.Log("pew");
            int index = Random.Range(0, array.Length);
            currentlyPlaying.Add(array[index]);
            soundSource.PlayOneShot(array[index], volume);
            IsActive = false;

        }
    }

    public void PlaySound(int index, float volume)
    {
        soundSource.PlayOneShot(audioClips[index], volume);
    }

    public void PlaySpecialSound(int index, float volume)
    {
        specialAudioSource.PlayOneShot(audioClips[index], volume);
    }

    public void PlaySoundVariation(int[] array, float volume)
    {
        int i = array[Random.Range(0, array.Length)];
        soundSource.PlayOneShot(audioClips[i], volume);
    }

    public IEnumerator PlaySoundRepeat(int index, float volume, int repeats)
    {
        for (int i = 0; i < repeats; i++)
        {
            soundSource.PlayOneShot(audioClips[index], volume);
            yield return new WaitForSeconds(audioClips[index].length);
        }
    }

    public void PlayCombatSound(string name, float volume)
    {
        if (CombatSoundLimiter)
        {
            CombatSoundLimiter = false;

            if (name.Equals("PU(NoWeapon)(Clone)"))
            {
                PlaySoundVariation(new int[] { 26, 27, 28, 29 }, volume);
            }
            if (name.Equals("EU(NoWeapon)(Clone)"))
            {
                PlaySoundVariation(new int[] { 26, 27, 28, 29 }, volume);
            }
            else
            {

            }
            StartCoroutine(ResetCombatSoundLimiter());
        }
    }

    IEnumerator ResetCombatSoundLimiter()
    {
        yield return new WaitForSeconds(.2f);
        CombatSoundLimiter = true;
    }

    public void PlayMusic(int index, float volume)
    {
        musicSource.Stop();
        musicSource.volume = volume;
        musicSource.clip = musicTracks[index];
        currentTrack = musicTracks[index];
        musicSource.Play();
    }

    public void ButtonClickSound()
    {
        PausedUISource.PlayOneShot(audioClips[4], 1f);
    }

    public void StopMusic()
    {
        musicSource.Stop();
        if (!isMainSceneMusicMuted)
        {
            isMainSceneMusicMuted = true;
            StopCoroutine(MusicPlayList());
        }
    }

    void resetcurrentlyPlaying()
    {
        currentlyPlaying.Clear();
        Debug.Log("currently playing reset");
    }

    public bool SoundBuffer()
    {
        if (CombatSoundLimiter)
        {
            CombatSoundLimiter = false;
            StartCoroutine(ResetCombatSoundLimiter());
            return true;
        }
        else
        {
            return false;
        }

    }

    public IEnumerator MusicPlayList()
    {
        while (true)
        {
            if (!musicSource.isPlaying)
            {
                Debug.Log("starting new music track");
                int i = Random.Range(0, musicTracks.Length);
                PlayMusic(i, 1f);
                Debug.Log("music track length " + musicTracks[i].length + " seconds");  
            }
            yield return null;
        }
        
    }
    public void IncreaseCC()
    {
        constructionCounter += 1;
    }
    public void DecreaseCC()
    {
        constructionCounter -= 1;
    }
}

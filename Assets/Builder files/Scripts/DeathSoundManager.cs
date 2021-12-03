using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathSoundManager : MonoBehaviour
{

    public AudioSource voiceSource;

    private bool soundIsPaused = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.paused || GameManager.instance.IsGameOver)
        {
            if (!soundIsPaused)
            {
                voiceSource.Pause();
                soundIsPaused = true;
            }

        }
        else
        {
            if (soundIsPaused)
            {
                voiceSource.UnPause();
                soundIsPaused = false;
            }

        }
        if (!voiceSource.isPlaying && !soundIsPaused)
        {
            Destroy(gameObject);
        }
    }
   
    public void DeathSound(AudioClip deathClip, float volume )
    {
        voiceSource.clip = deathClip;
        voiceSource.volume = volume;
        voiceSource.Play();
    }
}

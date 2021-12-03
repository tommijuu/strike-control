using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAudioManager : MonoBehaviour
{
    public AudioSource soundSource;

    public AudioClip[] audioClips;

    public void ButtonClickSound()
    {
        soundSource.PlayOneShot(audioClips[0], 1f);
    }
    public void ButtonErrorSound()
    {
        soundSource.PlayOneShot(audioClips[1], 1f);
    }
}

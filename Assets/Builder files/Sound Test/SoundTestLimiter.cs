using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTestLimiter : MonoBehaviour
{
    public bool uselimiter = true;
    public float limiterReset = 0.2f;
    public bool limiter = true;

    private List<string> currentlyPlaying = new List<string>();

    public bool SoundBuffer()
    {
        if (uselimiter)
        {
            if (limiter)
            {
                limiter = false;
                StartCoroutine(Soundlimiter());
                return true;
            }
            else
            {
                return false;
            }
        }
        return true;

    }

    public bool SoundBuffer2(string name)
    {
        if (uselimiter)
        {
            if (currentlyPlaying.Contains(name))
            {
                return false;
            }
            currentlyPlaying.Add(name);
            StartCoroutine(Soundlimiter2());
            return true;
        }
        return true;          
    }

    IEnumerator Soundlimiter()
    {
        yield return new WaitForSeconds(limiterReset);
        limiter = true;
    }

    IEnumerator Soundlimiter2()
    {
        yield return new WaitForSeconds(limiterReset);
        currentlyPlaying.Clear();
    }

}

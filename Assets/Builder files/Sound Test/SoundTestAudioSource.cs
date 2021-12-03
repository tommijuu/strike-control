using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTestAudioSource : MonoBehaviour
{
    public AudioSource weaponSource;
    public AudioSource voiceSource;

    public AudioClip[] weaponClips;
    public AudioClip[] deathClips;

    public GameObject SoundTestLimiter;
    public GameObject audioListener;

    [HideInInspector]
    public float distance;

    // Start is called before the first frame update
    void Start()
    {
        SoundTestLimiter = GameObject.Find("Limiter");
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(transform.position, audioListener.transform.position);
    }

    public void PlaySoundCombat(string unitname, float volume)
    {
        if (weaponClips.Length > 0)
        {
            if (distance < weaponSource.maxDistance)
            {
                if (SoundTestLimiter.GetComponent<SoundTestLimiter>().SoundBuffer2(unitname))
                {
                    int index = Random.Range(0, weaponClips.Length);
                    weaponSource.PlayOneShot(weaponClips[index], volume);
                    Debug.Log(transform.parent.name + " pew! " + index);
                }
                else
                {
                    Debug.Log(transform.parent.name + " no pew");
                }
            }
            else
            {
                Debug.Log(transform.parent.name + " no pew due to distance");
            }
            
        }
        else
        {
            Debug.Log(transform.parent.name+" "+weaponClips+" is empty");
        }
        
        
    }

    public void PlaySoundDeath(string unitname, float volume)
    {
        if (deathClips.Length > 0)
        {
            if (distance < weaponSource.maxDistance)
            {
                unitname += "death";
                if (SoundTestLimiter.GetComponent<SoundTestLimiter>().SoundBuffer2(unitname))
                //if (SoundTestLimiter.GetComponent<SoundTestLimiter>().SoundBuffer())
                {
                    int index = Random.Range(0, deathClips.Length);
                    weaponSource.PlayOneShot(deathClips[index], volume);
                    Debug.Log(transform.parent.name + " is dead " + index);
                }
                else
                {
                    Debug.Log(transform.parent.name + " silent death");
                }
            }
            else
            {
                Debug.Log(transform.parent.name + " silent death due to distance");
            }
            
        }
        else
        {
            Debug.Log(transform.parent.name+" "+deathClips+ " is empty");
        }


    }
}

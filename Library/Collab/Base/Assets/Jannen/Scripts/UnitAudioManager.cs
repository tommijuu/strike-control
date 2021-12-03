using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAudioManager : MonoBehaviour
{
    public AudioSource weaponSource;
    //public AudioSource voiceSource;

    public AudioClip[] weaponClips;
    public AudioClip[] infantryDeathClips;

    public GameObject deathSound;
    public GameManager manageGame;

    private bool soundIsPaused = false;

    public float distance;

    // Start is called before the first frame update
    void Start()
    {
        manageGame = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(transform.position, manageGame.audioListener.transform.position);

        if (manageGame.paused || manageGame.IsGameOver)
        {
            if (!soundIsPaused)
            {
                weaponSource.Pause();
                soundIsPaused = true;
            }

        }
        else
        {
            if (soundIsPaused)
            {
                weaponSource.UnPause();
                soundIsPaused = false;
            }

        }
    }

    public void PlaySoundCombat(string unitname, float volume)
    {
        if (weaponClips.Length > 0)
        {
            if (distance < weaponSource.maxDistance)
            {
                if (manageGame.SoundBuffer(unitname))
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
            Debug.Log(transform.parent.name + " " + weaponClips + " is empty");
        }


    }

    /*
    public void PlaySoundDeath(string unitname, float volume)
    {
        if (infantryDeathClips.Length > 0)
        {
            if (distance < voiceSource.maxDistance)
            {
                unitname += "death";
                if (manageGame.SoundBuffer(unitname))
                {
                    int index = Random.Range(0, infantryDeathClips.Length);
                    voiceSource.PlayOneShot(infantryDeathClips[index], volume);
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
            Debug.Log(transform.parent.name + " " + infantryDeathClips + " is empty");
        }


    }
    */

    public void UnitDeath(string unitname, string unitType, float volume)
    {
        if (unitType.Equals("infantry"))
        {
            if (infantryDeathClips.Length > 0)
            {
                if (distance < weaponSource.maxDistance)
                {
                    //unitname += "death";
                    if (manageGame.SoundBuffer(unitType))
                    {
                        int index = Random.Range(0, infantryDeathClips.Length);
                        //AudioSource.PlayClipAtPoint(infantryDeathClips[index], transform.position, volume);
                        GameObject unitDeathSound = Instantiate(deathSound, transform.position, transform.rotation);
                        //Debug.Log(unitDeathSound.name);
                        // unitDeathSound.GetComponent<DeathSoundManager>().testdebug();
                        unitDeathSound.GetComponent<DeathSoundManager>().DeathSound(infantryDeathClips[index],volume);
                        
                        //weaponSource.PlayOneShot(infantryDeathClips[index], volume);
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
                Debug.Log(transform.parent.name + " " + infantryDeathClips + " is empty");
            }
        }
        
    }
}
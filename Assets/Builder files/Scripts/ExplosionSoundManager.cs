using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionSoundManager : MonoBehaviour
{
    public AudioSource explosionSource;
    public AudioClip[] explosionClips;

    public GameManager manageGame;
    public GameObject audioListener;

    private bool soundIsPaused = false;

    [HideInInspector]
    public float distance;
    private void Awake()
    {
        manageGame = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(transform.position, manageGame.audioListener.transform.position);

        if (manageGame.paused || manageGame.IsGameOver)
        {
            if (!soundIsPaused)
            {
                explosionSource.Pause();
                soundIsPaused = true;
            }

        }
        else
        {
            if (soundIsPaused)
            {
                explosionSource.UnPause();
                soundIsPaused = false;
            }

        }
    }

    public void PlaySoundExplosion(float volume)
    {
        if (explosionClips.Length > 0)
        {
            if (distance < explosionSource.maxDistance)
            {
                string unitname = "explosion";
                if (manageGame.SoundBuffer(unitname))
                {
                    int index = Random.Range(0, explosionClips.Length);
                    explosionSource.PlayOneShot(explosionClips[index], volume);
                    Debug.Log(transform.parent.name + " boom! " + index);
                }
                else
                {
                    Debug.Log(transform.parent.name + " no boom");
                }
            }
            else
            {
                Debug.Log(transform.parent.name + " no boom due to distance");
            }

        }
        else
        {
            Debug.Log(transform.parent.name + " " + explosionClips + " is empty");
        }


    }
}

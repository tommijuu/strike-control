using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTestUnitController : MonoBehaviour
{
    public float startplaying;
    public float playrate;
    
    public string unitname;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("playSound", startplaying, playrate);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void playSound()
    {
        GetComponentInChildren<SoundTestAudioSource>().PlaySoundCombat(unitname, 1f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    private int factionChoice = 0; //default value
    private int waveCountChoice = 5; //default value

    public int difficulty;

    public int FactionChoice { get { return factionChoice; } set { factionChoice = value; } }
    public int WaveCountChoice { get { return waveCountChoice; } set { waveCountChoice = value; } }

    private void Awake()
    {

        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
    }


    public void SetWaveCount(int difficultyChoice)
    {
        difficulty = difficultyChoice;
        if (difficultyChoice < 3 && difficultyChoice > -1)
        {
            waveCountChoice = (difficultyChoice + 1) * 5;
        }
        else if (difficultyChoice == 3)
        {
            waveCountChoice = int.MaxValue; //"infinite"
        }
        else
        {
            waveCountChoice = 5;
        }

    }
}

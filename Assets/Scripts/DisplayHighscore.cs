using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayHighscore : MonoBehaviour
{
    private DatabaseAccess databaseAccess;

    private Text highscoreOutput;

    void Start()
    {
        databaseAccess = GameObject.FindGameObjectWithTag("DatabaseAccess").GetComponent<DatabaseAccess>();
        highscoreOutput = gameObject.transform.GetChild(1).GetComponent<Text>();
        InvokeRepeating("DisplayHighScores", 1f, 10f);
    }

    private async void DisplayHighScores()
    {
        var task = databaseAccess.GetScores();
        var result = await task;
        var output = "";

        List<HighScore> scores = new List<HighScore>();

        foreach (var score in result)
        {
            scores.Add(score);
        }

        List<HighScore> SortedList = scores.OrderByDescending(s => s.Score).ToList();

        foreach (var score in SortedList)
        {

            output += score.UserName + " : " + " Faction: " + score.Faction + ", " +
              " Difficulty: " + score.Difficulty + ", " + " Wave: " + score.Wave + ", " +
             " Score: " + score.Score + "\n\n";

        }

        highscoreOutput.text = output;
    }
}



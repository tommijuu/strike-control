using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayScore : MonoBehaviour
{
    //EtteKatoMunSalasanaa687
    //mongodb+srv://Cyrbex:<EtteKatoMunSalasanaa687>@cluster0.peyid.mongodb.net/<dbname>?retryWrites=true&w=majority

    private Text scoreText;

    private GameManager gm;

    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        scoreText = GameObject.Find("Score").GetComponent<Text>();
    }

    void Update()
    {
        scoreText.text = "Score: " + gm.score.ToString();
    }
}

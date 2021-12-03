using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CustomGame : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public GameObject descriptionText;
    [HideInInspector]
    public MainMenu mm;
    [HideInInspector]
    public enum Faction
    {
        Empire = 0,
        Union = 1,
        Cult = 2
    }
    [HideInInspector]
    public enum Difficulty
    {
        Easy = 0,
        Normal = 1,
        Hard = 2,
        Impossible = 3
    }

    //These bools are just for visual things
    public bool empireSelected, unionSelected, cultSelected;
    public bool easy, normal, hard, impossible;

    public void Awake()
    {

        mm = GameObject.Find("MainMenuCanvas").GetComponent<MainMenu>();
    }

    public void SetFaction()
    {
        //TODO: Leave as selected even when something else is clicked
        if (gameObject.name.Equals("EmpireButton")){
            mm.playerFaction = (int)Faction.Empire;
            empireSelected = true;
            unionSelected = false;
            cultSelected = false;
        }
        if (gameObject.name.Equals("UnionButton")){
            mm.playerFaction = (int)Faction.Union;
            empireSelected = false;
            unionSelected = true;
            cultSelected = false;
        }
        if (gameObject.name.Equals("CultButton")){
            mm.playerFaction = (int)Faction.Cult;
            empireSelected = false;
            unionSelected = false;
            cultSelected = true;
        }
    }

    public void SetDifficulty()
    {
        //TODO: Leave as selected even when something else is clicked
        if (gameObject.name.Equals("Easy"))
        {
            mm.playerDifficulty = (int)Difficulty.Easy;
            easy = true;
            normal = false;
            hard = false;
            impossible = false;
        }
        if (gameObject.name.Equals("Normal"))
        {
            mm.playerDifficulty = (int)Difficulty.Normal;
            easy = false;
            normal = true;
            hard = false;
            impossible = false;

        }
        if (gameObject.name.Equals("Hard"))
        {
            mm.playerDifficulty = (int)Difficulty.Hard;
            easy = false;
            normal = false;
            hard = true;
            impossible = false;
        }
        if (gameObject.name.Equals("Impossible"))
        {
            mm.playerDifficulty = (int)Difficulty.Impossible;
            easy = false;
            normal = false;
            hard = false;
            impossible = true;
        }
    }

    //showing description for difficulties
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (gameObject.CompareTag("DifficultyButton") || gameObject.CompareTag("FactionButton"))
        {
            descriptionText.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (gameObject.CompareTag("DifficultyButton") || gameObject.CompareTag("FactionButton"))
        {
            descriptionText.SetActive(false);
        }
    }
}

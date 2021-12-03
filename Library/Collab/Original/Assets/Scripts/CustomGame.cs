using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CustomGame : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public GameObject descriptionText;
    [HideInInspector]
    public GameObject defaultFactionButton;
    [HideInInspector]
    public GameObject defaultDifficultyButton;
    
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
    [HideInInspector]
    public bool empireSelected, unionSelected, cultSelected;
    [HideInInspector]
    public bool easy, normal, hard, impossible;

    public void Awake()
    {
        mm = GameObject.Find("MainMenuCanvas").GetComponent<MainMenu>();

        defaultFactionButton = GameObject.Find("EmpireButton");
        defaultDifficultyButton = GameObject.Find("Easy");

        if (defaultFactionButton = GameObject.Find("EmpireButton"))
        {
            defaultFactionButton.transform.Find("Text").gameObject.SetActive(true);
            defaultFactionButton.GetComponent<Button>().interactable = false;
            mm.playerFaction = (int)Faction.Empire;
            empireSelected = true;
            mm.lastFactionDescription = defaultFactionButton.transform.Find("Text").gameObject;
        }

        if (defaultDifficultyButton = GameObject.Find("Easy"))
        {
            defaultDifficultyButton.transform.Find("EasyDescription").gameObject.SetActive(true);
            defaultDifficultyButton.GetComponent<Button>().interactable = false;
            mm.playerDifficulty = (int)Difficulty.Easy;
            easy = true;
            mm.lastDifficultyDescription = defaultDifficultyButton.transform.Find("EasyDescription").gameObject;
        }
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

    //showing description of the button
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (gameObject.CompareTag("FactionButton"))
        {
            if (mm.lastFactionDescription != descriptionText && mm.lastFactionDescription != null)
            {
                mm.lastFactionDescription.SetActive(false);
                descriptionText.SetActive(true);
            }
            else
            {
                return;
            }
        }

        if (gameObject.CompareTag("DifficultyButton"))
        {
            if(mm.lastDifficultyDescription != descriptionText && mm.lastDifficultyDescription != null)
            {
                mm.lastDifficultyDescription.SetActive(false);
                descriptionText.SetActive(true);
            }
            else
            {
                return;
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (gameObject.CompareTag("FactionButton"))
        {
            mm.lastFactionDescription = descriptionText;
        }

        if (gameObject.CompareTag("DifficultyButton"))
        {
            mm.lastDifficultyDescription = descriptionText;
        }
    }
}

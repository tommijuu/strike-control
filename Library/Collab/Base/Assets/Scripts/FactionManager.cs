using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FactionManager : MonoBehaviour
{
    public enum Faction
    {
        Empire = 0,
        Union = 1,
        Cult = 2
    }

    public static FactionManager instance;

    private GameObject[] machineGunners;

    [SerializeField] private GameObject empireMachineGunner;
    [SerializeField] private GameObject unionMachineGunner;
    [SerializeField] private GameObject cultMachineGunner;

    [SerializeField] private ScriptableObject machineGunnerButtonObject;
    [SerializeField] private ScriptableObject bazookaButtonObject;
    [SerializeField] private ScriptableObject jeepButtonObject;
    [SerializeField] private ScriptableObject gathererButtonObject;
    [SerializeField] private ScriptableObject lightTankButtonObject;
    [SerializeField] private ScriptableObject heavyTankButtonObject;

    private int playerFactionChoice;
    private int opponentFactionChoice;

    public int PlayerFactionChoice { get {return playerFactionChoice; } set { playerFactionChoice = value; } }
    public int OpponentFactionChoice { get { return OpponentFactionChoice; } }

    private int[] factions;

    private void Awake()
    {
            instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        factions = new int[]
        {
        0,1,2
        };

        machineGunners = new GameObject[]
        {
            empireMachineGunner,
            unionMachineGunner,
            cultMachineGunner
        };

        PlayerFactionChoice = (int)Faction.Empire; //default to Empire on testing purposes
        SetOpponentFactionChoice();
        //SetOpponentPreFabs();
    }

    public void SetOpponentFactionChoice()
    {
        int[] validFactionChoices = factions.Except(new int[] { playerFactionChoice }).ToArray(); //exclude the faction choice of the player

        opponentFactionChoice = validFactionChoices[Random.Range(0, validFactionChoices.Length)];

        Debug.Log("Player Faction: " + playerFactionChoice);
        Debug.Log("Opponent faction: " + opponentFactionChoice);
    }

    public void SetPlayerPreFabs() //Change all prefabs to suit the player faction
    {
        //TO DO Set all Player Prefabs, Ask Tommi for more info how to change
        machineGunnerButtonObject.
    }

    public void SetOpponentPreFabs() //Change all prefabs to suit the opponent's faction
    {
        EnemySpawner.instance.MachineGunner = machineGunners[opponentFactionChoice];
    }
}

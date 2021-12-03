using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class FactionManager : MonoBehaviour
{
    public enum Faction
    {
        Empire = 0,
        Union = 1,
        Cult = 2
    }

    public static FactionManager instance;

    [SerializeField] private GameObject factionLogo;

    [SerializeField] private Texture empireLogo;
    [SerializeField] private Texture unionLogo;
    [SerializeField] private Texture cultLogo;

    [SerializeField] private GameObject enemyEmpireMachineGunner;
    [SerializeField] private GameObject enemyUnionMachineGunner;
    [SerializeField] private GameObject enemyCultMachineGunner;

    [SerializeField] private GameObject playerEmpireMachineGunner;
    [SerializeField] private GameObject playerUnionMachineGunner;
    [SerializeField] private GameObject playerCultMachineGunner;

    [SerializeField] private GameObject enemyEmpireBazooka;
    [SerializeField] private GameObject enemyUnionBazooka;
    [SerializeField] private GameObject enemyCultBazooka;

    [SerializeField] private GameObject playerEmpireBazooka;
    [SerializeField] private GameObject playerUnionBazooka;
    [SerializeField] private GameObject playerCultBazooka;

    [SerializeField] private GameObject enemyEmpireJeep;
    [SerializeField] private GameObject enemyUnionJeep;
    [SerializeField] private GameObject enemyCultJeep;

    [SerializeField] private GameObject playerEmpireJeep;
    [SerializeField] private GameObject playerUnionJeep;
    [SerializeField] private GameObject playerCultJeep;

    [SerializeField] private GameObject enemyEmpireLightTank;
    [SerializeField] private GameObject enemyUnionLightTank;
    [SerializeField] private GameObject enemyCultLightTank;

    [SerializeField] private GameObject playerEmpireLightTank;
    [SerializeField] private GameObject playerUnionLightTank;
    [SerializeField] private GameObject playerCultLightTank;

    [SerializeField] private GameObject enemyEmpireHeavyTank;
    [SerializeField] private GameObject enemyUnionHeavyTank;
    [SerializeField] private GameObject enemyCultHeavyTank;

    [SerializeField] private GameObject playerEmpireHeavyTank;
    [SerializeField] private GameObject playerUnionHeavyTank;
    [SerializeField] private GameObject playerCultHeavyTank;

    [SerializeField] private GameObject playerEmpireGatherer;
    [SerializeField] private GameObject playerUnionGatherer;
    [SerializeField] private GameObject playerCultGatherer;

    [SerializeField] private Unit machineGunnerButtonObject;
    [SerializeField] private Unit bazookaButtonObject;
    [SerializeField] private Unit jeepButtonObject;
    [SerializeField] private Unit gathererButtonObject;
    [SerializeField] private Unit lightTankButtonObject;
    [SerializeField] private Unit heavyTankButtonObject;

    private GameObject[] playerMachineGunners;
    private GameObject[] enemyMachineGunners;
    private GameObject[] playerBazookas;
    private GameObject[] enemyBazookas;
    private GameObject[] playerJeeps;
    private GameObject[] enemyJeeps;
    private GameObject[] playerLightTanks;
    private GameObject[] enemyLightTanks;
    private GameObject[] playerHeavyTanks;
    private GameObject[] enemyHeavyTanks;
    private GameObject[] playerGatherers;

    private Texture[] factionLogos;
    private int[] machineGunnerCost;
    private int[] bazookaCost;
    private int[] jeepCost;
    private int[] lightTankCost;
    private int[] heavyTankCost;

    private int playerFactionChoice;
    private int enemyFactionChoice;
    private int[] factions;

    public int PlayerFactionChoice { get {return playerFactionChoice; } set { playerFactionChoice = value; } }
    public int EnemyFactionChoice { get { return enemyFactionChoice; } }
    public int EnemyMachineGunnerCost { get { return machineGunnerCost[enemyFactionChoice]; } }
    public int EnemyBazookaCost { get { return bazookaCost[enemyFactionChoice]; } }
    public int EnemyJeepCost { get { return jeepCost[enemyFactionChoice]; } }
    public int EnemyLightTankCost { get { return lightTankCost[enemyFactionChoice]; } }
    public int EnemyHeavyTankCost { get { return heavyTankCost[enemyFactionChoice]; } }

    public GameObject PlayerGatherer { get { return playerGatherers[playerFactionChoice]; } }

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

        factionLogos = new Texture[]
        {
            empireLogo, unionLogo, cultLogo
        };

        enemyMachineGunners = new GameObject[]
        {
            enemyEmpireMachineGunner,
            enemyUnionMachineGunner,
            enemyCultMachineGunner
        };

        playerMachineGunners = new GameObject[]
        {
            playerEmpireMachineGunner,
            playerUnionMachineGunner,
            playerCultMachineGunner
        };

        playerBazookas = new GameObject[]
       {
            playerEmpireBazooka,
            playerUnionBazooka,
            playerCultBazooka
       };

        enemyBazookas = new GameObject[]
      {
            enemyEmpireBazooka,
            enemyUnionBazooka,
            enemyCultBazooka
      };

        playerJeeps = new GameObject[]
{
            playerEmpireJeep,
            playerUnionJeep,
            playerCultJeep
};

        enemyJeeps = new GameObject[]
{
            enemyEmpireJeep,
            enemyUnionJeep,
            enemyCultJeep
};

        playerLightTanks = new GameObject[]
{
            playerEmpireLightTank,
            playerUnionLightTank,
            playerCultLightTank
};

        enemyLightTanks = new GameObject[]
{
            enemyEmpireLightTank,
            enemyUnionLightTank,
            enemyCultLightTank
};

        playerHeavyTanks = new GameObject[]
{
            playerEmpireHeavyTank,
            playerUnionHeavyTank,
            playerCultHeavyTank
};

        enemyHeavyTanks = new GameObject[]
{
            enemyEmpireHeavyTank,
            enemyUnionHeavyTank,
            enemyCultHeavyTank
};

        playerGatherers = new GameObject[]
{
            playerEmpireGatherer,
            playerUnionGatherer,
            playerCultGatherer
};

        machineGunnerCost = new int[]
        {
            100,
            50,
            75
        };

        bazookaCost = new int[]
        {
            200,
            100,
            150
        };

        jeepCost = new int[]
        {
            400,
            200,
            300
        };

        lightTankCost = new int[]
        {
            500,
            300,
            400
        };

        heavyTankCost = new int[]
        {
            1000,
            700,
            850
        };

        if (FindObjectOfType<LevelManager>())
        {
            playerFactionChoice = LevelManager.instance.FactionChoice;
        } else
        {
            PlayerFactionChoice = 0; //default to Empire if main menu is skipped
        }

        SetFactionLogo();
        SetEnemyFactionChoice();
        SetPlayerPreFabs();
        SpawnStartingUnitPrefabs();
    }


    public void SetEnemyFactionChoice()
    {
        int[] validFactionChoices = factions.Except(new int[] { playerFactionChoice }).ToArray(); //exclude the faction choice of the player

        enemyFactionChoice = validFactionChoices[Random.Range(0, validFactionChoices.Length)];

        Debug.Log("Player Faction: " + playerFactionChoice);
        Debug.Log("Opponent faction: " + enemyFactionChoice);
    }

    public void SetPlayerPreFabs() //Change all prefabs to suit the player faction
    {
        machineGunnerButtonObject.cost = machineGunnerCost[playerFactionChoice];
        machineGunnerButtonObject.model = playerMachineGunners[playerFactionChoice];
        bazookaButtonObject.cost = bazookaCost[PlayerFactionChoice];
        bazookaButtonObject.model = playerBazookas[playerFactionChoice];

        jeepButtonObject.cost = jeepCost[PlayerFactionChoice];
        jeepButtonObject.model = playerJeeps[playerFactionChoice];

        gathererButtonObject.model = playerGatherers[playerFactionChoice];

        lightTankButtonObject.cost = lightTankCost[PlayerFactionChoice];
        lightTankButtonObject.model = playerLightTanks[playerFactionChoice];

        heavyTankButtonObject.cost = heavyTankCost[PlayerFactionChoice];
        heavyTankButtonObject.model = playerHeavyTanks[playerFactionChoice];
    }

    public void SpawnStartingUnitPrefabs()
    {
        int unitCount;

        if(playerFactionChoice == (int)Faction.Empire)
        {
            unitCount = 3;
        }
        else if (playerFactionChoice == (int)Faction.Cult)
        {
            unitCount = 3;
        }
        else
        {
            unitCount = 6;
        }

        for (int i = 0; i < unitCount; i++)
        {
            Instantiate(playerMachineGunners[playerFactionChoice], new Vector3((-9 - i * 2), 1, 0), Quaternion.identity);
            Instantiate(playerMachineGunners[playerFactionChoice], new Vector3((-9 - i * 2), 1, -2), Quaternion.identity);
            if(playerFactionChoice == (int)Faction.Cult && i < 2)
            {
                Instantiate(playerMachineGunners[playerFactionChoice], new Vector3((-9 - i * 2), 1, -4), Quaternion.identity);
            }
        }
    }

    public void SetOpponentPreFabs() //Change all prefabs to suit the opponent's faction
    {
        EnemySpawner.instance.MachineGunner = enemyMachineGunners[enemyFactionChoice];
        EnemySpawner.instance.Bazooka = enemyBazookas[enemyFactionChoice];
         EnemySpawner.instance.Jeep = enemyJeeps[enemyFactionChoice];
         EnemySpawner.instance.LightTank = enemyLightTanks[enemyFactionChoice];
         EnemySpawner.instance.HeavyTank = enemyHeavyTanks[enemyFactionChoice];
    }

    public void SetFactionLogo()
    {
        factionLogo.GetComponent<RawImage>().texture = factionLogos[playerFactionChoice];
    }
}

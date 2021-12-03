using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner instance;

    [SerializeField] private Transform enemyUnits;
    [SerializeField] private GameObject machineGunner;
    [SerializeField] private GameObject bazooka;
    [SerializeField] private GameObject jeep;
    [SerializeField] private GameObject lightTank;
    [SerializeField] private GameObject heavyTank;

    [SerializeField] private Text waveTimerText;

    private List<GameObject> tier1EnemyList = new List<GameObject>();
    private List<GameObject> tier2EnemyList = new List<GameObject>();
    private List<GameObject> tier3EnemyList = new List<GameObject>();

    private float spawnTime = 2f;
    private bool lowTimerCheck = false;

    private float waveTimer = 60f;
    private float waveDelay = 60f;
    private float lowTimer;
    private int spawnCount;

    private List<GameObject> unitList = new List<GameObject>();

    public GameObject MachineGunner { get { return machineGunner; } set { machineGunner = value; } }
    public GameObject Bazooka { get { return bazooka; } set { bazooka = value; } }
    public GameObject Jeep { get { return jeep; } set { jeep = value; } }
    public GameObject LightTank { get { return lightTank; } set { lightTank = value; } }
    public GameObject HeavyTank { get { return heavyTank; } set { heavyTank = value; } }

    void Start()
    {
        instance = this;

        Invoke("LateStart", 3);

        lowTimer = waveTimer / 4;
        StartCoroutine(WaveIncoming());
    }

    // Update is called once per frame
    void Update()
    {
        WaveTimer();
    }

    private void LateStart()
    {
        Debug.Log("Late Start");

        FactionManager.instance.SetOpponentPreFabs();

        tier1EnemyList.Add(machineGunner);
        tier1EnemyList.Add(bazooka);
        tier2EnemyList.Add(jeep);
        tier2EnemyList.Add(lightTank);
    }


    private void SpawnGroup()
    {
        if (WaveManager.instance.GroupCount < WaveManager.instance.GroupsPerWave)
        {
            BuyUnits();
            int spawnPoint = WaveManager.instance.GroupCount;
            WaveManager.instance.GroupCount++;
            List<Vector3> positionList = PositionListGenerator.GetPositionListAround(transform.GetChild(spawnPoint).position, new float[] { 2f, 4f, 6f }, new int[] { 5, 10, 20 });

            int groupSize = unitList.Count;

            GameObject group = new GameObject();
            group.name = "Group";
            group.transform.SetParent(enemyUnits);

            for (int i = 0; i < groupSize; i++)
            {
                GameObject enemy = Instantiate(unitList[i], positionList[i], Quaternion.identity);
                enemy.transform.SetParent(group.transform);
            }

            enemyUnits.GetComponent<EnemyManager>().AddGroup(group.transform);
            unitList.Clear();
        }
    }

    private IEnumerator WaveIncoming()
    {

        yield return new WaitForSeconds(waveDelay);
        if (WaveManager.instance.WaveCount == 0) //first wave
        {
            Debug.Log("Repeat!");
            InvokeRepeating("SpawnGroup", 1, spawnTime);
            WaveManager.instance.WaveCount++;
            WaveManager.instance.UpdateWaveCountText();
            StartCoroutine(Audiomanager.instance.PlaySoundRepeat(2, 1f, 3));
        }
        else
        {
            WaveManager.instance.NextWave();
        }
        waveTimer = waveDelay;
        StartCoroutine(WaveIncoming());
    }

    private void WaveTimer()
    {
        waveTimer -= Time.deltaTime;

        if ((WaveManager.instance.WaveCount) < WaveManager.instance.MaxWaves)
        {
            if (waveTimer < lowTimer && !lowTimerCheck)
            {
                Audiomanager.instance.PlaySound(3, 1f);
                waveTimerText.color = Color.red;
                lowTimerCheck = true;
            }
            else if (waveTimer > lowTimer && lowTimerCheck)
            {
                waveTimerText.color = Color.white;
                lowTimerCheck = false;
            }
            waveTimerText.text = "Next wave in: " + Mathf.Round(waveTimer);
        }
        else
        {
            waveTimerText.color = Color.red;
            waveTimerText.text = "Survive " + Mathf.Round(waveTimer) + " seconds";

            //TO DO No timer but you have to kill all foes?
        }
    }

    public void BuyUnits()
    {
        int moneyForGroup = WaveManager.instance.MoneyPerGroup;

        while (moneyForGroup > 0)
        {
            GameObject tier1unit = tier1EnemyList[Random.Range(0, tier1EnemyList.Count)];
            GameObject tier2unit = tier2EnemyList[Random.Range(0, tier2EnemyList.Count)];

            if(moneyForGroup >= FactionManager.instance.EnemyHeavyTankCost)
            {
                moneyForGroup -= FactionManager.instance.EnemyHeavyTankCost;
                Debug.Log("DEBUG: Money after heavy tank buy" + moneyForGroup);
                unitList.Add(heavyTank);
            }

            if(moneyForGroup >= FactionManager.instance.EnemyLightTankCost)
            {
                if (tier2unit.gameObject.name == jeep.gameObject.name)
                {
                    moneyForGroup -= FactionManager.instance.EnemyJeepCost;
                    Debug.Log("DEBUG: Money after jeep buy" + moneyForGroup);
                }
                else
                {
                    moneyForGroup -= FactionManager.instance.EnemyLightTankCost;
                    Debug.Log("DEBUG: Money after light tank buy" + moneyForGroup);
                }

                unitList.Add(tier2unit);
            }

            if (tier1unit.gameObject.name == machineGunner.gameObject.name)
            {
                moneyForGroup -= FactionManager.instance.EnemyMachineGunnerCost;
            }
            else
            {
                moneyForGroup -= FactionManager.instance.EnemyBazookaCost;
            }

            if (moneyForGroup >= 0)
            {
                unitList.Add(tier1unit);
            }
        }
    }
}

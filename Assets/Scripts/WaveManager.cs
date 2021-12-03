using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;

    private int waveCount = 0;
    private int maxWaves;
    private int groupCount = 0;
    private int groupsPerWave = 3;
    private int moneyPerGroup = 150;
    [SerializeField] private Text waveCountText;

    public int WaveCount { get { return waveCount; } set { waveCount = value; } }
    public int MaxWaves { get { return maxWaves; } set { maxWaves = value; } }
    public int GroupCount { get { return groupCount; } set { groupCount = value; } }
    public int GroupsPerWave { get { return groupsPerWave; } set { groupsPerWave = value; } }
    public int MoneyPerGroup { get { return moneyPerGroup; } set { moneyPerGroup = value; } }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        if (FindObjectOfType<LevelManager>())
        {
            maxWaves = LevelManager.instance.WaveCountChoice;
        }
        else
        {
            maxWaves = 15;
        }

        UpdateWaveCountText();
    }

    public void NextWave()
    {
        waveCount++;
        if (waveCount > maxWaves)
        {
            GameManager.instance.GameWin(true);
        }
        else
        {
            StartCoroutine(Audiomanager.instance.PlaySoundRepeat(2, 1f, 3));

            if((groupsPerWave < EnemySpawner.instance.gameObject.transform.childCount) && (waveCount > 5) && (waveCount % 2 == 0))
            {
                groupsPerWave++;
            }

            groupCount = 0;

            if((waveCount % 5) == 0 && waveCount > 0)
            {
                moneyPerGroup += 50;
            }

            if(waveCount > 5 && waveCount < 10)
            {
                moneyPerGroup += 25;
            } else
            {
                moneyPerGroup += 50;
            }

            UpdateWaveCountText();
        }
    }

    public void UpdateWaveCountText()
    {
        if (maxWaves == int.MaxValue)
        {
            waveCountText.text = "Wave " + waveCount + "/" + "∞";
        } else
        {
            waveCountText.text = "Wave " + waveCount + "/" + maxWaves;
        }
    }
}

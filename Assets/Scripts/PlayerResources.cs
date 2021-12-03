using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerResources : MonoBehaviour
{
    public static PlayerResources instance;

    private int money = 7500;
    private int power = 0;
    private int powerConsumption = 0;
    private int maxPower = 100;
    private bool lowPower = false;
    public Text moneyField;

    public Image powerBar;
    public Image powerConsumptionBar;

    private Color powerConsumptionBarColor;
    private Color powerBarColor;
    [SerializeField] private GameObject factionLogo;
    public GameObject minimap;
    public Text LowPowerText;


    public int Power { get { return power; } set { power = value; } }
    public int PowerConsumption { get { return powerConsumption; } set { powerConsumption = value; } }
    public int MaxPower { get { return maxPower; } set { maxPower = value; } }
    public int Money { get { return money; } set { money = value; } }
    public bool LowPower { get { return lowPower; } set { lowPower = value; } }


    void Start()
    {
        instance = this;


        powerConsumptionBarColor = powerConsumptionBar.color;
        powerBarColor = powerBar.color;

        UpdatePowerBar();
    }

    private void Update()
    {
        moneyField.text = money.ToString();

        UpdatePowerBar();
    }

    private void UpdatePowerBar()
    {
        if (powerConsumption > power)
        {
            powerConsumptionBar.color = Color.red;
        }
        else
        {
            powerConsumptionBar.color = powerConsumptionBarColor;
        }

        if (power >= PowerConsumption)
        {
            maxPower = power;
        } else
        {
            maxPower = powerConsumption;
        }

        powerBar.fillAmount = (float)power / (float)maxPower;
        powerConsumptionBar.fillAmount = (float)powerConsumption / (float)maxPower;

        PowerCheck();
    }

    private void PowerDown()
    {
        Audiomanager.instance.PlaySound(0, 1f);
        lowPower = true;
        minimap.SetActive(false);
        factionLogo.SetActive(true);
        LowPowerText.gameObject.SetActive(true);
    }

    private void PowerRestored()
    {
        Audiomanager.instance.PlaySound(1, 1f);
        lowPower = false;
        minimap.SetActive(true);
        factionLogo.SetActive(false);
        LowPowerText.gameObject.SetActive(false);
    }

    public void ChangePowerOnBuild(int powerCost)
    {
        if (powerCost < 0) //powerplant has minus powercost
        {
            power -= powerCost;
        }
        else
        {
            powerConsumption += powerCost;
        }
    }

    public void ChangePowerOnDestroy(int powerCost)
    {
        if (powerCost < 0) //powerplant has minus powercost
        {
            power += powerCost;
        }
        else
        {
            powerConsumption -= powerCost;
        }
    }

            private void PowerCheck()
    {
        if (lowPower && (power >= powerConsumption))
        {
            PowerRestored();
        }
        else if (!lowPower && power < powerConsumption)
        {
                PowerDown();
        }
    }

}

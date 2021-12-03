using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class SpecialPower : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public UnityEvent onLeft;
    public UnityEvent onRight;
    public UnityEvent onMiddle;

    public Image progress;

    public GameManager manageGame;

    public float counter;
    public float specialPowerCounter;

    private int factionID = 0;

    private bool powerInUse = false;
    public bool powerDown;
    public bool inProgress;
    public bool done;

    public int RechargeDuration;
    public int empireSpecialPowerDuration;
    public int unionSpecialPowerDuration;

    public Sprite empireSprite;
    public Sprite unionSprite;
    public Sprite cultSprite;
    public Image buttonImage;

    public GameObject specialPowerCult;

    void Start()
    {
        manageGame = GameObject.Find("GameManager").GetComponent<GameManager>();
        factionID = FactionManager.instance.PlayerFactionChoice;
        Debug.Log("Faction ID: " + factionID);

        if (factionID == 0)
        {
            buttonImage.sprite = empireSprite;
        }
        else if (factionID == 1)
        {
            buttonImage.sprite = unionSprite;
        }
        else if (factionID == 2)
        {
            buttonImage.sprite = cultSprite;
        }
        else
        {
            Debug.Log("false faction ID: " + factionID);
        }
    }


    void Update()
    {
        if (PlayerResources.instance.Power >= PlayerResources.instance.PowerConsumption)
        {
            powerDown = false;
            if (!inProgress)
            {
                gameObject.GetComponent<Button>().interactable = true;
            }
            
        }
        else
        {
            powerDown = true;
            gameObject.GetComponent<Button>().interactable = false;
        }

        if(!inProgress && !done)
        {
            inProgress = true;
        }

        if (inProgress)
        {
            if (!powerDown)
            {
                if (counter < RechargeDuration)
                {
                    counter += Time.deltaTime;
                }
                else
                {
                    counter = 0;
                }

                progress.fillAmount = Mathf.Lerp(0, 1, counter / RechargeDuration);

                if (counter >= RechargeDuration)
                {
                    inProgress = false;
                    done = true;
                    Debug.Log("special power ready");
                    gameObject.GetComponent<Button>().interactable = true;
                }
                else
                {
                    gameObject.GetComponent<Button>().interactable = false;
                }
            }

        }

        if (factionID == 0) 
        {
            if (GameManager.instance.unionSpecialPowerActive)
            {
                GameManager.instance.unionSpecialPowerActive = false;
            }
            if (powerInUse)
            {
                
                if (!powerDown)
                {
                    if (specialPowerCounter < empireSpecialPowerDuration)
                    {
                        GameManager.instance.empireSpecialPowerActive = true;
                        specialPowerCounter += Time.deltaTime;
                    }
                    else
                    {
                        GameManager.instance.empireSpecialPowerActive = false;
                        specialPowerCounter = 0;
                        powerInUse = false;
                        Audiomanager.instance.PlaySpecialSound(7, 1f);
                        Debug.Log("power duration end");
                    }
                }
                else
                {
                    GameManager.instance.empireSpecialPowerActive = false;
                }
            }
            
        }
        if (factionID == 1)
        {
            if (GameManager.instance.empireSpecialPowerActive)
            {
                GameManager.instance.empireSpecialPowerActive = false;
            }
            if (powerInUse)
            {

                if (!powerDown)
                {
                    //Debug.Log("power in use");
                    if (specialPowerCounter < unionSpecialPowerDuration)
                    {
                        GameManager.instance.unionSpecialPowerActive = true;
                        specialPowerCounter += Time.deltaTime;
                    }
                    else
                    {
                        GameManager.instance.unionSpecialPowerActive = false;
                        specialPowerCounter = 0;
                        powerInUse = false;
                        Audiomanager.instance.PlaySpecialSound(7, 1f);
                        Debug.Log("power duration end");
                    }
                }
                else
                {
                    GameManager.instance.unionSpecialPowerActive = false;
                }
            }

        }


    }

    public void UsePower()
    {
        if (done)
        {         
            if (factionID == 0)
            {
                powerInUse = true;
                Audiomanager.instance.PlaySpecialSound(8, 1f);
                Debug.Log("EMPIRE SUPER POWER!");
                done = false;
            }
            else if (factionID == 1)
            {
                powerInUse = true;
                Audiomanager.instance.PlaySpecialSound(8, 1f);
                Debug.Log("UNION SUPER POWER!");
                done = false;
            }
            else if (factionID == 2)
            {
                CultSpecialPower();
                Debug.Log("CULT SUPER POWER!");
                done = false;
            }
            else
            {
                Debug.Log("false faction ID: " + factionID);
            }

        }
    }

    public void CultSpecialPower()
    {
        Instantiate(specialPowerCult, specialPowerCult.transform.position, specialPowerCult.transform.rotation);
    }

    public IEnumerator EmpireSpecialPower()
    {
        GameManager.instance.empireSpecialPowerActive = true;
        yield return new WaitForSeconds(empireSpecialPowerDuration);
        GameManager.instance.empireSpecialPowerActive = false;
    }

    public IEnumerator UnionSpecialPower()
    {
        GameManager.instance.unionSpecialPowerActive = true;
        yield return new WaitForSeconds(unionSpecialPowerDuration);
        GameManager.instance.unionSpecialPowerActive = false;
    }

    

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (gameObject.GetComponent<Button>().interactable)
            {
                Audiomanager.instance.PlaySound(4, 1f);
                onLeft.Invoke();
            }
            else
            {
                Audiomanager.instance.PlaySound(5, 1f);
            }
            
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            onRight.Invoke();
        }
        if (eventData.button == PointerEventData.InputButton.Middle)
        {
            onMiddle.Invoke();
        }
    }


    //To give CostInfo script the cost of the building
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (factionID == 0)
        {
            CostInfo.ShowTooltip_Static(GameObject.Find("SpecialPowerEmpire").GetComponent<Text>().text);
        }
        else if (factionID == 1)
        {
            CostInfo.ShowTooltip_Static(GameObject.Find("SpecialPowerUnion").GetComponent<Text>().text);
        }
        else if (factionID == 2)
        {
            CostInfo.ShowTooltip_Static(GameObject.Find("SpecialPowerCult").GetComponent<Text>().text);
        }
        else
        {
            Debug.Log("false faction ID: " + factionID);
        }
       
        
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        CostInfo.HideTooltip_Static();
    }

    private void OnDestroy()
    {
        GameManager.instance.empireSpecialPowerActive = false;
        GameManager.instance.unionSpecialPowerActive = false;
    }

}
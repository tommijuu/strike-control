using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class Repair : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{

    public Texture2D cursorArrow;

    //public bool inRepair;

    private GameObject building;

    private Button button;

    public UnityEvent onLeft;
    public UnityEvent onRight;
    public UnityEvent onMiddle;

    Ray ray;
    RaycastHit rayHit;

    private float healAmount;

    private int hqCost = 1000; //just some cost for repairing HQ because it doesn't have building cost

    private string name;

    private GameSettings gameSettings;
    
    void Start()
    {
        gameSettings = GameObject.Find("GameSettings").GetComponent<GameSettings>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (GameManager.instance.inRepair)
        {
            if (GameManager.instance.paused || GameManager.instance.IsGameOver)
            {
                CancelRepair();
            }
            if (Input.GetMouseButtonDown(0))
            {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out rayHit))
                {
                    building = rayHit.collider.gameObject;

                    //if clicked not null, not ground, not player unit or it is a turret
                    if ((building != null && !building.CompareTag("Ground") && !building.CompareTag("PlayerUnit") && !building.CompareTag("EnemyUnit") && !building.CompareTag("ResourceNode")) || (building.name.Equals("TurretModel(Clone)") || building.name.Equals("HeavyTurretModel(Clone)")))
                    {
                        //if it's turret, take max health from playerunitcontroller script
                        if (building.name.Equals("TurretModel(Clone)") || building.name.Equals("HeavyTurretModel(Clone)"))
                        {
                            healAmount = building.GetComponent<PlayerUnitController>().stats.maxHealth;
                        }else if (building.CompareTag("Headquarters"))
                        {
                            healAmount = building.GetComponent<HeadQuarters>().buildingStats.maxHealth;
                        }
                        else
                        {
                            healAmount = building.GetComponent<BuildingManager>().buildingStats.maxHealth; //gets clicked building's max health
                        }

                        if (healAmount == building.GetComponent<HealthBar>().health) //if health is already full, cancel repair
                        {
                            CancelRepair();
                        }
                        else
                        {
                            if (!building.CompareTag("Headquarters")) //headquarters can't be built yet and doesn't have buildingmanager
                            {
                                name = building.GetComponent<BuildingManager>().name;

                                foreach (Button menuIcon in building.GetComponent<BuildingManager>().manageGame.menuIcons)
                                {
                                    if (menuIcon.name.Equals(name)) //if menuicon is the same name as the building that's clicked
                                    {
                                        button = menuIcon;
                                    }
                                }
                                
                                if (PlayerResources.instance.Money >= button.GetComponent<BuildBuilding>().cost / 2)
                                {
                                    building.GetComponent<HealthBar>().health = healAmount; //gets current health and puts max health
                                    PlayerResources.instance.Money -= button.GetComponent<BuildBuilding>().cost / 2; //repairing for others than HQ cost half the of building cost atm
                                    CancelRepair();
                                }
                                else
                                {
                                    CancelRepair();
                                }
                            }

                            if (building.CompareTag("Headquarters") && PlayerResources.instance.Money >= hqCost)
                            {
                                building.GetComponent<HealthBar>().health = healAmount; //gets current health and puts max health
                                PlayerResources.instance.Money -= hqCost; //so hardcoded cost reee
                                CancelRepair();
                            }
                            else
                            {
                                CancelRepair();
                            }
                        }

                    }
                    else
                    {
                        Audiomanager.instance.PlaySound(5, 1f);
                    }
                }
                
            }
            
            if(Input.GetMouseButtonUp(1))
            {
                CancelRepair();
            }
        }
    }

    public void ChangeCursor()
    {
        Audiomanager.instance.PlaySound(4, 1f);
        Cursor.SetCursor(cursorArrow, Vector2.zero, CursorMode.ForceSoftware);
        GameManager.instance.inRepair = true;
    }

    public void CancelRepair()
    {
        if (!GameManager.instance.paused && !GameManager.instance.IsGameOver)
        {
            Audiomanager.instance.PlaySound(4, 1f);
        }
        Cursor.SetCursor(gameSettings.cursorArrow, Vector2.zero, CursorMode.ForceSoftware);
        GameManager.instance.inRepair = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (!GameManager.instance.inUse)
            {
                onLeft.Invoke();
            }
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (!GameManager.instance.inUse)
            {
                Audiomanager.instance.PlaySound(5, 1f);
            }
        }
        if (eventData.button == PointerEventData.InputButton.Middle)
        {
            onMiddle.Invoke();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        CostInfo.ShowTooltip_Static("Repair buildings\nCosts half of the building cost");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CostInfo.HideTooltip_Static();
    }
}

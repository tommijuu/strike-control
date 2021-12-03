using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowPowerBarTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        CostInfo.ShowTooltip_Static("Power Amount: " + PlayerResources.instance.Power.ToString() + 
            "\nPower Consumption: " + PlayerResources.instance.PowerConsumption.ToString());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CostInfo.HideTooltip_Static();
    }
}

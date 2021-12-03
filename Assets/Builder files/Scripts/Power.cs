using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class Power : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public UnityEvent onLeft;
    public UnityEvent onRight;
    public UnityEvent onMiddle;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Audiomanager.instance.PlaySound(5, 1f);
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Audiomanager.instance.PlaySound(5, 1f);
        }
        if (eventData.button == PointerEventData.InputButton.Middle)
        {

        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

    }
    public void OnPointerExit(PointerEventData eventData)
    {

    }
}

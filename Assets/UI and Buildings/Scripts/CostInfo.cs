using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CostInfo : MonoBehaviour
{
    private static CostInfo instance;
    [SerializeField]
    private Camera UICamera; //RectTransformUtility command needs this even though our CanvasUI isn't Screen Space - Camera (doesn't have its own camera)

    private Text tooltipText;
    private RectTransform background;

    private void Awake()
    {
        instance = this;
        background = transform.Find("Background").GetComponent<RectTransform>();
        tooltipText = transform.Find("Text").GetComponent<Text>();
        gameObject.SetActive(false);
    }

    void Update()
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), Input.mousePosition, UICamera, out localPoint);
        transform.localPosition = localPoint + new Vector2(-tooltipText.preferredWidth + 20f, 15f);
    }

    private void ShowToolTip(string tooltipString)
    {
        gameObject.SetActive(true);

        tooltipText.text = tooltipString;
        float textPaddingSize = 4f;
        Vector2 backgroundSize = new Vector2(tooltipText.preferredWidth + textPaddingSize * 2f, tooltipText.preferredHeight + textPaddingSize * 2f);
        background.sizeDelta = backgroundSize;

    }

    private void HideToolTip()
    {
        gameObject.SetActive(false);
    }

    public static void ShowTooltip_Static(string tooltipString) //BuildBuilding and BuildUnit call this to show name and cost (sell and repair too)
    {
        instance.ShowToolTip(tooltipString);
    }

    public static void HideTooltip_Static()
    {
        instance.HideToolTip();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceBar : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    private GameObject resourceBar;
    private RawImage resourceBarFill;

    private GameObject canvas;
    private Camera cam;

    private int maxResources;

    private float resourceRatio;

    void Start()
    {
        cam = Camera.main;
        canvas = GameObject.FindGameObjectWithTag("HealthCanvas");
        resourceBar = Instantiate(prefab, canvas.transform);
        resourceBarFill = resourceBar.transform.GetChild(2).GetComponent<RawImage>();
        

        maxResources = GetComponentInParent<ResourceGatherer>().MaxResourceStorage;
    }

    void LateUpdate()
    {
        resourceBar.transform.position = GetComponentInParent<HealthBar>().HealthBarObject.transform.position + new Vector3(0, -6f, 0);

        resourceRatio = ((float)GetComponentInParent<ResourceGatherer>().OilStorage + (float)GetComponentInParent<ResourceGatherer>().SuperOilStorage) / (float)maxResources;
        resourceBarFill.transform.localScale = new Vector3(resourceRatio, 1, 1);
    }

    void Die()
    {
        if (GetComponentInParent<HealthBar>().health <= 0)
        {
            GameObject.FindGameObjectWithTag("PlayerManager").GetComponent<PlayerManager>().selectedUnits.Remove(transform);
            Destroy(resourceBar);
        }
    }

    public void ResourceBarVisible()
    {
        resourceBar.gameObject.SetActive(true);
    }

    public void ResourceBarInvisible()
    {
        resourceBar.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        Destroy(resourceBar);
    }
}


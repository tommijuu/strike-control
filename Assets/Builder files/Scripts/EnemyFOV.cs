using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFOV : MonoBehaviour
{
    public GameObject phantom;
    GameObject model;
    public bool visible = false;

    void Start()
    {
        model = transform.GetChild(0).gameObject;
        model.SetActive(false);
        GetComponentInChildren<SpriteRenderer>().enabled = false;
    }


    void Update()
    {
        if (visible && !model.activeSelf)
        {
            model.SetActive(true);
            GetComponentInChildren<SpriteRenderer>().enabled = true;
        }
        if (!visible && model.activeSelf)
        {
            model.SetActive(false);
            GetComponentInChildren<SpriteRenderer>().enabled = false;
            GetComponentInChildren<HealthBar>().HealthBarInvisible();
        }
        if (visible)
        {
            visible = false;
        }
        

    }
    public void EnemyVisible()
    {
        visible = true;
    }


    public void MakePhantom()
    {
        Instantiate(phantom, transform.position, Quaternion.identity);
    }
    
}

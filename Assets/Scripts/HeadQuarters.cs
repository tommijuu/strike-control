using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadQuarters : MonoBehaviour
{
    public BuildingStats buildingStats;
    private GameObject effectUnion;
    public GameObject unionEffect;

    void Start()
    {
        ParticleSystem ef = unionEffect.GetComponent<ParticleSystem>();
        var empireShape = ef.shape;
        empireShape.scale = new Vector3(transform.localScale.z, transform.localScale.x, 0f);
    }
    void Update()
    {
        if (GameManager.instance.unionSpecialPowerActive)
        {
            if (!effectUnion)
            {
                effectUnion = Instantiate(unionEffect, new Vector3(transform.position.x, 2f, transform.position.z), unionEffect.transform.rotation);
            }
        }
        else
        {
            if (effectUnion)
            {
                Destroy(effectUnion);
            }
        }
    }
    private void OnDestroy()
    {
        GameManager.instance.GameWin(false);
    }
}

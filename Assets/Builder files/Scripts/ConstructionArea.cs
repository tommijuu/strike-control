using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionArea : MonoBehaviour
{
    [SerializeField] BuildingStats stats;
    [HideInInspector]
    public float radius;
    [Range(0.0f, 0.3f)]
    public float outline;
    // Start is called before the first frame update
    void Start()
    {
        radius = stats.constructionRange;
        outline /= radius;
        GetComponent<Renderer>().material.SetFloat("_Outline", outline);
        transform.localScale = new Vector3(radius * 2 * (1 / transform.parent.localScale.x), transform.localScale.y, radius * 2 * (1 / transform.parent.localScale.z));    
    }
}

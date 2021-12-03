using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEntityFOV : MonoBehaviour
{
    public float radius;
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(radius / 5 * (1 / transform.parent.localScale.x), transform.localScale.y, radius / 5 * (1 / transform.parent.localScale.z));
    }

}

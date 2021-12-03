using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyGameObject : MonoBehaviour
{

    public float time = 1f;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Dest", time);
    }

    void Dest()
    {
        Destroy(gameObject);
    }
}

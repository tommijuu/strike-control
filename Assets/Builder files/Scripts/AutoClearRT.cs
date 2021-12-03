using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Camera))]
public class AutoClearRT : MonoBehaviour
{
    public bool NoClearAfterStart = false;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Camera>().clearFlags = CameraClearFlags.Color;
    }

    // Update is called once per frame
    void Update()
    {
        if (!NoClearAfterStart)
        {
            GetComponent<Camera>().clearFlags = CameraClearFlags.Depth;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMode : MonoBehaviour
{
    public static BuildMode instance;

    private bool isBuildMode;

    public bool IsBuildMode { get { return isBuildMode; } set { isBuildMode = value; } }

    // Start is called before the first frame update
    void Start()
    {
        isBuildMode = false;
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleBuildMode()
    {
        if (!isBuildMode)
        {
            isBuildMode = true;
        } else
        {
            isBuildMode = false;
        }

        Debug.Log("Build mode: " + isBuildMode);
    }
}

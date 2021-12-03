using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWheels : MonoBehaviour
{
    public Transform[] wheels;
    public float rotationSpeed = 10f;
    public bool rotate = false;
    public bool axis = false;

    // Update is called once per frame
    void Update()
    {
        if(rotate)
        {
            foreach (Transform wheel in wheels)
            {
                if(!axis)
                    wheel.Rotate(rotationSpeed * Time.deltaTime, 0, 0);
                else
                    wheel.Rotate(0, -rotationSpeed * Time.deltaTime, 0);
            }
        }
    }
}

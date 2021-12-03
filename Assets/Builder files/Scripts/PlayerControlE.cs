using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlE : MonoBehaviour
{

    public GameObject Building;
    public GameObject Building2;
    public GameObject Builder;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameObject.Find("Builder2(Clone)"))
        {
            if (Input.GetKeyDown("1"))
            {
                
                Builder.GetComponent<Builder2>().building = Building;
                Instantiate(Builder, new Vector3(0, -5, 0), Quaternion.identity);
                

            }
            if (Input.GetKeyDown("2"))
            {
                Builder.GetComponent<Builder2>().building = Building2;
                Instantiate(Builder, new Vector3(0, -5, 0), Quaternion.identity);
                

            }
        }

    }
}


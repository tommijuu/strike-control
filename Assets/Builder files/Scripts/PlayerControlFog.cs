using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlFog : MonoBehaviour
{
    public float speed;
    public float radius;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        x *= speed * Time.deltaTime;
        z *= speed * Time.deltaTime;

        transform.Translate(x, 0, z);
    }

    private void DistanceCheck()
    {
        
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius*2);
        int i = 0;

        while (i < hitColliders.Length)
        {
            if (hitColliders[i].tag == "enemy")
            {
                Debug.Log(hitColliders[i].transform.position);
                
                if (Physics.Raycast(transform.position, (hitColliders[i].transform.position - transform.position), radius))
                {
                    
                    Debug.DrawRay(transform.position, (hitColliders[i].transform.position - transform.position), Color.green);
                    hitColliders[i].GetComponent<Renderer>().enabled = true;
                    Debug.Log("Did Hit");
                }
                else
                {
                    Debug.DrawRay(transform.position, (hitColliders[i].transform.position - transform.position), Color.red);
                    hitColliders[i].GetComponent<Renderer>().enabled = false;
                    Debug.Log("Did not Hit");
                }
                

            }
            i++;
        }
        
    }
}

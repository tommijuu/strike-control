using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVisible : MonoBehaviour
{
    public float radius;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DistanceCheck();
    }

    private void DistanceCheck()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius * 2);
        int i = 0;

        while (i < hitColliders.Length)
        {
            if (hitColliders[i].tag == "Player")
            {
                Debug.Log(hitColliders[i].transform.position);

                if (radius >= Vector3.Distance(hitColliders[i].transform.position, transform.position))
                {
                    GetComponent<Renderer>().enabled = true;
                    Debug.DrawRay(transform.position, (hitColliders[i].transform.position - transform.position), Color.green);
                    Debug.Log("Did Hit");
                    break;
                }
                else
                {
                    GetComponent<Renderer>().enabled = false;
                    Debug.DrawRay(transform.position, (hitColliders[i].transform.position - transform.position), Color.red);
                    Debug.Log("Did not Hit");
                }


            }
            i++;
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVScript : MonoBehaviour
{
    public float radius;
    public GameObject FieldOfView;
    // Start is called before the first frame update
    void Start()
    {
        GameObject FOV = Instantiate(FieldOfView, transform.position, Quaternion.identity);
        FieldOfView.transform.localScale = new Vector3((radius / 5), 0, (radius / 5));
        FOV.transform.parent = gameObject.transform;
    }


    // Update is called once per frame
    void Update()
    {
        VisibilityCheck();
    }
    private void VisibilityCheck()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        int i = 0;

        while (i < hitColliders.Length)
        {
            if (hitColliders[i].tag == "enemy")
            {
                //Debug.Log("Did Hit");
                hitColliders[i].GetComponent<EnemyFOV>().EnemyVisible();
            }
            if (hitColliders[i].tag == "phantom")
            {
                //Debug.Log("Did Hit");
                hitColliders[i].GetComponent<Phantom>().EnemyVisible();
            }
            i++;
        }

    }
}

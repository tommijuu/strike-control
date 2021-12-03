using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFOV : MonoBehaviour
{
    [SerializeField] EntityStats stats;
    public float radius;
    // Start is called before the first frame update
    void Start()
    {       
        radius = stats.sightRange;
        transform.localScale = new Vector3(radius / 5 * (1 / transform.parent.localScale.x), transform.localScale.y, radius / 5 * (1 / transform.parent.localScale.z));
    }
       

    // Update is called once per frame
    void Update()
    {
        FOVCheck();
    }

    private void FOVCheck()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        int i = 0;

        while (i < hitColliders.Length)
        {
            if (hitColliders[i].tag == "EnemyUnit")
            {
                
                hitColliders[i].GetComponent<EnemyFOV>().EnemyVisible();
            }
            if (hitColliders[i].tag == "phantom")
            {
                
                hitColliders[i].GetComponent<Phantom>().EnemyVisible();
            }
            i++;
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject target;

    public Transform partToRotate;

    public float turnSpeed = 10f;

    public bool turretRotating;

    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f); //so that it doesn't update the target all the time
    }

    void UpdateTarget()
    {
        if(!PlayerResources.instance.LowPower)
            target = gameObject.GetComponent<PlayerUnitController>().targetEnemy;
    }

    void Update()
    {
        if (!PlayerResources.instance.LowPower)
        {
            if (target == null)
            {
                turretRotating = false;
                return;
            }

            turretRotating = true;
            //LOCKING ON TARGET
            Vector3 dir = target.transform.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
            partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        }
        
        
    }

    private void OnDestroy()
    {
        PlayerResources.instance.ChangePowerOnDestroy(GetComponent<BuildingManager>().buildingStats.powerCost);
    }
}

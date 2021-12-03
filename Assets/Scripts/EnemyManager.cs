using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public Transform enemies;

    //List<Transform> groups;

    GameObject mainTarget;

    // Start is called before the first frame update
    void Start()
    {
        //groups = new List<Transform>();
        FindMainTarget();

        //Lisätään groupit listaan
        foreach (Transform group in enemies)
        {
            AddGroup(group);
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*
        foreach (Transform group in groups)
        {
            GameObject target = GroupHasTargetWhileMovingTowardsHQ(group);

            if(target != null)
            {
                Attack(group, target);
            }

            if (mainTarget != null)
            {
                if (GroupCanContinue(group))
                {
                    Attack(group, mainTarget, true);
                }
            }
        }
        */
    }

    // Tarkistaa että onko milläkään vihollisella kohdetta, jos ei ole, voidaan jatkaa
    bool GroupCanContinue(Transform group)
    {
        int groupCanContinue = 0;
        foreach (Transform enemy in group)
        {
            EnemyUnitController enemyUnitController = enemy.GetComponent<EnemyUnitController>();
            if (!enemyUnitController.goingTowardsHQ && enemyUnitController.IsAgentStopped() && !enemyUnitController.EnemyInAggroRange())
            {
                groupCanContinue++;
            }
        }

        if (groupCanContinue == group.childCount)
            return true;

        return false;
    }


    GameObject GroupHasTargetWhileMovingTowardsHQ(Transform group)
    {
        foreach (Transform enemy in group)
        {
            EnemyUnitController enemyUnitController = enemy.GetComponent<EnemyUnitController>();
            GameObject target = enemyUnitController.EnemyInAggroRange();

            if (enemyUnitController.goingTowardsHQ && target != null)
            {
                return target;
            }
        }
        return null;
    }

    void FindMainTarget()
    {
        mainTarget = GameObject.FindGameObjectWithTag("Headquarters");
    }

    public void Attack(Transform group, GameObject target, bool attackHQ = false)
    {
        Debug.Log("Attack!");

        Vector3 average = new Vector3();
        for (int i = 0; i < group.childCount; i++)
        {
            average.x += group.GetChild(i).position.x;
            average.z += group.GetChild(i).position.z;
        }
        average.x /= group.childCount;
        average.z /= group.childCount;

        float dt = 7f;
        float d = Mathf.Sqrt(Mathf.Pow(average.x - target.transform.position.x, 2) + Mathf.Pow(average.z - target.transform.position.z, 2));
        float t = dt / d;
        Vector3 targetPoint = new Vector3((1 - t) * target.transform.position.x + t * average.x, 0, (1 - t) * target.transform.position.z + t * average.z);

        List<Vector3> positionList = PositionListGenerator.GetPositionListAround(targetPoint, new float[] { 2f, 4f, 6f, 8.5f }, new int[] { 5, 10, 20, 30 });

        for (int i = 0; i < group.childCount; i++)
        {
            group.GetChild(i).GetComponent<EnemyUnitController>().SetDestination(positionList[i]);
            if (!attackHQ)
                group.GetChild(i).GetComponent<EnemyUnitController>().goingTowardsHQ = false;
            else
                group.GetChild(i).GetComponent<EnemyUnitController>().goingTowardsHQ = true;

        }
    }

    public void AddGroup(Transform group)
    {
        //groups.Add(group);
    }
}

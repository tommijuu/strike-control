using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnitController : UnitController
{
    public bool goingTowardsHQ;
    private GameObject hq;

    private void Start()
    {
        hq = GameObject.FindGameObjectWithTag("Headquarters");
    }

    void Update()
    {
        stateText.GetComponent<TextMesh>().text = currentState.ToString();

        if (currentState != State.Moving)
        {
            myAgent.isStopped = true;
            myAgent.updateRotation = false;
        }
        else
        {
            myAgent.isStopped = false;
            myAgent.updateRotation = true;
        }

        //Look for units if not in combat
        if (currentState != State.Combat && targetEnemy == null)
        {
            //Look for units and start combat if player units in attack range
            CheckAttackRange();
        }

        //If in combat and target is not in attack range
        if (currentState == State.Combat && !IsTargetInAttackRange())
        {
            //Find a new target
            ReTarget();
        }

        //If unit is idle, move towards the HQ
        if (currentState == State.Idle)
        {
            if (!(hq == null))
            {
                SetDestination(hq.transform.position);
            }


            if (targetEnemy != null && IsTargetInAttackRange() && currentState != State.Combat)
            {
                StartCoroutine(StartAttacking());
            }
        }
        Animations();
        RotateWheels();
    }

    protected override void CheckAttackRange()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, stats.attackRange);
        foreach (Collider collider in colliders)
        {
            foreach (string tag in enemyTags)
            {
                if (collider.CompareTag(tag))
                {
                    targetEnemy = collider.gameObject;
                    if (currentState != State.Combat)
                    {
                        if (!GetComponent<Turret>())
                        {
                            StartCoroutine(RotateTowardsTarget());
                        }
                        StartCoroutine(StartAttacking());
                    }
                    break;
                }
            }
        }
    }


    public override void SetDestination(Vector3 destination)
    {
        MyAgent.SetDestination(destination);
        ChangeState(State.Moving);
        if (isTank || isJeep)
        {
            StartCoroutine(ResetTurretRotation());
        }
    }



}

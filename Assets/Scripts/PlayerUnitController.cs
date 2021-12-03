using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnitController : UnitController
{
    public LineRenderer lineRenderer;

    private void Start()
    {
        if (GetComponent<ResourceGatherer>())
        {
            lineRenderer.gameObject.SetActive(false);
        }

        if(!GetComponent<ResourceGatherer>() && !GetComponent<Turret>())
        {
            StartCoroutine(SpawnStackFix());
        }

    }

    // Update is called once per frame
    void Update()
    {
        stateText.GetComponent<TextMesh>().text = currentState.ToString();


        if (!GetComponent<Turret>() && !GetComponent<ResourceGatherer>())
        {
            if (currentState != State.Moving && currentState != State.AttackCommand)
            {
                myAgent.updateRotation = false;
                myAgent.updatePosition = false;
            }
            else
            {
                myAgent.updateRotation = true;
                myAgent.updatePosition = true;
            }
        }

        if (GetComponent<Turret>())
        {
            if (!PlayerResources.instance.LowPower)
            {
                //Look for units if not in combat
                if (currentState != State.Combat && currentState != State.AttackCommand && targetEnemy == null)
                {
                    //Look for units if player units in attack range
                    
                    CheckAttackRange();
                }

                //If in combat and target is not in attack range
                if (currentState == State.Combat && !IsTargetInAttackRange())
                {
                    //Find a new target
                    ReTarget();
                }
            }
            else
            {
                StopCoroutine(RotateTowardsTarget());
                StopCoroutine(StartAttacking());
                targetEnemy = null;
                ChangeState(State.Idle);
            }
        }
        else if (GetComponent<ResourceGatherer>())
        {
            if (currentState == State.Moving)
            {
                if (!lineRenderer.gameObject.activeSelf)
                {
                    lineRenderer.gameObject.SetActive(true);
                }

                UpdateLine();
            }
        }
        else
        {
            UpdateLine();

            if (IsAgentStopped() && currentState == State.Moving && currentState != State.AttackCommand)
            {
                currentState = State.Idle;
            }


            if (currentState == State.Moving)
            {
                StopCoroutine(RotateTowardsTarget());
                StopCoroutine(StartAttacking());
                attacking = false;
            }

            if (currentState == State.AttackCommand && IsAgentStopped() && IsTargetInAttackRange())
            {
                currentState = State.Combat;
                targetEnemy = attackCommandTarget;
            }

            //Look for units if idle
            if (currentState == State.Idle)
            {
                //Look for units and start combat if player units in attack range
                CheckAttackRange();
            }

            //If in combat and target is not in attack range
            if (currentState == State.Combat && !IsTargetInAttackRange())
            {
                //Find a new target
                ReTarget();

                if (targetEnemy != null)
                {
                    StartCoroutine(RotateTowardsTarget());
                }
            }

            if (currentState == State.Combat && IsTargetInAttackRange())
            {
                StartCoroutine(RotateTowardsTarget());
                StartAttackFunction();
            }
        }
        Animations();
        RotateWheels();
    }

    void UpdateLine()
    {
        if (lineRenderer != null)
        {
            if (isTank || isJeep)
                lineRenderer.SetPosition(0, new Vector3(transform.position.x, transform.position.y + .2f, transform.position.z));
            else
                lineRenderer.SetPosition(0, new Vector3(transform.position.x, transform.position.y - .8f, transform.position.z));
            if (IsAgentStopped())
            {
                lineRenderer.enabled = false;
            }
        }
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
                    if(targetEnemy == null)
                    {
                        targetEnemy = collider.gameObject;
                    }
                    if (currentState != State.Combat)
                    {
                        if (!attacking)
                        {
                            if (!GetComponent<Turret>())
                            {
                                StartCoroutine(RotateTowardsTarget());
                            }
                            StartCoroutine(StartAttacking());
                        }
                    }
                    break;
                }
            }
        }
    }

    //turret's automatic targeting broke somehow because of this (probably some destination stuff because it doesn't actually move)
    //so added a check to exclude turret
    public override void SetDestination(Vector3 destination)
    {
        if (!GetComponent<Turret>())
        {
            MyAgent.SetDestination(destination);
            ChangeState(State.Moving);
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(1, new Vector3(destination.x, destination.y + .3f, destination.z));
            if (isTank || isJeep)
            {
                StartCoroutine(ResetTurretRotation());
            }
        }

    }

    private IEnumerator SpawnStackFix()
    {
        yield return new WaitForSeconds(0.25f);
        ChangeState(State.Moving);
        myAgent.SetDestination(transform.position + new Vector3(0.5f, 0.5f, 0.5f));
        yield return new WaitForSeconds(0.25f);
        ChangeState(State.Idle);

    }
}

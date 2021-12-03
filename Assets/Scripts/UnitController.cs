using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class UnitController : MonoBehaviour
{
    protected NavMeshAgent myAgent;
    public UnitStats stats;
    HealthBar healthBar;

    public enum State { Idle, Moving, Combat, AttackCommand }
    public State currentState;

    public NavMeshAgent MyAgent { get { return myAgent; } }

    [SerializeField]
    private float unitSize;
    public float UnitSize { get { return unitSize; } }

    protected bool goingToAttackTarget;
    public GameObject targetEnemy;
    public string[] enemyTags;

    protected bool attacking;

    public GameObject muzzleFlash;
    public Transform[] shootPoint;
    int heavyTankShootpointTest = 0;

    bool rotatingTowardsTarget;

    public GameObject stateText;

    public Animator animator;

    public GameObject projectilePrefab;
    public float rotationSpeed = 100;

    public bool isTank;
    public bool isJeep;

    public GameObject attackCommandTarget;

    public LayerMask layerMask;

    void Awake()
    {
        myAgent = GetComponent<NavMeshAgent>();
        healthBar = GetComponent<HealthBar>();
        ChangeState(State.Idle);

        stateText.gameObject.SetActive(false);
    }

    protected void RotateWheels()
    {
        RotateWheels wheels = GetComponent<RotateWheels>();
        if (wheels)
        {
            wheels.rotationSpeed = myAgent.velocity.magnitude * 100;
            if (currentState == State.Moving)
                wheels.rotate = true;
            else wheels.rotate = false;
        }
    }

    protected void Animations()
    {
        if (animator != null)
        {
            if (currentState == State.Moving || currentState == State.AttackCommand)
            {
                animator.SetBool("isRunning", true);
                animator.SetBool("isCombat", false);
                animator.SetFloat("runSpeed", myAgent.velocity.magnitude);
            }
            else if (currentState == State.Idle)
            {
                animator.SetBool("isRunning", false);
                animator.SetBool("isCombat", false);
            }
            else if (currentState == State.Combat)
            {
                animator.SetBool("isRunning", false);
                animator.SetBool("isCombat", true);
            }
        }
    }


    protected bool IsTargetInAttackRange()
    {
        if (Vector3.Distance(targetEnemy.transform.position, transform.position) <= stats.attackRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    protected void ReTarget()
    {
        bool targetLost = false;
        if (targetEnemy != null && !IsTargetInAttackRange())
        {
            targetLost = true;
        }

        if (targetLost)
        {
            targetEnemy = EnemyInAttackRange();
        }
    }



    protected void FollowIfInAggroRange()
    {

    }

    protected virtual void CheckAttackRange() { }

    protected void GoToTargetAndAttack()
    {
        if (IsAgentStopped())
        {
            goingToAttackTarget = false;
            StartCoroutine(StartAttacking());
        }
    }

    public bool IsAgentStopped()
    {
        if (!myAgent.pathPending)
        {
            if (myAgent.remainingDistance <= myAgent.stoppingDistance)
            {
                if (!myAgent.hasPath || myAgent.velocity.sqrMagnitude == 0f)
                {
                    if (currentState != State.Combat)
                    {
                        currentState = State.Idle;
                    }
                    return true;
                }
            }
        }

        return false;
    }

    public void Shoot()
    {
        DoDamage();

        GetComponentInChildren<UnitAudioManager>().PlaySoundCombat(stats.name, 1f);
        Vector3 rot = shootPoint[0].parent.rotation.eulerAngles + new Vector3(90, 0, 0);
        Instantiate(muzzleFlash, shootPoint[0].position, Quaternion.Euler(rot));
    }


    public void DoDamage()
    {
        EntityStats targetStats = targetEnemy.GetComponent<HealthBar>().stats;
        HealthBar targetHealth = targetEnemy.GetComponent<HealthBar>();

        float damageFormula = 0;
        if (stats.damageType == DamageType.projectile)
        {
            damageFormula = stats.damage - stats.damage * targetStats.projectileResistance;
        }
        else if (stats.damageType == DamageType.melee)
        {
            damageFormula = stats.damage - stats.damage * targetStats.meeleeResistance;
        }
        else if (stats.damageType == DamageType.explosion)
        {
            damageFormula = stats.damage - stats.damage * targetStats.explosionResistance;
        }

        targetHealth.health -= damageFormula;
        Debug.Log(gameObject.name + " shoots " + targetEnemy.name + " and does " + damageFormula + " damage! " + " Enemy has " + targetHealth.health + " hp left");
    }

    public void ShootProjectile()
    {
        Vector3 rot;
        GameObject projectile;
        if (!isTank)
        {
            rot = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            projectile = Instantiate(projectilePrefab, shootPoint[0].position, Quaternion.Euler(rot));
        }
        else
        {
            Transform head = transform.GetChild(0).GetChild(1);
            rot = new Vector3(head.rotation.eulerAngles.x + 90, head.rotation.eulerAngles.y, head.rotation.eulerAngles.z);
            if (shootPoint.Length == 2)
            {
                rot = new Vector3(head.rotation.eulerAngles.x + 90, head.rotation.eulerAngles.y - 90, head.rotation.eulerAngles.z);
                if (heavyTankShootpointTest == 0)
                    heavyTankShootpointTest++;
                else heavyTankShootpointTest--;
            }
            projectile = Instantiate(projectilePrefab, shootPoint[heavyTankShootpointTest].position, Quaternion.Euler(rot));


        }


        projectile.GetComponent<Projectile>().unitController = this;
        if (animator != null)
            animator.SetTrigger("shoot");
        GetComponentInChildren<UnitAudioManager>().PlaySoundCombat(stats.name, 1f);
    }

    protected IEnumerator RotateTowardsTarget()
    {
        rotatingTowardsTarget = true;
        Quaternion targetRotation = Quaternion.identity;

        if (!isTank && !isJeep)
        {
            while (Quaternion.Angle(transform.rotation, targetRotation) > .01f)
            {
                Vector3 enemyPos = new Vector3(targetEnemy.transform.position.x, 0, targetEnemy.transform.position.z);
                Vector3 ownPos = new Vector3(transform.position.x, 0, transform.position.z);
                Vector3 targetDirection = (enemyPos - ownPos).normalized;
                targetRotation = Quaternion.LookRotation(targetDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
                yield return null;
            }
        }
        else
        {
            Transform head = transform.GetChild(0).GetChild(1);
            while (Quaternion.Angle(head.rotation, targetRotation) > .01f && targetEnemy != null)
            {
                Vector3 enemyPos = new Vector3(targetEnemy.transform.position.x, 0, targetEnemy.transform.position.z);
                Vector3 ownPos = new Vector3(head.position.x, 0, head.position.z);
                Vector3 targetDirection = (enemyPos - ownPos).normalized;
                if (shootPoint.Length == 1)
                    targetRotation = Quaternion.LookRotation(targetDirection) * Quaternion.Euler(new Vector3(-90, 0, 0));
                if (shootPoint.Length == 2)
                    targetRotation = Quaternion.LookRotation(targetDirection) * Quaternion.Euler(new Vector3(-90, 90, 0));
                head.rotation = Quaternion.RotateTowards(head.rotation, targetRotation, Time.deltaTime * rotationSpeed);
                yield return null;
            }
        }

        rotatingTowardsTarget = false;
        yield return null;
    }

    protected IEnumerator ResetTurretRotation()
    {
        Quaternion targetRotation = Quaternion.Euler(new Vector3(-90, 0, 0));
        Transform head = transform.GetChild(0).GetChild(1);
        while (Quaternion.Angle(head.localRotation, targetRotation) > .01f)
        {
            head.localRotation = Quaternion.RotateTowards(head.localRotation, targetRotation, Time.deltaTime * rotationSpeed);
            yield return null;
        }

        head.localRotation = targetRotation;

        yield return null;
    }


    protected void StartAttackFunction()
    {
        if (!attacking)
        {
            StartCoroutine(StartAttacking());
        }
    }


    protected IEnumerator StartAttacking()
    {
        if (targetEnemy == null)
            yield return null;
        Debug.Log(transform.name + " starts to shoot " + targetEnemy.name);
        HealthBar targetHealth = targetEnemy.GetComponent<HealthBar>();

        float elapsed = 0f;

        attacking = true;
        ChangeState(State.Combat);

        while (targetEnemy != null && targetHealth.health > 0)
        {
            if (IsTargetInRange())
            {
                elapsed += Time.deltaTime;
                if (currentState == State.Moving)
                    yield break;

                if (!LooksAtTarget() && !gameObject.name.Equals("TurretModel(Clone)") && !gameObject.name.Equals("HeavyTurretModel(Clone)") && !rotatingTowardsTarget)
                    StartCoroutine(RotateTowardsTarget());
                else if (elapsed >= stats.attackSpeed)
                {
                    if (projectilePrefab == null)
                        Shoot();
                    else
                        ShootProjectile();
                    elapsed = 0f;
                }
                yield return null;
            }
            else
            {
                yield break;
            }
        }

        attacking = false;
        ChangeState(State.Idle);

        targetEnemy = null;
        yield return null;
    }

    protected bool LooksAtTarget()
    {
        RaycastHit hit;
        if (!isTank && !isJeep)
        {
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, stats.attackRange, layerMask))
            {
                if (hit.transform != null)
                    if (hit.transform.gameObject == targetEnemy)
                        return true;
            }
        }
        else
        {
            Transform head = transform.GetChild(0).GetChild(1);
            if (shootPoint.Length == 1)
            {
                Debug.DrawRay(head.position, head.TransformDirection(Vector3.down) * 1000, Color.red);
                if (Physics.Raycast(head.position, head.TransformDirection(Vector3.down), out hit, stats.attackRange, layerMask))
                {
                    if (hit.transform != null)
                        if (hit.transform.gameObject == targetEnemy)
                            return true;
                }
            }
            if (shootPoint.Length == 2)
            {
                Debug.DrawRay(head.position, head.TransformDirection(Vector3.left) * 1000, Color.red);
                if (Physics.Raycast(head.position, head.TransformDirection(Vector3.left), out hit, stats.attackRange, layerMask))
                {
                    if (hit.transform != null)
                        if (hit.transform.gameObject == targetEnemy)
                            return true;
                }
            }

        }

        return false;
    }

    protected bool IsTargetInRange()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, stats.attackRange);
        foreach (Collider collider in colliders)
            foreach (string tag in enemyTags)
                if (collider.CompareTag(tag))
                    if (collider.gameObject == targetEnemy)
                        return true;
        return false;
    }

    protected GameObject EnemyInAttackRange()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, stats.attackRange);
        foreach (Collider collider in colliders)
            foreach (string tag in enemyTags)
                if (collider.CompareTag(tag))
                    return collider.gameObject;
        return null;
    }


    public GameObject EnemyInAggroRange()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, stats.aggroRange);
        foreach (Collider collider in colliders)
            foreach (string tag in enemyTags)
                if (collider.CompareTag(tag))
                    return collider.gameObject;
        return null;
    }


    public void SetTargetAndDestination(GameObject target, Vector3 destination)
    {
        targetEnemy = target;
        attackCommandTarget = target;

        if (!IsTargetInAttackRange())
        {
            goingToAttackTarget = true;
            SetDestination(destination);
        }

    }

    public virtual void SetDestination(Vector3 destination)
    {
        ChangeState(State.Moving);
        myAgent.SetDestination(destination);
    }

    public void ChangeState(State newState)
    {
        currentState = newState;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ResourceGatherer : MonoBehaviour
{
    private float counter = 0f;
    private int oilStorage = 0;
    private int superOilStorage = 0;
    private int superOilPriceModifier = 5;

    private bool gatheringComplete = false;
    private bool unloadingComplete = false;
    private bool isUnloading = false;
    private bool noResourceNodesIdleState = false;

    private GameObject[] resourceNodes;
    private GameObject closestResourceNode = null;
    private GameObject[] oilRefineries;
    private GameObject closestOilRefinery = null;

    private NavMeshAgent nav;

    private bool currentResourceIsEmpty = false;
    private bool isMovingTowardsResourceNode = false;

    private float resourceTimer = 1f;
    private int gatherAmountPerTick = 22;
    private const int maxResourceStorage = 220;

    private GameObject effectEmpire;
    public GameObject empireEffect;

    public bool CurrentResourceIsEmpty { set { currentResourceIsEmpty = value; } get { return currentResourceIsEmpty; } }
    public bool IsMovingTowardsResourceNode { set { isMovingTowardsResourceNode = value; } get { return isMovingTowardsResourceNode; } }
    public int OilStorage { get { return oilStorage; } }
    public int SuperOilStorage { get { return superOilStorage; } }
    public int MaxResourceStorage { get { return maxResourceStorage; } }

    private void Start()
    {
        nav = GetComponent<NavMeshAgent>();

        FindClosestResourceNode();
        FindClosestOilRefinery();

        MoveToClosestResourceNode();

        ParticleSystem uf = empireEffect.GetComponent<ParticleSystem>();
        var unionShape = uf.shape;
        unionShape.scale = new Vector3(transform.localScale.z, transform.localScale.x, 0f);

    }

    private void Update()
    {
        if (GameManager.instance.empireSpecialPowerActive)
        {
            if (!effectEmpire)
            {
                effectEmpire = Instantiate(empireEffect, new Vector3(transform.position.x, 2f, transform.position.z), empireEffect.transform.rotation);
            }
        }
        else
        {
            if (effectEmpire)
            {
                Destroy(effectEmpire);
            }
        }

        if (effectEmpire)
        {
            effectEmpire.transform.position = transform.position;
        }

        if ((currentResourceIsEmpty && !isUnloading && ((oilStorage + superOilStorage) < maxResourceStorage)) && GetComponent<UnitController>().currentState != UnitController.State.Moving)
        {
            if(resourceNodes.Length > 0)
            {
                MoveToClosestResourceNode();
                currentResourceIsEmpty = false;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "ResourceNode")
        {
            counter += Time.deltaTime;

            if (counter >= resourceTimer)
            {
                Gather(other);
            }
        }
        else if (other.tag == "OilRefineryPlatform")
        {
            counter += Time.deltaTime;

            if (counter >= resourceTimer)
            {
                Unload(other);
            }
        }
    }

    private void Gather(Collider other)
    {
        Debug.Log("Gathering!");

        isMovingTowardsResourceNode = false;
        if (other.gameObject.tag == "ResourceNode" && ((oilStorage + superOilStorage) < maxResourceStorage))
        {
            if (!other.GetComponent<ResourceNode>().IsSuperOil && ((oilStorage + gatherAmountPerTick) <= maxResourceStorage))
            {
                other.GetComponent<ResourceNode>().ReduceResourceAmount(gatherAmountPerTick);
                oilStorage += gatherAmountPerTick;
                counter = 0;
            } else if(other.GetComponent<ResourceNode>().IsSuperOil && ((superOilStorage + gatherAmountPerTick) <= maxResourceStorage))
            {
                other.GetComponent<ResourceNode>().ReduceResourceAmount(gatherAmountPerTick);
                superOilStorage += gatherAmountPerTick;
                counter = 0;
            }
        }

        if ((oilStorage + superOilStorage) == maxResourceStorage)
        {
            MoveToClosestRefinery();
        }
    }

    private void Unload(Collider other)
    {
        if (other.gameObject.tag == "OilRefineryPlatform")
        {
            if ((superOilStorage - gatherAmountPerTick) >= 0)
            {
                superOilStorage -= gatherAmountPerTick;
                PlayerResources.instance.Money += (gatherAmountPerTick * superOilPriceModifier * GameManager.instance.ESPMoneyModifier);
                counter = 0;
                isUnloading = true;
            }
            else if ((oilStorage - gatherAmountPerTick) >= 0)
            {
                oilStorage -= gatherAmountPerTick;
                PlayerResources.instance.Money += (gatherAmountPerTick * GameManager.instance.ESPMoneyModifier);
                counter = 0;
                isUnloading = true;
            }

            if ((oilStorage == 0 && superOilStorage == 0 && resourceNodes.Length > 0) && GetComponent<UnitController>().currentState != UnitController.State.Moving)
            {
                isUnloading = false;
                MoveToClosestResourceNode();
            }
        }
    }

    private void FindClosestResourceNode()
    {
        resourceNodes = GameObject.FindGameObjectsWithTag("ResourceNode");

        if (resourceNodes.Length == 0)
        {
            MoveToClosestRefinery();
        }
        else
        {
            float distanceToClosestResourceNode = 10000f; //some value has to be put

            foreach (GameObject node in resourceNodes)
            {
                float tempDistance = Vector3.Distance(transform.position, node.transform.position);
                if (closestResourceNode == null)
                {
                    distanceToClosestResourceNode = tempDistance;
                }

                if (tempDistance <= distanceToClosestResourceNode)
                {
                    closestResourceNode = node;
                    distanceToClosestResourceNode = tempDistance;
                }
            }
        }
    }

    private void FindClosestOilRefinery()
    {
        oilRefineries = GameObject.FindGameObjectsWithTag("OilRefineryPlatform");
        if (oilRefineries.Length == 0)
        {
            //Do nothing
        }
        else
        {
            float distanceToClosestOilRefinery = 10000f; //some value has to be put

            foreach (GameObject refinery in oilRefineries)
            {
                float tempDistance = Vector3.Distance(transform.position, refinery.transform.position);
                if (closestOilRefinery == null)
                {
                    distanceToClosestOilRefinery = tempDistance;
                }

                if (tempDistance <= distanceToClosestOilRefinery)
                {
                    closestOilRefinery = refinery;
                    distanceToClosestOilRefinery = tempDistance;
                }
            }
        }
    }

    private void MoveToClosestResourceNode()
    {
        FindClosestResourceNode();
        if (resourceNodes.Length > 0)
        {
            isMovingTowardsResourceNode = true;
            nav.SetDestination(closestResourceNode.transform.position);
        }
    }

    private void MoveToClosestRefinery()
    {
        isMovingTowardsResourceNode = false;
        FindClosestOilRefinery();
        if (oilRefineries.Length > 0)
        {
            nav.SetDestination(closestOilRefinery.transform.position);
        }
    }

}


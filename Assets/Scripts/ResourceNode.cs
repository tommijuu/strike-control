using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceNode : MonoBehaviour
{
    [SerializeField ] private int resourceAmount;

    private bool resourceIsEmpty = false;
    [SerializeField] private bool isSuperOil;

    private GameObject[] gatherersMovingTowardsTheNode;

    public int ResourceAmount { get { return resourceAmount; } set { resourceAmount = value; } }
    public bool IsSuperOil { get { return isSuperOil; } }

    public void ReduceResourceAmount(int amount)
    {
            resourceAmount -= amount;

        if (resourceAmount == 0)
        {
            resourceIsEmpty = true;

            gatherersMovingTowardsTheNode = GameObject.FindGameObjectsWithTag("PlayerUnit");

            //let gatherers that move towards it to move to closest existing resource node
            foreach (GameObject gatherer in gatherersMovingTowardsTheNode)
            {
               //check if empty while moving
                if (gatherer.gameObject.GetComponent<ResourceGatherer>())
                {
                    if (gatherer.gameObject.GetComponent<ResourceGatherer>().IsMovingTowardsResourceNode)
                    {
                        gatherer.gameObject.GetComponent<ResourceGatherer>().CurrentResourceIsEmpty = true;
                    }
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Collides with " + other.gameObject.name);

        if (other.tag == "PlayerUnit" && resourceIsEmpty)
        {
            other.GetComponent<ResourceGatherer>().CurrentResourceIsEmpty = true;
            Destroy(gameObject);
        }
    }

}

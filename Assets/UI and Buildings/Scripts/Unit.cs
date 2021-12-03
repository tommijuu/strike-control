using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Used for creating scriptables for unit buttons

[CreateAssetMenu(fileName = "New Unit", menuName = "Units/Unit")]
public class Unit : ScriptableObject
{
    public new string name;
 
    public GameObject model;

    public int cost;

    public int productionDuration;

    public Vector3 creationPlace;
}

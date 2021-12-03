using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Building", menuName = "Building/Building")]
public class Building : ScriptableObject
{
    public new string name;
    public GameObject model;
    public float health;
    public float powerCost;
}

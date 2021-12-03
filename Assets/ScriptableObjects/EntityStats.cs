using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStats : ScriptableObject
{
    public int size;
    public float maxHealth;
    public float sightRange;
    [Range(0f, 1f)]
    public float meeleeResistance;
    [Range(0f, 1f)]
    public float projectileResistance;
    [Range(0f, 1f)]
    public float explosionResistance;
}

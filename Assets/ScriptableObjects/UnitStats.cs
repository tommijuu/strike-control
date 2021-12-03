using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitType
{
    infantry,
    vehicle
}

public enum DamageType
{
    melee,
    projectile,
    explosion
}
[CreateAssetMenu(fileName = "New UnitStats", menuName = "Units/UnitStats")]
public class UnitStats : EntityStats
{
    public UnitType unitType;
    public float movementSpeed;
    public float attackRange;
    public float attackSpeed;
    public float aggroRange;
    public float damage;
    public DamageType damageType;
}

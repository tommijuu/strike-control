using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New BuildingStats", menuName = "Building/BuildingStats")]
public class BuildingStats : EntityStats
{
    public int powerCost;
    public float constructionRange;
}


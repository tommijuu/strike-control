using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionListGenerator
{
    public static List<Vector3> GetPositionListAround(Vector3 startPosition, float[] ringDistance, int[] ringPositionCount, bool addStart = true)
    {
        List<Vector3> positionList = new List<Vector3>();
        if(addStart)
            positionList.Add(startPosition);
        for (int ring = 0; ring < ringPositionCount.Length; ring++)
        {
            List<Vector3> ringPositionList = GetPositionListAround(startPosition, ringDistance[ring], ringPositionCount[ring]);
            positionList.AddRange(ringPositionList);
        }
        return positionList;
    }

    public static List<Vector3> GetPositionListAround(Vector3 startPosition, float distance, int positionCount)
    {
        float largestUnitInSelection = PlayerManager.largestUnitInSelection;
        List<Vector3> positionList = new List<Vector3>();
        for (int i = 0; i < positionCount; i++)
        {
            int angle = i * (360 / positionCount);
            Vector3 dir = Quaternion.Euler(0, angle, 0) * new Vector3(1, 0, 0);
            Vector3 position = startPosition + dir * distance * largestUnitInSelection;
            positionList.Add(position);
        }

        return positionList;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HexasphereGrid;

public class Utilities : MainManager
{

    public static Vector3 GenerateRandomVector3(float min, float max)
    {
        return new Vector3(Random.Range(min, max),
            Random.Range(min, max),
            Random.Range(min, max));
    }

    public static float NormalizePerlin(float rawPerlin)
    {
        return (rawPerlin + 1) / 2;
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPrefabs : MonoBehaviour
{
    [SerializeField] GameObject villagePrefab;

    public GameObject GetPrefab(string name)
    {
        switch (name)
        {
            case "Village":
                return villagePrefab;
            default: Debug.Log("prefab not found");
                return null;
                
        }
    }
}

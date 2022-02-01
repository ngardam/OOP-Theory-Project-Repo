using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Classes;

public abstract class BuildingDictionary
{
    // Start is called before the first frame update

    public static Dictionary<string, BuildingType> buildingDictionary = new Dictionary<string, BuildingType>
    {
        {
            "Village", new BuildingType
            {
                requiredToBuild = new Dictionary<string, int>
                {
                    {
                        "Wood", 10
                    },
                    {
                        "Food", 10
                    }
                }
            }
        },
        {
            "Farm", new BuildingType
            {
                requiredToBuild = new Dictionary<string, int>
                {
                    {
                        "Wood", 10
                    },
                    {
                        "Food", 15
                    }
                }
            }
        }
    };
}

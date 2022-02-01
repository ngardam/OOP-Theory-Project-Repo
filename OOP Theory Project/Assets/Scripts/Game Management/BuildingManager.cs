using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    [SerializeField] BuildingMenu buildingMenu;
    public List<string> unlockedBuildings { get; private set; } = new List<string>(); 

    public void UnlockBuilding(string buildingType)
    {
        if (!unlockedBuildings.Contains(buildingType))
        {
            unlockedBuildings.Add(buildingType);

            if (buildingMenu.gameObject.activeSelf)
            {
                buildingMenu.CreateBuildingMenu();
            }
        }
        else
        {
            Debug.LogWarning("Error with unlocking building " + buildingType);
        }
    }
}

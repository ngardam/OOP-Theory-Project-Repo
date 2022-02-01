using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Classes;

public class BuildingMenu : MonoBehaviour
{
    [SerializeField] GameObject content;

    [SerializeField] GameObject buildButtonPrefab;

    [SerializeField] BuildingManager buildingManager;

    private float buildButtonOffset = 90f; // Y offset of each button from previous one
    // Start is called before the first frame update
    void Start()
    {

        CreateBuildingMenu();

    }

    public void CreateBuildingMenu()
    {
        List<string> unlockedBuildings = buildingManager.unlockedBuildings;

        int i = 0;

        foreach (KeyValuePair<string, BuildingType> entry in BuildingDictionary.buildingDictionary)
        {
            GameObject newButton = Instantiate(buildButtonPrefab, content.transform);
            BuildingButton newBuildingButton = newButton.GetComponent<BuildingButton>();

            newButton.transform.Translate(0f, -i * buildButtonOffset, 0f);

            newButton.GetComponent<Button>().interactable = unlockedBuildings.Contains(entry.Key);

            newBuildingButton.buttonText.text = entry.Key;



            string infoText = WriteBuildingInfoText(entry.Value);

            newBuildingButton.infoText.text = infoText;

            i++;
        }
    }

    private string WriteBuildingInfoText(BuildingType buildingType)
    {
        string text = "Cost: \n";

        foreach (KeyValuePair<string, int> cost in buildingType.requiredToBuild)
        {
            text += cost.Key + ": " + cost.Value + "\n";
        }

        return text;
    }
}

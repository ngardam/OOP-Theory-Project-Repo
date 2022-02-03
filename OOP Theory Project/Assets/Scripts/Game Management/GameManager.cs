using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Classes;

public class GameManager : MonoBehaviour
{
    private bool loadingFromSave = false;

    [SerializeField] GameObject researchPanel;
    [SerializeField] GameObject buildMenuScrollView;

    public GameObject targetHexasphere;

    Vector3 seedVector;

    // Start is called before the first frame update
    void Start()
    {
        if (loadingFromSave)
        {
            LoadSaveData();
        }
        else
        {
            seedVector = Utilities.GenerateRandomVector3(-1000f, 1000f);
            GenerateNewGameWorld(seedVector);
            
        }

        
    }

    public void SetTargetHexasphere(GameObject hexasphereObject)
    {
        targetHexasphere = hexasphereObject;
    }

    private void LoadSaveData()
    {

    }

    private void GenerateNewGameWorld(Vector3 seedVector)
    {

        WorldGeneration worldGen = FindObjectOfType<WorldGeneration>();
        WorldGenSpecs specs = worldGen.GenerateRandomWorldGenSpecs();

        worldGen.GenerateWorld(specs);
    }

    public void ToggleResearchView()
    {
        researchPanel.SetActive(!researchPanel.activeSelf);

        if (researchPanel.activeSelf)
        {
            researchPanel.GetComponent<ResearchView>().StartDisplay();
        }
    }

    public void ToggleBuildingMenu()
    {
        if (!buildMenuScrollView.activeSelf)
        {
            buildMenuScrollView.SetActive(true);
            buildMenuScrollView.GetComponent<BuildingMenu>().CreateBuildingMenu();
        }
        else
        {
            buildMenuScrollView.SetActive(false);
            targetHexasphere.GetComponent<HexsphereInteraction>().StopPlaceBuildingMode();
        }
    }

    public void BuildButtonClicked(string buildingType)
    {
        GameObject previewPrefab = GetComponentInChildren<BuildingPrefabs>().GetPrefab(buildingType);

        HexsphereInteraction hexInteraction = targetHexasphere.GetComponent<HexsphereInteraction>();

        hexInteraction.StartPlaceBuildingMode(previewPrefab);
    }
}

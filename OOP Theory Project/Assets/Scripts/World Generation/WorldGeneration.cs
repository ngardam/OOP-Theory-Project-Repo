using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HexasphereGrid;
using static Classes;

public class WorldGeneration : MonoBehaviour
{
    [SerializeField] GameObject seedHexasphere;

    [SerializeField] Texture2D gradient;

    [SerializeField] GameObject villagePrefab;
    [SerializeField] GameObject villagerPrefab;

    public WorldGenSpecs GenerateRandomWorldGenSpecs()  //not very random
    {
        WorldGenSpecs specs = new WorldGenSpecs();

        specs.seedVector = Utilities.GenerateRandomVector3(-1000f, 1000f);

        specs.divisions = 36; //

        specs.scale = 20f;

        specs.terrainFrequency = 10f;
        specs.forestFrequency = 15f;

        specs.forestThreshold = 0.65f;
        specs.berryFrequency = 20f;
        specs.berryThreshold = 0.65f;

        specs.waterLevel = 0.35f;
        specs.sandLevel = 0.45f;
        specs.grassLevel = 0.65f;

        specs.numberOfStarterVillagers = 2;

        specs.bottomTopLevel = new float[3]
        {
            0.40f,
            0.70f,
            0.5f
        };


        specs.gradient = gradient;

        return specs;
    }

    public void GenerateWorld(WorldGenSpecs specs)
    {
        GameObject newHexa = Instantiate(seedHexasphere);

        seedHexasphere.SetActive(false);

        Hexasphere hexa = newHexa.GetComponent<Hexasphere>();
        HexaspherePopulation hexPop = newHexa.GetComponent<HexaspherePopulation>();

        newHexa.transform.localScale *= specs.scale;
        hexa.numDivisions = specs.divisions;
        newHexa.GetComponent<SphereCollider>().radius = 0.5f; // shrink radius from 0.55f so that entities on surface can be selected
        newHexa.GetComponent<HexasphereLogistics>().InitializeHexasphereLogistics();


        GetComponent<TerrainGeneration>().GenerateTerrain(hexa, specs);

        GetComponent<FloraGeneration>().PlaceForests(hexa, specs);

        GetComponent<FloraGeneration>().PlaceBerries(hexa, specs);

        int starterVillageTileIndex = CreateRandomVillage(hexa);

        for (int i = 0; i < specs.numberOfStarterVillagers; i++)
        {
            GameObject newVillager = AnimalManager.CreateAnimal(hexa, starterVillageTileIndex, villagerPrefab);
            hexPop.AddToPopulationList(newVillager);
        }




        CameraController cameraController = GameObject.Find("Main Camera").GetComponent<CameraController>();

        hexa.FlyTo(starterVillageTileIndex, 1f);



        //FindObjectOfType<PropPlacement>().PlaceFogOfWar(hexa);


        //Destroy(seedHexasphere);
    }

    private int CreateRandomVillage(Hexasphere hexa)
    {
        GameObject village;

        int numberOfAttempts = 1000;

        for (int i = 0; i < numberOfAttempts; i++)
        {
            int randomTileIndex = Random.Range(0, hexa.tiles.Length);

            if (StarterVillageLocationGood(hexa, randomTileIndex))
            {
                village = PropPlacement.CreateProp(hexa, randomTileIndex, villagePrefab);
                Tile _villageTile = hexa.tiles[randomTileIndex];
                village.GetComponent<Residential>().InitializeBuilding(hexa, randomTileIndex);
                _villageTile.gameObject = village;
                _villageTile.contents = "Village";

                
                return randomTileIndex;
            }

        }


        Debug.Log("No Location Found");
        return -1;
    }

    private bool StarterVillageLocationGood(Hexasphere hexa, int tileIndex)
    {

        Tile tile = hexa.tiles[tileIndex];

        bool isGood = true;

        isGood &= TileClear(tile) && WithinPolarBounds(hexa, tileIndex);

        return isGood;
    }

    private bool TileClear(Tile tile)
    {
        return (tile.contents == null && tile.terrain == "Flatland");
    }

    private bool WithinPolarBounds(Hexasphere hexa, int tileIndex)
    {
        float latitude = hexa.GetTileLatLon(tileIndex).x;
        float maxLatitude = 30f;

        return Mathf.Abs(latitude) <= maxLatitude;
    }

}

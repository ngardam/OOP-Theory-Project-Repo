using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HexasphereGrid;
using static Classes;

public class FloraGeneration : MonoBehaviour
{
    [SerializeField] GameObject[] forestPrefabs;

    [SerializeField] GameObject[] berriesPrefabs;

  //  [SerializeField] Hexasphere hexa;
  //
  //  Tile[] arrayOfTiles;
  //
  //  [SerializeField] float placeForestThreshold = 0.8f;
  //
  //  [SerializeField] float forestPerlinFrequency = 30f;
  //
  //  Vector3 forestPerlinSeed;

    //[SerializeField] PropPlacement propPlacement;

    private void Start()
    {
        //arrayOfTiles = hexa.tiles;

        //propPlacement = FindObjectOfType<PropPlacement>();


    }

    public void PlaceForests(Hexasphere hexa, WorldGenSpecs specs)
    {
        Vector3 transformedSeed = (specs.seedVector + new Vector3(478f, 130f, 847f)) * 2.8452f;  // "random" transformation

        Tile[] arrayOfTiles = hexa.tiles;

        foreach (Tile tile in arrayOfTiles)
        {
            if (tile.terrain == "Flatland" && tile.contents == null)
            {
                Vector3 pos = tile.center;
                float perlin = PerlinNoise3D.get3DPerlinNoise(pos + transformedSeed, specs.forestFrequency);
                perlin = Utilities.NormalizePerlin(perlin);

                if (perlin >= specs.forestThreshold)
                {
                    GameObject randomPrefab = forestPrefabs[Random.Range(0, forestPrefabs.Length)];
                    GameObject newForest = PropPlacement.CreateProp(hexa, tile.index, randomPrefab);
                    //hexa.SetTileTag(tile.index, "Forest");
                    tile.gameObject = newForest;
                    tile.contents = "Forest";
                }
            }
        }
    }

    public void PlaceBerries(Hexasphere hexa, WorldGenSpecs specs)
    {
        Tile[] arrayOfTiles = hexa.tiles;

        Vector3 transformedSeed = (specs.seedVector + new Vector3(292f, -784f, 992f)) * 4.4298f;  // "random" transformation

        foreach (Tile tile in arrayOfTiles)
        {
            if (tile.terrain == "Flatland" && tile.contents == null)
            {
                Vector3 pos = tile.center;
                float perlin = PerlinNoise3D.get3DPerlinNoise(pos + transformedSeed, specs.berryFrequency);
                perlin = Utilities.NormalizePerlin(perlin);

                if (perlin >= specs.berryThreshold)
                {
                    //initialize this tile's inventory
                    //tile.inventory = new Dictionary<string, int> { {"Berries", 0 }};
                    GameObject randomPrefab = berriesPrefabs[Random.Range(0, berriesPrefabs.Length)];
                    GameObject myProp = PropPlacement.CreateProp(hexa, tile.index, randomPrefab);

                    if (tile == null)
                    {
                        Debug.Log("Tile Null Warning 0");
                    }

                    myProp.GetComponent<Berries>().SetParentTile(tile);        
                    
                    tile.gameObject = myProp;
                    tile.contents = "Berries";
                    myProp.GetComponent<Berries>().SetRipeness(Random.Range(1, 150));

                }
            }
        }

    }
}

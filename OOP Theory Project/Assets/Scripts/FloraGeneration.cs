using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HexasphereGrid;

public class FloraGeneration : MonoBehaviour
{
    [SerializeField] GameObject[] forestPrefabs;

    [SerializeField] Hexasphere hexa;

    Tile[] arrayOfTiles;

    [SerializeField] float placeForestThreshold = 0.8f;

    [SerializeField] float forestPerlinFrequency = 30f;

    Vector3 forestPerlinSeed;

    PropPlacement propPlacement;

    private void Start()
    {
        arrayOfTiles = hexa.tiles;

        propPlacement = GetComponent<PropPlacement>();


    }

    public void PlaceForests()
    {
        forestPerlinSeed = new Vector3(Random.Range(-1000, 1000), Random.Range(-1000, 1000), Random.Range(-1000, 1000));

        foreach (Tile tile in arrayOfTiles)
        {
            if (tile.tag == "Clear")
            {
                Vector3 pos = tile.center;
                float perlin = PerlinNoise3D.get3DPerlinNoise(pos + forestPerlinSeed, forestPerlinFrequency);
                //normalize number to between 0 and 1
                perlin = (perlin + 1) / 2;
                if (perlin >= placeForestThreshold)
                {
                    GameObject randomPrefab = forestPrefabs[Random.Range(0, forestPrefabs.Length)];
                    propPlacement.PlaceProp(hexa, tile.index, randomPrefab);
                    hexa.SetTileTag(tile.index, "Forest");
                }
            }
        }
    }
}

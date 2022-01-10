using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HexasphereGrid;
//using Uni

public class HexasphereConstruction : MonoBehaviour
{
    private Hexasphere hexa;

    float perlinXRange = 1000;
    float perlinYRange = 1000;
    float perlinZRange = 1000;


    [SerializeField] PerlinNoise3D perlinNoise3D;
    [SerializeField] float perlinFrequency = 1f;

    [SerializeField] Texture2D gradient;

    [SerializeField] float flatBottom = 0.2f;
    [SerializeField] float flatTop = 0.6f;
    [SerializeField] float level = 0.2f;
    [SerializeField] float sandLevel = 0.24f;

    [SerializeField] Material waterMaterial;
    [SerializeField] Material grassMaterial;
    [SerializeField] Material sandMaterial;
    [SerializeField] Material mountainMaterial;

    Vector3 randomPerlinPosition;


    private void Start()
    {
        perlinNoise3D = FindObjectOfType<PerlinNoise3D>();
        hexa = GetComponent<Hexasphere>();
    }

    public void Apply3DPerlinNoiseToTileHeights()
    {
        randomPerlinPosition = new Vector3(Random.Range(-perlinXRange, perlinXRange),
                                           Random.Range(-perlinYRange, perlinYRange),
                                           Random.Range(-perlinZRange, perlinZRange));

        Tile[] tileArray = hexa.tiles;

        foreach (Tile tile in tileArray)
        {
            Vector3 tilePos = tile.center;

            float _height = PerlinNoise3D.get3DPerlinNoise(tilePos + randomPerlinPosition, perlinFrequency);

            _height = NormalizePerlin(_height); //attempting to normalize height to between 0 and 1

            hexa.SetTileExtrudeAmount(tile.index, _height);

            Debug.Log("setting height " + _height);
        }
    }

    public void ShadeFromGradient()
    {
        Tile[] tileArray = hexa.tiles;

        // Texture2D ourGradient = 

        int sizeOfGradientImage = gradient.width;
        Debug.Log("Texture width: " + sizeOfGradientImage);

        foreach (Tile tile in tileArray)
        {
            Color newColor;
            float height = hexa.GetTileExtrudeAmount(tile.index);


            newColor = gradient.GetPixel(Mathf.FloorToInt(height * sizeOfGradientImage), 0);

            hexa.SetTileColor(tile.index, newColor);
        }
    }

    public void ApplyPerlinToVertices()
    {
        Tile[] tileArray = hexa.tiles;

        float extrudeMultiplier = hexa.extrudeMultiplier;

        foreach (Tile tile in tileArray)
        {
            float tileHeight = tile.extrudeAmount;

            Vector3[] verticeArray = tile.vertices;

            if (tileHeight <= flatBottom || tileHeight >= flatTop)
            {

                foreach (Vector3 vertice in verticeArray)
                {
                    float newHeight = NormalizePerlin(PerlinNoise3D.get3DPerlinNoise((vertice + randomPerlinPosition), perlinFrequency));

                    newHeight *= extrudeMultiplier;

                    //float relativeHeight = tileHeight - newHeight;

                    //Debug.Log("elevation value: " + relativeHeight);

                    hexa.SetTileVertexElevation(tile.index, System.Array.IndexOf(verticeArray, vertice), newHeight / 10);
                }
            }
        }
    }

    public float NormalizePerlin(float rawPerlin)
    {
        return (rawPerlin + 1) / 2;
    }

    public void FlattenPlayArea()
    {
        Tile[] tileArray = hexa.tiles;

        foreach (Tile tile in tileArray)
        {
            float height = tile.extrudeAmount;

            if (height > flatBottom && height < flatTop)
            {
                hexa.SetTileExtrudeAmount(tile.index, level);
                hexa.SetTileTag(tile.index, "Clear");
            }
        }
    }

    public void ApplyWaterMaterial()
    {
        Tile[] tileArray = hexa.tiles;

        foreach (Tile tile in tileArray)
        {
            float height = tile.extrudeAmount;
            if (height <= flatBottom)
            {
                //tile.customMat = waterMaterial;
                //tile.extrudeAmount = flatBottom;
                hexa.SetTileMaterial(tile.index, waterMaterial);
                hexa.SetTileExtrudeAmount(tile.index, flatBottom);
            }
        }
    }

    public void TagTerrain()
    {
        Tile[] tileArray = hexa.tiles;

        foreach (Tile tile in tileArray)
        {
            float height = tile.extrudeAmount;

            if (height <= flatBottom)
            {
                hexa.SetTileTag(tile.index, "Water");
            }
            else if (height < sandLevel)
            {
                hexa.SetTileTag(tile.index, "Sand");
                Debug.Log("Sand tagged");
            }
            else if (height < flatTop)
            {
                hexa.SetTileTag(tile.index, "Grass");
            }
            else
            {
                hexa.SetTileTag(tile.index, "Mountain");
            }
        }
    }

    public void ApplyMaterials()
    {
        Tile[] tileArray = hexa.tiles;

        foreach (Tile tile in tileArray)
        {
            float randomRotation = Random.Range(0, 6) * 60;
            switch (tile.tag)
            {
                case "Water":
                    hexa.SetTileMaterial(tile.index, waterMaterial);
                    break;
                case "Sand":
                    hexa.SetTileMaterial(tile.index, sandMaterial);
                    break;
                case "Grass":
                    hexa.SetTileMaterial(tile.index, grassMaterial);
                    break;
                case "Mountain":
                    hexa.SetTileMaterial(tile.index, mountainMaterial);
                    break;
                default:
                    break;
            }
            hexa.SetTileTextureRotation(tile.index, randomRotation);
        }
    }
}

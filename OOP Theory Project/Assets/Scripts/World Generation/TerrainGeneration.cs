using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HexasphereGrid;
using static Classes;

public class TerrainGeneration : MonoBehaviour
{
    private Texture2D gradient;
    private int widthOfGradient;
    private Hexasphere targetHexa;
    private Tile[] arrayOfTiles;

    public void GenerateTerrain(Hexasphere hexa, WorldGenSpecs specs)
    {

        SetTargetHexa(hexa);
        widthOfGradient = specs.gradient.width;
        gradient = specs.gradient;
        float frequency = specs.terrainFrequency;
        Vector3 seedPos = specs.seedVector;




        foreach(Tile tile in arrayOfTiles)
        {
            Vector3 pos = tile.center;
            float perlinOutput = PerlinNoise3D.get3DPerlinNoise(pos + seedPos, frequency);
            perlinOutput = Utilities.NormalizePerlin(perlinOutput);

            tile.generationHeight = perlinOutput;

            hexa.SetTileExtrudeAmount(tile.index, perlinOutput);

            PaintTileFromGradient(tile);

            SetTerrainTagByHeight(tile, specs);

            //tile.contents = "None";



        }

        LevelAndDesignatePlayableArea(specs);

    }

    private void SetTerrainTagByHeight(Tile tile, WorldGenSpecs specs)
    {

        float height = tile.generationHeight;

        if (height < specs.waterLevel)
        {
            tile.terrain = "Water";
        }
        else if (height < specs.sandLevel)
        {
            tile.terrain = "Sand";
        }
        else if (height < specs.grassLevel)
        {
            tile.terrain = "Flatland";
        }
        else
        {
            tile.terrain = "Mountain";
        }
    
    }

    private void PaintTileFromGradient(Tile tile)
    {

            Color newColor;

            float height = tile.generationHeight;


            newColor = gradient.GetPixel(Mathf.FloorToInt(height * widthOfGradient), 0);

            targetHexa.SetTileColor(tile.index, newColor);
        
    }

    private void LevelAndDesignatePlayableArea(WorldGenSpecs specs)
    {
        foreach (Tile tile in arrayOfTiles)
        {
            float height = tile.generationHeight;
            float bottom = specs.bottomTopLevel[0];
            float top = specs.bottomTopLevel[1];
            float level = specs.bottomTopLevel[2];

            if (height > bottom && height < top)
            {
                targetHexa.SetTileExtrudeAmount(tile.index, level);
            } 
            else
            {
               targetHexa.SetTileCanCross(tile.index, false);  //this breaks it for some reason
            }
        }
    }

    public void SetTargetHexa(Hexasphere hexasphere)
    {
        targetHexa = hexasphere;
        arrayOfTiles = targetHexa.tiles;
    }
}

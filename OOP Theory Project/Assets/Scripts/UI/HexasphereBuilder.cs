using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HexasphereGrid;
using System;



public class HexasphereBuilder : MonoBehaviour
{
    [SerializeField] GameObject rock4Prefab;
    [SerializeField] GameObject seedHexasphere;
    [SerializeField] Slider divisionsSlider;

    [SerializeField] float generationSpeed = 0.1f;

    [SerializeField] float neighborHeightWeight = 1f;

    [SerializeField] float vertexHeightDamper = 5f;

    private (Color, float, string)[] colorHeights = new (Color, float, string)[]
    {
        (Color.white, 0.7f, "HighMountain"),
        (Color.gray, 0.61f, "Mountain"),
        (Color.green, 0.5f, "Grass"),
        (Color.yellow, 0.48f, "Sand"),
        (Color.blue, 0.0f, "Water")

    };

    private TerrainType[] terrainTypes;  //terrain in order of height

    GameObject newHex;
    Hexasphere hexa;

    private void Awake()
    {
        terrainTypes = new TerrainType[]
        {
             new TerrainType
             {
                 name = "Mountain",
                 prefab = rock4Prefab,
                 height = 0.6f,
                 color = Color.gray,

             },
             new TerrainType
             {
                 name = "Mountain",
                 prefab = rock4Prefab,
                 height = 0.6f,
                 color = Color.gray,

             },
             new TerrainType
             {
                 name = "Mountain",
                 prefab = rock4Prefab,
                 height = 0.6f,
                 color = Color.gray,

             },
             new TerrainType
             {
                 name = "Mountain",
                 prefab = rock4Prefab,
                 height = 0.6f,
                 color = Color.gray,

             }
        };
}

    private void Start()
    {

        //seedHexasphere.SetActive(true);
        buildNewHexasphere();
    }

    public void buildNewHexasphere()
    {
        if (newHex != null)
        {
            Destroy(newHex);
        }
        seedHexasphere.SetActive(true);


        int divisions = Mathf.FloorToInt(divisionsSlider.value);
        newHex = Instantiate(seedHexasphere);

        hexa = newHex.GetComponent<Hexasphere>();
        hexa.numDivisions = divisions;

        seedHexasphere.SetActive(false);
        

    }

    public void deleteHexasphere()
    {
        Destroy(newHex);
    }

    public void RandomizeHeightsStart()
    {
        StartCoroutine(RandomizeHeights());
    }

    public void SmartHeightsStart()
    {
        SmartHeights();
    }

    private void SmartHeights()
    {
        if (hexa != null)
        {
           // List<Vector3> processedTileVertices = new List<Vector3>();

            Tile[] arrayOfTiles = hexa.tiles;
            foreach (Tile tile in arrayOfTiles)
            {
                int[] neighborIndexes = hexa.GetTileNeighbours(tile.index);
                float[] neighborHeights = new float[neighborIndexes.Length];
                float neighborHeightTotal = 0;
                int numOfGeneratedNeighbors = 0;
                float averageNeighborHeight = 0f;

                for (int i = 0; i < neighborIndexes.Length; i++)
                {
                    float _height = hexa.GetTileExtrudeAmount(neighborIndexes[i]);
                    if (_height > 0)
                    {
                        //height has been set already
                        neighborHeightTotal += _height;
                        numOfGeneratedNeighbors++;
                        neighborHeights[i] = _height;
                    }

                }

                if (numOfGeneratedNeighbors > 0)
                {
                    averageNeighborHeight = neighborHeightTotal / (numOfGeneratedNeighbors);
                }

                float randomHeight = UnityEngine.Random.Range(0f, 1f);
                Debug.Log("average neighbor height: " + averageNeighborHeight);
                Debug.Log("new random height: " + randomHeight);
                float weightedNewHeight = ((numOfGeneratedNeighbors * (averageNeighborHeight * neighborHeightWeight)) + randomHeight) / ((numOfGeneratedNeighbors * neighborHeightWeight) + 1);
                Debug.Log("smart* height: " + weightedNewHeight);
                hexa.SetTileExtrudeAmount(tile.index, weightedNewHeight);

               // yield return new WaitForSeconds(generationSpeed);
            }
        }
    }

    public IEnumerator RandomizeHeights()
    {
       // bool inProgress = true;


        if (hexa != null)
        {
            Tile[] arrayOfTiles = hexa.tiles;
            foreach (Tile tile in arrayOfTiles)
            {
                float height = UnityEngine.Random.Range(0f, 1f);
                //tile.extrudeAmount = height;
                hexa.SetTileExtrudeAmount(tile.index, height);
                Debug.Log("changing Tile height" + height);
                yield return new WaitForSeconds(generationSpeed);
            }

            
        } else
        {
           // inProgress = false;
        }

        


    }

    private Color HeightToColor(float height)
    {
        Color newColor = Color.magenta;
        foreach ((Color, float, string) color in colorHeights)
        {
            if (height >= color.Item2)
            {
                newColor = color.Item1;
                break;
            }

        }
        return newColor;
    }

    public void TagAllByHeightAndGenerateProps()
    {
        Tile[] arrayOfTiles = hexa.tiles;
        foreach (Tile tile in arrayOfTiles)
        {
            TagTileByHeight(tile);
            BuildTileProp(tile);
        }
    }

    private void BuildTileProp(Tile tile)
    {
        TerrainType newTerrain = new TerrainType() ;
        foreach (TerrainType _terrain in terrainTypes)
        {
            if (_terrain.name == tile.tag)
            {
                newTerrain = _terrain;
                break;
            }
        }

        if (newTerrain.prefab != null)
            {
            float _height = tile.extrudeAmount;
            //hexa.SetTileExtrudeAmount(tile.index, 0.2f);

            GameObject newTileProp = Instantiate(newTerrain.prefab);
            hexa.ParentAndAlignToTile(newTileProp, tile.index);
            newTileProp.transform.Rotate(-90f, 0f, 0f, Space.Self);
            newTileProp.transform.Rotate(0f, UnityEngine.Random.Range(0f, 360f), 0f);
            newTileProp.transform.localScale *= 2;
            }
    }

    private void TagTileByHeight(Tile tile)  //Array of terrainTypes must be in order of highest to lowest
    {
        //string tag = "null";
        foreach (TerrainType terrain in terrainTypes)
        {
            if (terrain.height <= tile.extrudeAmount)
            {
                tile.tag = terrain.name;
                Debug.Log("Tagging as " + terrain.name);
                break;
            }

        }
    }

  //  private void ProcessTile(Tile tile)
  //  {
  //         // float height = hexa.GetTileExtrudeAmount(tile.index);
  //         // Color newColor = HeightToColor(height);
  //         // hexa.SetTileColor(tile.index, newColor);
  //  }

    public void ColorHeights()
    {
        Tile[] arrayOfTiles = hexa.tiles;

        foreach ( Tile tile in arrayOfTiles)
        {
            float height = hexa.GetTileExtrudeAmount(tile.index);
            Color newColor = HeightToColor(height);
            hexa.SetTileColor(tile.index, newColor);
        }
    }

    public void ApplyProps()
    {
        Tile[] arrayOfTiles = hexa.tiles;

        foreach (Tile tile in arrayOfTiles)
        {
            if (tile.tag == "Mountain")
            {

            }
            

        }
    }

    
    public void VertexTest()
    {
        hexa.enableGridEditor = true;

        Vector3[] vertices = hexa.tiles[0].vertices;
    }

    public void SmoothVertices()
    {
        List<Vector3> smoothedVertices;

        Tile[] arrayOfTiles = hexa.tiles;

        int verticesProcessed = 0;

        foreach(Tile tile in arrayOfTiles)
        {
            Tile[] neighborTiles = hexa.GetTileNeighboursTiles(tile.index);
            Vector3[] myVertices = tile.vertices;

            foreach (Vector3 vertex in myVertices)
            {
                verticesProcessed++;
                //  Gizmos.DrawSphere(vertex, 1);
                //Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere), vertex, Quaternion.identity);
                Tile[] neighborsSharingVertex = new Tile[2];
                int neighborsFound = 0;

                    foreach (Tile neighbor in neighborTiles)
                    {
                         if (neighborsFound < 2)
                         {
                             Vector3[] neighborVertices = neighbor.vertices;
                             foreach (Vector3 neighborVertex in neighborVertices)
                             {
                                 if (neighborVertex == vertex)
                                 {
                                     neighborsSharingVertex[neighborsFound] = neighbor;
                                     neighborsFound++;
                                     break;
                                 }
                             }
                         }
                    }

                float averageHeight = AverageTileHeights(tile, neighborsSharingVertex[0], neighborsSharingVertex[1]);

                //float adjustedHeight = averageHeight - hexa.GetTileExtrudeAmount(tile.index);

                hexa.SetTileVertexElevation(tile.index, Array.IndexOf(myVertices, vertex), averageHeight / vertexHeightDamper);

            }

        //   foreach(Tile neighborTile in neighborTiles)
        //   {
        //       Vector3[] neighborVertices = neighborTile.vertices;
        //   }
        }

        Debug.Log("vertices processed: " + verticesProcessed);

        
    }

    private float AverageTileHeights(Tile tile1, Tile tile2, Tile tile3)
    {
        float tile1Height = hexa.GetTileExtrudeAmount(tile1.index);
        float tile2Height = hexa.GetTileExtrudeAmount(tile2.index);
        float tile3Height = hexa.GetTileExtrudeAmount(tile3.index);

        float average = (tile1Height + tile2Height + tile3Height) / 3;

        return average;
    }

    //void ondra
    
}

public class TerrainType
{
    public string name;
    public GameObject prefab;
    public Material baseMaterial;
    public Color color;
    public float height;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HexasphereGrid;

public class HexasphereBuilder : MonoBehaviour
{
    [SerializeField] GameObject seedHexasphere;
    [SerializeField] Slider divisionsSlider;

    [SerializeField] float generationSpeed = 0.1f;

    [SerializeField] float neighborHeightWeight = 1f;

    private (Color, float)[] colorHeights = new (Color, float)[]
    {
        (Color.white, 0.7f),
        (Color.gray, 0.61f),
        (Color.green, 0.5f),
        (Color.yellow, 0.48f),
        (Color.blue, 0.0f)

    };

    GameObject newHex;
    Hexasphere hexa;

    private void Awake()
    {

    }

    private void Start()
    {

        seedHexasphere.SetActive(true);
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
        StartCoroutine(SmartHeights());
    }

    private IEnumerator SmartHeights()
    {
        if (hexa != null)
        {
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
                    //neighborHeights[i] = tile.extrudeAmount;
                    //Debug.Log("neighbor height: " + neigh);
                    //neighborHeightTotal += neighborHeights[i];
                }

                if (numOfGeneratedNeighbors > 0)
                {
                    averageNeighborHeight = neighborHeightTotal / (numOfGeneratedNeighbors);
                }

                float randomHeight = Random.Range(0f, 1f);
                Debug.Log("average neighbor height: " + averageNeighborHeight);
                Debug.Log("new random height: " + randomHeight);
                float weightedNewHeight = ((numOfGeneratedNeighbors * (averageNeighborHeight * neighborHeightWeight)) + randomHeight) / ((numOfGeneratedNeighbors * neighborHeightWeight) + 1);
                Debug.Log("smart height: " + weightedNewHeight);
                hexa.SetTileExtrudeAmount(tile.index, weightedNewHeight);

                yield return new WaitForSeconds(generationSpeed);
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
                float height = Random.Range(0f, 1f);
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
        foreach ((Color, float) color in colorHeights)
        {
            if (height >= color.Item2)
            {
                newColor = color.Item1;
                break;
            }

        }
        return newColor;
    }

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

    



}

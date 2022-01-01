using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HexasphereGrid;

public class HexasphereConstruction : MonoBehaviour
{
    private Hexasphere hexa;

    float perlinXRange = 1000;
    float perlinYRange = 1000;
    float perlinZRange = 1000;


    [SerializeField] PerlinNoise3D perlinNoise3D;
    [SerializeField] float perlinFrequency = 1f;


    private void Start()
    {
        perlinNoise3D = FindObjectOfType<PerlinNoise3D>();
        hexa = GetComponent<Hexasphere>();
    }

    public void Apply3DPerlinNoiseToTileHeights()
    {
        Vector3 randomPerlinPosition = new Vector3(Random.Range(-perlinXRange, perlinXRange),
                                                    Random.Range(-perlinYRange, perlinYRange),
                                                    Random.Range(-perlinZRange, perlinZRange));
        Tile[] tileArray = hexa.tiles;

        foreach (Tile tile in tileArray)
        {
            Vector3 tilePos = tile.center;

            float _height = PerlinNoise3D.get3DPerlinNoise(tilePos + randomPerlinPosition, perlinFrequency);

            _height = (_height + 1) / 2; //attempting to normalize height to between 0 and 1

            hexa.SetTileExtrudeAmount(tile.index, _height);

            Debug.Log("setting height " + _height);
        }
    }
}

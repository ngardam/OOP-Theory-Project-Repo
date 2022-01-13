using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HexasphereGrid;


public class HexsphereInteraction : MonoBehaviour
{
    private Hexasphere hexa;

    [SerializeField] TileInfoPanel tileInfoPanel;

    // Start is called before the first frame update
    void Start()
    {
        hexa = GetComponent<Hexasphere>();

        hexa.OnTileClick += (int index) => TileClicked(index);
    }

    void TileClicked(int tileIndex)
    {
        Tile tile = hexa.tiles[tileIndex];
        Debug.Log("Tile Clicked: " + tileIndex);
        tileInfoPanel.DisplayTileInfo(hexa, tile);
    }
}

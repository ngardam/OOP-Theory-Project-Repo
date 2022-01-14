using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HexasphereGrid;

//using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TileInfoPanel : MonoBehaviour
{

    //[SerializeField] Hexasphere targetHexa;

    private float refreshRate = 1f;

    private Tile targetTile;
    private Hexasphere targetHexasphere;

    private Text infoText;

    // Start is called before the first frame update
    void Start()
    {
        infoText = GetComponentInChildren<Text>();
    }


    public void DisplayTileInfo(Hexasphere hexa, Tile tile)
    {
        targetTile = tile;
        targetHexasphere = hexa;

        StartCoroutine(RefreshTileInfo());
    }

    IEnumerator RefreshTileInfo()
    {
        while (targetTile != null)
        {
            WriteTileInfo(targetTile);
            yield return new WaitForSeconds(refreshRate);
        }
    }

    private void WriteTileInfo(Tile tile)
    {
        string newText = "Index: " + tile.index + "\nCanCross: " + tile.canCross + "\nTerrain Type: " + tile.terrain + "\nContents: " + tile.contents;

        if (tile.contents == "Berries")
        {
            newText += "\n Ripeness: " + tile.gameObject.GetComponent<Berries>().ripeness;
        }

        newText += "\n\n";

        if (tile.inventory.Count > 0)
        {
            newText += "Inventory: \n";

            foreach (KeyValuePair<string, int> entry in tile.inventory)
            {
                newText += entry.Key + ":    " + entry.Value;
            }
        }

        newText += PrintLatLong(targetHexasphere, tile.index);

        infoText.text = newText;
    }

    private string PrintLatLong(Hexasphere hexa, int tileIndex)
    {
        float latitude = hexa.GetTileLatLon(tileIndex).x;
        float longitude = hexa.GetTileLatLon(tileIndex).y;

        return "\nLatitude: " + latitude + "\nLongitude: " + longitude;
    }


}

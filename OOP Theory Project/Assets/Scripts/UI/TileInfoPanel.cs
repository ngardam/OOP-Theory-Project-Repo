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
    private HexasphereLogistics hexaLogistics;

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
        hexaLogistics = hexa.GetComponentInParent<HexasphereLogistics>();

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
        else if(tile.contents == "Village")
        {
            newText += "\n New Villager Progress: " + tile.gameObject.GetComponent<Residential>().reproductionProgress;
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

        newText += PrintLogisticsInfo(hexaLogistics, tile.index);

        infoText.text = newText;
    }

    private string PrintLogisticsInfo(HexasphereLogistics logistics, int tileIndex)
    {
        string text = "Logistics: \n";
        if (logistics.pendingPickupArray[tileIndex].Count != 0)
        {
            Dictionary<string, int> dict = logistics.pendingPickupArray[tileIndex];
            text += "\n active requests:";
            foreach (KeyValuePair<string, int> entry in dict)
            {
                text += "\ntype: " + entry.Key + " qty: " + entry.Value;
            }
        }

        return text;
    }
    private string PrintLatLong(Hexasphere hexa, int tileIndex)
    {
        float latitude = hexa.GetTileLatLon(tileIndex).x;
        float longitude = hexa.GetTileLatLon(tileIndex).y;

        return "\nLatitude: " + latitude + "\nLongitude: " + longitude;
    }


}

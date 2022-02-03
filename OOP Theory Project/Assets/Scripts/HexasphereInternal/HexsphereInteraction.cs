using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HexasphereGrid;


public class HexsphereInteraction : MonoBehaviour
{
    private Hexasphere hexa;

    [SerializeField] TileInfoPanel tileInfoPanel;

    private bool placeBuildingMode;

    private GameObject buildingPreviewPrefab;

    private GameObject activeBuildingPreview;

    // Start is called before the first frame update
    void Start()
    {
        hexa = GetComponent<Hexasphere>();

        hexa.OnTileClick += (int index) => TileClicked(index);

        hexa.OnTileMouseOver += (int index) => OnTileMouseOver(index);
    }

    void TileClicked(int tileIndex)
    {
        Tile tile = hexa.tiles[tileIndex];
        Debug.Log("Tile Clicked: " + tileIndex);
        tileInfoPanel.DisplayTileInfo(hexa, tile);
    }

    void OnTileMouseOver(int tileIndex)
    {
        if (placeBuildingMode)
        {
            if (IsTileBuildable(tileIndex))
            {
                if (activeBuildingPreview != null)
                {
                    hexa.ParentAndAlignToTile(activeBuildingPreview, tileIndex);
                    activeBuildingPreview.transform.Rotate(-90f, 0f, 0f);
                }
                else
                {
                    activeBuildingPreview = Instantiate(buildingPreviewPrefab);
                    
                    hexa.ParentAndAlignToTile(activeBuildingPreview, tileIndex);
                    activeBuildingPreview.transform.Rotate(-90f, 0f, 0f);
                }

            }
        }
    }

    private bool IsTileBuildable(int tileIndex)
    {
        Tile tile = hexa.tiles[tileIndex];

        return (tile.terrain == "Flatland" && tile.contents == null);
    }

    public void StartPlaceBuildingMode(GameObject previewPrefab)
    {
        placeBuildingMode = true;
        buildingPreviewPrefab = previewPrefab;

    }

    public void StopPlaceBuildingMode()
    {
        placeBuildingMode = false;
        if (activeBuildingPreview != null)
        {
            Destroy(activeBuildingPreview);
        }
        buildingPreviewPrefab = null;
    }
}

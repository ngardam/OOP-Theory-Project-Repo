using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HexasphereGrid;



public class Person : Animal
{

    //[SerializeField] GameObject chaseObject;

 //  void Update()
 //  {
 //      if (mode == "Idle")
 //      {
 //
 //      }
 //  }

    protected override void SeekFood()
    {
        Tile foodSourceTile = FindNearestFoodSource();
        if (foodSourceTile == null)
        {
            Debug.Log("Cannot find food");
        }
        else
        {
            //destinationVector = foodSourceTile.;
            destinationVector = parentHexa.GetTileCenter(foodSourceTile.index, true);
            isTraveling = true;
        }
    }

    private Tile FindNearestFoodSource()
    {
        int myTileIndex = parentHexa.GetTileAtLocalPos(transform.localPosition);

        Debug.Log("My tile: " + myTileIndex);

        for (int i = 1; i <= searchLimit; i++)  //Should add check to current tile before this
        {
            List<int> tileIndexList = parentHexa.GetTilesWithinSteps(myTileIndex, i, i);

            Debug.Log("number of tiles with step " + i + ": " + tileIndexList.Count);

            foreach (int tileIndex in tileIndexList)
            {
                Tile tile = parentHexa.tiles[tileIndex];

                if (tile.inventory.ContainsKey("Berry"))
                {
                    if (tile.inventory["Berry"] > 0)
                    {
                        Debug.Log("Food source Tile: " + tile.index);
                        return tile;
                    }
                }

            }
        }

        return null;
    }

}

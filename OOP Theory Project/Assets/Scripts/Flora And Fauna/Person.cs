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
        if (!hasTask)
        {
            Tile foodSourceTile = FindNearestFoodSource();
            if (foodSourceTile == null)
            {
                Debug.Log("Cannot find food");
            }
            else
            {
                StartCoroutine(GoToFoodAndEatFromTileInventory(foodSourceTile));
                hasTask = true;

            }
        }
    }

    IEnumerator GoToFoodAndEatFromTileInventory(Tile tile)
    {
        Debug.Log("Foodseek corouting started:");
        int[] pathfindingTileIndexes = GeneratePathfindingTileIndices(tile);

        isTraveling = true;
        int step = 0;
        int numberOfSteps = pathfindingTileIndexes.Length;
        Vector3 destination;





        while(isTraveling)
        {
            destination = parentHexa.GetTileCenter(pathfindingTileIndexes[step]);

            MoveTowards(destination);

            if (AtLocation(destination))
            {
                step++;

                if (step == numberOfSteps)
                {

                    isTraveling = false;
                }
                else
                {
                    //destination = pathfindingSteps[step];
                }
            }

            yield return null;

        }
        //destination reached
        Debug.Log("Final destination reached");

        EatFromTile(tile);
        Debug.Log("Yum");
        hasTask = false;

        

    }



    private int[] GeneratePathfindingTileIndices(Tile destinationTile)
    {
        if (MyTileIndex() == destinationTile.index)
        {
            Debug.Log("At tile already");
            return new int[1] { destinationTile.index };
        }
        else
        {


            List<int> tileIndicesList = parentHexa.FindPath(MyTileIndex(), destinationTile.index);

            int[] indexArray = new int[tileIndicesList.Count];

            for (int i = 0; i < indexArray.Length; i++)
            {
                indexArray[i] = tileIndicesList[i];
            }

            return indexArray;
        }

    }

    private int MyTileIndex()
    {
        return parentHexa.GetTileAtLocalPos(transform.localPosition);
    }
    

    private Tile FindNearestFoodSource()
    {
        int myTileIndex = MyTileIndex();

        //Debug.Log("My tile: " + myTileIndex);

        for (int i = 1; i <= searchLimit; i++)  //Should add check to current tile before this
        {

            if(ContainsFood(myTileIndex))
            {
                return parentHexa.tiles[myTileIndex];
            }
            List<int> tileIndexList = parentHexa.GetTilesWithinSteps(myTileIndex, i, i);

            Debug.Log("number of tiles with step " + i + ": " + tileIndexList.Count);

            foreach (int tileIndex in tileIndexList)
            {
                Tile tile = parentHexa.tiles[tileIndex];

                if (ContainsFood(tileIndex))
                {
                    return tile;
                }
                

            }
        }

        return null;
    }

    private bool ContainsFood(int tileIndex)
    {
        Tile tile = parentHexa.tiles[tileIndex];

        if (tile.inventory.ContainsKey("Food"))
        {
            if (tile.inventory["Food"] > 0)
            {
                Debug.Log("Food source Tile: " + tile.index);
                return true;
            }

        }

        return false;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HexasphereGrid;
using static Classes;



public class Person : Animal
{

    //private TileLogisticsRequest activeLogisticsJob = new TileLogisticsRequest();

    private Dictionary<string, int> inventory = new Dictionary<string, int>();

    public HexasphereLogistics hexaLogistics;


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

        if(ContainsFood(myTileIndex))
        {
            return parentHexa.tiles[myTileIndex];
        }


        for (int i = 1; i <= searchLimit; i++)  
        {


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

    public void AssignWorkOrder(TileLogisticsRequest resourceRequest)
    {
        hasTask = true;
        StartCoroutine(FollowWorkOrder(resourceRequest));
    }

    IEnumerator FollowWorkOrder(TileLogisticsRequest logisticsOrder)
    {

        Debug.Log("Work order began: " + logisticsOrder.type);

        int[] pathfindingTileIndexes = GeneratePathfindingTileIndices(parentHexa.tiles[logisticsOrder.supplierIndex]);
        int step = 0;
        int numberOfSteps = pathfindingTileIndexes.Length;
        Tile interactTile;
        Vector3 destinationVector;
        isTraveling = true;

        while (isTraveling)
        {
            destinationVector = parentHexa.GetTileCenter(pathfindingTileIndexes[step]);

            MoveTowards(destinationVector);

            if (AtLocation(destinationVector))
            {
                step++;

                if (step == numberOfSteps)
                {
                    isTraveling = false;
                }

            }
            yield return null;
        }

        // we have reached the supplier tile
        interactTile = parentHexa.tiles[logisticsOrder.supplierIndex];

        PickUpFromSupplier(interactTile, logisticsOrder.type, 1);       //People only grab 1 at a time.. if need to grab more, will need to compare carry capacity with requested qty

        pathfindingTileIndexes = GeneratePathfindingTileIndices(parentHexa.tiles[logisticsOrder.requesterIndex]);
        step = 0;
        numberOfSteps = pathfindingTileIndexes.Length;
        isTraveling = true;

        while (isTraveling)
        {
            //TravelAlongPath(pathfindingTileIndexes);
            destinationVector = parentHexa.GetTileCenter(pathfindingTileIndexes[step]);

            MoveTowards(destinationVector);

            if (AtLocation(destinationVector))
            {
                step++;

                if (step == numberOfSteps)
                {
                    isTraveling = false;
                }

            }
            yield return null;
        }

        //we have arrive at the requester tile

        interactTile = parentHexa.tiles[logisticsOrder.requesterIndex];

        DropOffAtRequester(interactTile, logisticsOrder.type, 1);

        hasTask = false;




    }

 //   private void TravelAlongPath(List<int> pathfindingTileIndexes)
 //   {
 //
 //   }

    private void PickUpFromSupplier(Tile supplierTile, string type, int qty)
    {
        supplierTile.PickUpReservedItem(type, qty);
        AddToInventory(type, qty);

    }

    private void DropOffAtRequester(Tile requesterTile, string type, int qty)
    {
        requesterTile.AddItem(type, qty);
        SubtractFromInventory(type, qty);
        hexaLogistics.ConfirmDelivery(requesterTile.index, type, qty);
    }



    private void AddToInventory(string type, int qty)
    {
        if (inventory.ContainsKey(type))
        {
            inventory[type] += qty;
        }
        else
        {
            inventory.Add(type, qty);
        }
    }

    private void SubtractFromInventory(string type, int qty)
    {
        if (inventory.ContainsKey(type))
        {
            inventory[type] -= qty;
        }
        else
        {
            Debug.Log("Worker Inventory Error");
        }
    }

}

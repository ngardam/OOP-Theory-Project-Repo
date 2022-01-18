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

    private TileLogisticsRequest activeWorkOrder = new TileLogisticsRequest();



    private int pickupQty = 1; //pickup qty is always 1 right now

    protected override void StartLogic()
    {
        StartCoroutine(PersonLogic());
    }

  //  protected override void SeekFood()
  //  {
  //      if (!hasTask)
  //      {
  //          Tile foodSourceTile = FindNearestFoodSource();
  //          if (foodSourceTile == null)
  //          {
  //              Debug.Log("Cannot find food");
  //          }
  //          else
  //          {
  //             // hexaLogistics.AddToPendingPickupArray("Food", pickupQty, foodSourceTile.index);
  //              StartCoroutine(GoToFoodAndEatFromTileInventory(foodSourceTile));
  //              hasTask = true;
  //
  //          }
  //      }
  //  }

   // IEnumerator IdleBehavior()
   // {
   //     string idleMode = "null";
   //     while (!hasTask)
   //     {
   //         yield return new WaitForSeconds(idleBehaviorFrequency);
   //         if (idleMode == "null")
   //         {
   //             idleMode = RandomIdleMode();
   //             yield return StartCoroutine(PerformIdleAction(idleMode));
   //             Debug.Log("Idle Action Done");
   //             idleMode = "null";
   //         }
   //     }
   // }

   // IEnumerator PerformIdleAction(string idleMode)
   // {
   //     //bool onGoing = true;
   //     if (idleMode == "FaceRandomTile")
   //     {
   //         FaceRandomNeighbor();
   //         //onGoing = false;
   //     }
   //     else if (idleMode == "MoveToRandomNeighbor")
   //     {
   //         Vector3 pos = RandomNeighborPos();
   //         yield return StartCoroutine(GoToPosition(pos));
   //         //onGoing = MoveToRandomNeighbor();  //new neighbor is being chosen every frame, obviously this is not ideal
   //         //Debug.Log("Moving to random neighbor");
   //     }
   //
   //     //return onGoing;
   // }

   // private bool MoveToRandomNeighbor()
   // {
   //     
   //     int neighborIndex = RandomNeighborIndex();
   //     Vector3 randomNeighborPos = parentHexa.GetTileCenter(neighborIndex);
   //
   //     MoveTowards(randomNeighborPos);
   //
   //     return !AtLocation(randomNeighborPos);
   //
   //     
   // }

    private Vector3 RandomNeighborPos()
    {
        int index = RandomNeighborIndex();
        Vector3 pos = parentHexa.GetTileCenter(index);
        return pos;
    }

    

    private void FaceRandomNeighbor()
    {
        int randNeighbor = RandomNeighborIndex();
        Vector3 randomNeighborPos = parentHexa.GetTileCenter(randNeighbor);

        transform.LookAt(randomNeighborPos, Vector3.back);
        
    }

    private int RandomNeighborIndex()
    {
        int myTile = parentHexa.GetTileAtPos(gameObject.transform.position);
        int[] neighborIndexes = parentHexa.GetTileNeighbours(myTile);
        List<int> canCrossIndexes = new List<int>();

        foreach (int index in neighborIndexes)
        {
            if (parentHexa.GetTileCanCross(index))
            {
                canCrossIndexes.Add(index);
            }
        }
        int randNeighbor = canCrossIndexes[Random.Range(0, canCrossIndexes.Count)];

        return randNeighbor;
    }

    private string RandomIdleMode()
    {
        int randNum = Random.Range(0, 100);

        if (randNum < 25)
        {
            return "FaceRandomTile";
        }
        else if (randNum < 50)
        {
            return "MoveToRandomNeighbor";
        }
        else if (randNum < 75)
        {
            return "Ponder";
        }
        else
        {
            return "DoNothing";
        }

    }

   // IEnumerator GoToFoodAndEatFromTileInventory(Tile tile)
   // {
   //     hexaLogistics.AddToPendingPickupArray("Food", pickupQty, tile.index);
   //     Debug.Log("Foodseek corouting started:");
   //     int[] pathfindingTileIndexes = GeneratePathfindingTileIndices(tile);
   //
   //     isTraveling = true;
   //     int step = 0;
   //     int numberOfSteps = pathfindingTileIndexes.Length;
   //     Vector3 destination;
   //
   //     while(isTraveling)
   //     {
   //         destination = parentHexa.GetTileCenter(pathfindingTileIndexes[step]);
   //
   //         StartCoroutine(GoToPosition(destination));
   //
   //         if (AtLocation(destination))
   //         {
   //             step++;
   //
   //             if (step == numberOfSteps)
   //             {
   //
   //                 isTraveling = false;
   //             }
   //             else
   //             {
   //                 //destination = pathfindingSteps[step];
   //             }
   //         }
   //
   //         yield return null;
   //
   //     }
   //
   //
   //     EatFromTile(tile);
   //     hexaLogistics.RemoveFromPendingPickupArray("Food", pickupQty, tile.index);
   //     Debug.Log("Yum");
   //     TaskComplete();
   //
   //
   // }





    IEnumerator PersonLogic()
    {
        mode = "Idle";

        while(alive)
        {
            if (HasWorkOrder())
            {
                Debug.Log("Work order started");
                //mode = "Busy";
                yield return StartCoroutine(FulfillActiveWorkOrder());
                mode = "Idle";
                //activeWorkOrder = null;               //for some reason unloading the work order causes total stoppage
                Debug.Log("Work Order Complete");
            }
            yield return new WaitForSeconds(logicFrequency);
        }
    }

    IEnumerator FulfillActiveWorkOrder()
    {
        

        yield return StartCoroutine(PickUpAtTile(activeWorkOrder.supplierIndex, activeWorkOrder.type, pickupQty));
        //pickup complete.. test if pick up complete
        if (!inventory.ContainsKey(activeWorkOrder.type))
        {
            Debug.Log("Error 1 with Pickup");
            if (inventory[activeWorkOrder.type] != pickupQty)
            {
                Debug.Log("Error 2 with Pickup");
            }
        }
        yield return StartCoroutine(DropOffItems(activeWorkOrder.requesterIndex, activeWorkOrder.type, pickupQty));

        activeWorkOrder.isComplete = true;


        

    }

    IEnumerator DropOffItems(int tileIndex, string type, int qty)
    {
        yield return StartCoroutine(MoveToTile(tileIndex));

        DropOffAtRequester(parentHexa.tiles[tileIndex], type, qty);
    }

    IEnumerator PickUpAtTile(int tileIndex, string type, int qty)
    {
        yield return StartCoroutine(MoveToTile(tileIndex));
        PickUpFromSupplier(parentHexa.tiles[tileIndex], type, qty);
    }



    private bool HasWorkOrder()
    {
        return (activeWorkOrder.isComplete != true);
    }
    

    private Tile FindNearestFoodSource()
    {
        int myTileIndex = MyTileIndex();

        //Debug.Log("My tile: " + myTileIndex);

        if(ContainsUnreservedFood(myTileIndex))
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

                if (ContainsUnreservedFood(tileIndex))
                {
                    return tile;
                }
                

            }
        }

        return null;
    }

    private bool ContainsUnreservedFood(int tileIndex)
    {
        Tile tile = parentHexa.tiles[tileIndex];

        if (tile.inventory.ContainsKey("Food"))
        {
            if (tile.inventory["Food"] - hexaLogistics.HowManyPendingPickups("Food", tileIndex) > 0)
            {
                Debug.Log("Food source Tile: " + tile.index);
                return true;
            }

        }

        return false;
    }

    public void AssignWorkOrder(TileLogisticsRequest resourceRequest)
    {

        // hasTask = true;

        activeWorkOrder = resourceRequest;    //issue with... all order being assigned and marked pending..
        mode = "Busy";

        Debug.Log("Assigning new work order");

        //StartCoroutine(FollowWorkOrder(resourceRequest));
    }

   // IEnumerator FollowWorkOrder(TileLogisticsRequest logisticsOrder)
   // {
   //     hexaLogistics.AddToPendingPickupArray(logisticsOrder.type, pickupQty, logisticsOrder.supplierIndex);
   //     Debug.Log("Work order began: " + logisticsOrder.type);
   //
   //     int[] pathfindingTileIndexes = GeneratePathfindingTileIndices(parentHexa.tiles[logisticsOrder.supplierIndex]);
   //     int step = 0;
   //     int numberOfSteps = pathfindingTileIndexes.Length;
   //     Tile interactTile;
   //     Vector3 destinationVector;
   //     //StopCoroutine(GoToPosition);
   //     isTraveling = true;
   //
   //     while (isTraveling)
   //     {
   //         destinationVector = parentHexa.GetTileCenter(pathfindingTileIndexes[step]);
   //
   //         StartCoroutine(GoToPosition(destinationVector));
   //
   //         if (AtLocation(destinationVector))
   //         {
   //             step++;
   //
   //             if (step == numberOfSteps)
   //             {
   //                 isTraveling = false;
   //             }
   //
   //         }
   //         yield return null;
   //     }
   //
   //     // we have reached the supplier tile
   //     interactTile = parentHexa.tiles[logisticsOrder.supplierIndex];
   //
   //     PickUpFromSupplier(interactTile, logisticsOrder.type, 1);       //People only grab 1 at a time.. if need to grab more, will need to compare carry capacity with requested qty
   //
   //     pathfindingTileIndexes = GeneratePathfindingTileIndices(parentHexa.tiles[logisticsOrder.requesterIndex]);
   //     step = 0;
   //     numberOfSteps = pathfindingTileIndexes.Length;
   //     isTraveling = true;
   //
   //     while (isTraveling)
   //     {
   //         //TravelAlongPath(pathfindingTileIndexes);
   //         destinationVector = parentHexa.GetTileCenter(pathfindingTileIndexes[step]);
   //
   //         StartCoroutine(GoToPosition(destinationVector));
   //
   //         if (AtLocation(destinationVector))
   //         {
   //             step++;
   //
   //             if (step == numberOfSteps)
   //             {
   //                 isTraveling = false;
   //             }
   //
   //         }
   //         yield return null;
   //     }
   //
   //     //we have arrive at the requester tile
   //
   //     interactTile = parentHexa.tiles[logisticsOrder.requesterIndex];
   //
   //     DropOffAtRequester(interactTile, logisticsOrder.type, 1);
   //
   //     TaskComplete();
   //
   //
   //
   //
   // }

  //  private void TaskComplete()
  //  {
  //      hasTask = false;
  //      StartCoroutine(IdleBehavior());
  //  }

 //   private void TravelAlongPath(List<int> pathfindingTileIndexes)
 //   {
 //
 //   }

    private void PickUpFromSupplier(Tile supplierTile, string type, int qty)
    {
        //supplierTile.PickUpReservedItem(type, qty);
        supplierTile.RemoveItem(type, qty);
        hexaLogistics.RemoveFromPendingPickupArray(type, qty, supplierTile.index);
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

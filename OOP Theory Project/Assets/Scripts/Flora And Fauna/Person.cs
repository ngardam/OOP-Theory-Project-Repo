using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HexasphereGrid;
using static Classes;



public class Person : Animal
{

    private ResearchManager researchManager;

    private Dictionary<string, int> inventory = new Dictionary<string, int>();

    public HexasphereLogistics hexaLogistics;

    private TileLogisticsRequest activeWorkOrder = new TileLogisticsRequest();



    private int pickupQty = 1; //pickup qty is always 1 right now
    private int researchRate = 10; //How many research will be generated per research action

    protected override void StartLogic()
    {
        researchManager = GameObject.Find("Research Manager").GetComponent<ResearchManager>();

        StartCoroutine(PersonLogic());
    }



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
        int myTile = MyTileIndex();
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







    IEnumerator PersonLogic()
    {
        mode = "Idle";

        while(alive)
        {
            if (isHungry())
            {
                yield return StartCoroutine(FoodSeekBehavior());
            }

            if (HasWorkOrder())
            {
                Debug.Log("Work order started");
                mode = "Busy";
                yield return StartCoroutine(FulfillActiveWorkOrder());
                mode = "Idle";
                            
                Debug.Log("Work Order Complete");
            }
            else if (mode == "Idle")
            {
                string idleMode = RandomIdleMode();
                yield return StartCoroutine(PerformIdleAction(idleMode));
            }
            yield return new WaitForSeconds(logicFrequency);
        }
    }

    IEnumerator PerformIdleAction(string idleMode)
    {


        if (idleMode == "DoNothing")
        {
            yield return new WaitForSeconds(1f);

        }
        else if (idleMode == "FaceRandomTile")
        {
            FaceRandomNeighbor();
            yield return new WaitForSeconds(1f);
        }
        else if (idleMode == "MoveToRandomNeighbor")
        {
            int neighborIndex = RandomNeighborIndex();
            yield return StartCoroutine(MoveToTile(neighborIndex));
            yield return new WaitForSeconds(1f);
        }
        else if (idleMode == "Ponder")
        {
            Debug.Log("Hmm..");
            researchManager.AddResearch("Ponder", researchRate);
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator FoodSeekBehavior()
    {
        while (isHungry())
        {
            Tile foodSourceTile = FindNearestFoodSource();

            if (foodSourceTile != null)
            {
                hexaLogistics.AddToPendingPickupArray("Food", 1, foodSourceTile.index);

                yield return StartCoroutine(MoveToTile(foodSourceTile.index));
                EatFromTile(foodSourceTile);
                hexaLogistics.RemoveFromPendingPickupArray("Food", 1, foodSourceTile.index);
            }

            yield return null;
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



    private void PickUpFromSupplier(Tile supplierTile, string type, int qty)
    {

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

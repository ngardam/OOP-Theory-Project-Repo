using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HexasphereGrid;
using static Classes;


public class HexasphereLogistics : MonoBehaviour
{
    private int ManualCarrySearchLimit = 12; //The maximum number of tiles that your little people will travel for logistics tasks

    private Hexasphere hexa;

    private HexaspherePopulation hexPop;

    private List<TileLogisticsRequest> resourceRequests = new List<TileLogisticsRequest> { };

    private float logisticsRefreshRate = 0.1f;

    private bool isRunning = false;

    public Dictionary<string, int>[] pendingPickupArray { get; private set; }

    private int pickupQty = 1; //for now, only 1 item can be picked up at a time


    public void InitializeHexasphereLogistics()
    {
        hexa = GetComponent<Hexasphere>();
        hexPop = GetComponent<HexaspherePopulation>();
        pendingPickupArray = new Dictionary<string, int>[hexa.tiles.Length];

        for (int i = 0; i < pendingPickupArray.Length; i++)
        {
            pendingPickupArray[i] = new Dictionary<string, int>() { };
        }

        isRunning = true;


        StartCoroutine(Logistics());
    }

    private IEnumerator Logistics()
    {
        while (isRunning)
        {
            yield return new WaitForSeconds(logisticsRefreshRate);
            CleanUpList();
            IssueOrders();
        }
    }

    private void CleanUpList()
    {
        for (int i = resourceRequests.Count; i > 0 ; i--)
        {
            if (resourceRequests[i-1].qty == 0)
            {
                resourceRequests.Remove(resourceRequests[i-1]);
                Debug.Log("request removed from logistics list");
            }
            else if (resourceRequests[i-1].qty < 0)
            {
                Debug.LogWarning("Error: negative request");

            }
        }
    }

    private void IssueOrders()
    {
        foreach (TileLogisticsRequest request in resourceRequests)
        {
            if (request.active > request.qty)
            {
                Debug.Log("Error: active logistics job overflow");
            }
            else if (request.active < request.qty)
            {
                int supplierIndex = FindNearestIndexOfResource(request.requesterIndex, request.type);

                
                if (supplierIndex != -1)
                {
                    GameObject nearestIdleWorker = FindNearestIdleWorker(supplierIndex);

                    if (nearestIdleWorker != null)
                    {
                        TileLogisticsRequest NewWorkOrder = new TileLogisticsRequest();
                        NewWorkOrder.supplierIndex = supplierIndex;
                        NewWorkOrder.requesterIndex = request.requesterIndex;
                        NewWorkOrder.type = request.type;
                        //NewWorkOrder.supplierIndex = supplierIndex;
                        NewWorkOrder.qty = pickupQty;

                        //AddToPendingPickupArray(request.type, pickupQty, request.supplierIndex);
                        

                        request.active++;
                        nearestIdleWorker.GetComponent<Person>().AssignWorkOrder(NewWorkOrder);  //hmmmm. what's going on here? Need to create new request so changing it doesn't affect active work orders
                    }
                }
                else
                {
                    Debug.Log("Supplier Not Found");
                }
            }
            else
            {
                Debug.Log("All pending");
            }

        }


    }

    public void AddToPendingPickupArray(string type, int qty, int supplierIndex)
    {
        Dictionary<string, int> pendingPickupDict = pendingPickupArray[supplierIndex];

        if (pendingPickupDict.ContainsKey(type))
        {
            pendingPickupDict[type] += qty;
        }
        else
        {
            pendingPickupDict.Add(type, qty);
        }
        
    }

    public void RemoveFromPendingPickupArray(string type, int qty, int supplierIndex)
    {
        Dictionary<string, int> selectedDict = pendingPickupArray[supplierIndex];

        if (selectedDict.ContainsKey(type))
        {
            selectedDict[type] -= qty;
        }
    }

    public int HowManyPendingPickups(string type, int tileIndex)
    {
        
        if (pendingPickupArray[tileIndex].ContainsKey(type))
        {
            return pendingPickupArray[tileIndex][type];
        }
        else
        {
            return 0;
        }
    }

    public void ConfirmDelivery(int tileIndex, string type, int qty)
    {
        TileLogisticsRequest requestToUpdate = resourceRequests.Find(x => x.requesterIndex == tileIndex && x.type == type);
        requestToUpdate.active -= qty;
    }

    private GameObject FindNearestIdleWorker(int supplierIndex)
    {
        Vector3 supplierPos = hexa.GetTileCenter(supplierIndex);

        float smallestDistance = 10000f;
        int closestPersonIndex = -1;


        for (int i = 0; i < hexPop.populationList.Count; i++)
        {
            GameObject person = hexPop.populationList[i];
            bool hasTask = person.GetComponent<Person>().hasTask;

            float distance = Vector3.Distance(person.transform.position, supplierPos);
            
            if (distance < smallestDistance && !hasTask)
            {
                smallestDistance = distance;
                closestPersonIndex = i;
            }
        }

        if (smallestDistance <= ManualCarrySearchLimit)
        {
            return hexPop.populationList[closestPersonIndex];
        }
        else
        {
            return null;
        }
    }

    private int FindNearestIndexOfResource(int requester, string type)
    {
        //int index = -1;

        for (int i = 1; i <= ManualCarrySearchLimit; i++)
        {


            List<int> tileIndexList = hexa.GetTilesWithinSteps(requester, i, i);

            //Debug.Log("number of tiles with step " + i + ": " + tileIndexList.Count);

            foreach (int tileIndex in tileIndexList)
            {
                Tile tile = hexa.tiles[tileIndex];

                if (tile.CheckForItem(type))
                {
                    int qty = tile.HowMany(type);
                    int reservedQty = 0;

                    if (pendingPickupArray[tileIndex].ContainsKey(type))
                    {
                        reservedQty = pendingPickupArray[tileIndex][type];
                    }
                    

                    if (reservedQty < qty)
                    {
                        return tile.index;

                    }
                    
                }


            }
        }

        Debug.Log("indexOfNearestResource not found");
        return -1;
    }

    public void SubmitRequest(int _requesterIndex, string _type, int _qty)
    {
        TileLogisticsRequest newRequest = new TileLogisticsRequest
        {
            type = _type,
            requesterIndex = _requesterIndex,
            qty = _qty
        };

        ProcessNewRequest(newRequest);

        //resourceRequests.Add(newRequest);
    }

    private void ProcessNewRequest(TileLogisticsRequest request)
    {


        int requestIndex = resourceRequests.FindIndex(x => x.requesterIndex == request.requesterIndex && x.type == request.type);  //Is this tile already requesting these resources?

        if (requestIndex == -1)
        {

            resourceRequests.Add(request);
        }
        else
        {

            if (resourceRequests[requestIndex].qty != request.qty)
            {
                resourceRequests[requestIndex].qty = request.qty;

            }
            
        }
    }

}

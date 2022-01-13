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


    public void InitializeHexasphereLogistics()
    {
        hexa = GetComponent<Hexasphere>();
        hexPop = GetComponent<HexaspherePopulation>();
        isRunning = true;

        StartCoroutine(Logistics());
    }

    private IEnumerator Logistics()
    {
        while (isRunning)
        {
            yield return new WaitForSeconds(logisticsRefreshRate);
            IssueOrders();
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
                        request.supplierIndex = supplierIndex;
                        hexa.tiles[supplierIndex].SubmitPickupRequest(request.type, 1);
                        request.active++;
                        nearestIdleWorker.GetComponent<Person>().AssignWorkOrder(request);
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
                    int reservedQty = tile.HowManyPendingPickup(type);

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
        //TileLogisticsRequest preexistingRequest = resourceRequests.Find(x => x.requesterIndex == request.requesterIndex && x.type == request.type); 

        int requestIndex = resourceRequests.FindIndex(x => x.requesterIndex == request.requesterIndex && x.type == request.type);  //Is this tile already requesting these resources?
        //bool doesexist = resourceRequests.Find(x => x.requesterIndex == request.requesterIndex && x.type == request.type);
        if (requestIndex == -1)
        {
            Debug.Log("no request found, creating new");
            resourceRequests.Add(request);
        }
        else
        {
           // Debug.Log("problem here?");
            if (resourceRequests[requestIndex].qty != request.qty)
            {
                resourceRequests[requestIndex].qty = request.qty;
                Debug.Log("Updating request quantity: " + request.qty);
            }
            
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HexasphereGrid;
using static Classes;


public class HexasphereLogistics : MonoBehaviour
{
    private Hexasphere hexa;

    private List<TileLogisticsRequest> resourceRequests = new List<TileLogisticsRequest> { };


    public void InitializeHexasphereLogistics()
    {
        hexa = GetComponent<Hexasphere>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
                Debug.Log("Updating request quantity");
            }
            
        }
    }

}

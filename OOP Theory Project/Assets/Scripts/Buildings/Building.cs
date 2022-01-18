using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HexasphereGrid;

public abstract class Building : MonoBehaviour
{
    protected bool isActive;

    protected Hexasphere parentHexasphere;
    protected HexasphereLogistics hexaLogistics;
    protected int tileIndex;
    protected Tile parentTile;

    protected Dictionary<string, int> inventory;
    //private float logisticsPulseFrequency = 1f; //time in seconds between logistics request attempts

    protected Dictionary<string, int> resourceRequests = new Dictionary<string, int> { };

    public abstract void InitializeBuilding(Hexasphere _hexa, int _tileIndex);

    protected virtual void InventoryChanged()
    {
        EmitLogisticsRequests();
    }

    protected void EmitLogisticsRequests()
    {

        Debug.Log("Emitting");
        int qtyOnHand;
        int qtyWanted;
        int qtyRequested;

          
        
            
            foreach(KeyValuePair<string, int> entry in resourceRequests)
            {
                qtyOnHand = parentTile.HowMany(entry.Key);
                qtyWanted = entry.Value;
                qtyRequested = qtyWanted - qtyOnHand;

                hexaLogistics.SubmitRequest(tileIndex, entry.Key, qtyRequested);

            }

        
    }

    // Start is called before the first frame update

}

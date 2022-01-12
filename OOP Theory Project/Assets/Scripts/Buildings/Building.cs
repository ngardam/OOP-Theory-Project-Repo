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
    private float logisticsPulseFrequency = 1f; //time in seconds between logistics request attempts

    protected Dictionary<string, int> resourceRequests = new Dictionary<string, int> { };

    public abstract void InitializeBuilding(Hexasphere _hexa, int _tileIndex);

    protected IEnumerator EmitLogisticsRequests()
    {

        Debug.Log("Begin emission");
        int qtyOnHand;
        int qtyWanted;
        int qtyRequested;

        while (isActive)
        {
            yield return new WaitForSeconds(logisticsPulseFrequency);
            foreach(KeyValuePair<string, int> entry in resourceRequests)
            {
                qtyOnHand = parentTile.HowMany(entry.Key);
                qtyWanted = entry.Value;
                qtyRequested = qtyWanted - qtyOnHand;

                hexaLogistics.SubmitRequest(tileIndex, entry.Key, qtyRequested);

            }

        }
    }

    // Start is called before the first frame update

}

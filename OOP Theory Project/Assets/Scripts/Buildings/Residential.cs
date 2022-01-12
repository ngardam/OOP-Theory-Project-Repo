using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HexasphereGrid;

public class Residential : Building
{
    private int foodRequestQty = 10;
    public override void InitializeBuilding(Hexasphere _hexa, int _tileIndex)
    {
        parentHexasphere = _hexa;
        parentTile = _hexa.tiles[_tileIndex];
        tileIndex = _tileIndex;
        hexaLogistics = parentHexasphere.GetComponent<HexasphereLogistics>();
        isActive = true;

        resourceRequests.Add("Food", foodRequestQty);
        StartCoroutine(EmitLogisticsRequests());

    }
}

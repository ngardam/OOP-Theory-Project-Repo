using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HexasphereGrid;

public class Residential : Building
{
    private int foodRequestQty = 10;

    private int reproductionThreshold = 7;

    private float reproductionTimer = 1f;
    private float reproductionRate = 1f;


    public float reproductionProgress { get; private set;} = 0f;
    private float reproductionComplete = 100f; //make new villager

    [SerializeField] GameObject villagerPrefab;

    private float behaviorTimer = 1.1f; //behavior tick

    public override void InitializeBuilding(Hexasphere _hexa, int _tileIndex)
    {
        parentHexasphere = _hexa;
        parentTile = _hexa.tiles[_tileIndex];
        tileIndex = _tileIndex;
        hexaLogistics = parentHexasphere.GetComponent<HexasphereLogistics>();
        parentTile.inventoryChanged.AddListener(InventoryChanged);
        parentTile.AddItem("Food", 0);
        parentTile.contents = "Village";
        inventory = parentTile.inventory;

        isActive = true;
        StartCoroutine(ResidentialBehavior());

        resourceRequests.Add("Food", foodRequestQty);
        EmitLogisticsRequests();

    }

    IEnumerator ResidentialBehavior()
    {
        while (isActive)
        {
            if (inventory["Food"] >= reproductionThreshold)
            {
                reproductionProgress += reproductionRate;
            }
            else
            {
                reproductionProgress -= reproductionRate;
                if(reproductionProgress < 0f)
                {
                    reproductionProgress = 0f;
                }
            }

            if (reproductionProgress >= reproductionComplete)
            {
                AnimalManager.CreateAnimal(parentHexasphere, tileIndex, villagerPrefab);
                
                reproductionProgress = 0f;
            }
            yield return new WaitForSeconds(reproductionTimer);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HexasphereGrid;

public class Plant : MonoBehaviour
{
    protected Tile parentTile;

    private float health = 100f;

    private float healthMax = 100f;

    private float reproductionCycle = 0f;

    private float reproductionCycleMax;

    protected bool alive = true;

    public void SetParentTile(Tile tile)
    {
        parentTile = tile;
        //Debug.Log("Parent tile set");
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forest : Plant
{

    [SerializeField] GameObject[] trees;

    [SerializeField] GameObject[] stumps;

    private float growthFrequency = 1f; //time in seconds for tree growth to increase by growthRate

    private int growthRate = 10;
    private int maxGrowth = 50;

    public int maturity = 0;



    private bool growing = false;

    protected override void InventoryChanged()
    {
        int woodInventory = parentTile.inventory["Wood"];

        for (int i = 0; i < trees.Length; i++)
        {
            if (i < woodInventory)
            {
                trees[i].GetComponent<MeshRenderer>().enabled = true;

                stumps[i].SetActive(false);
            }
            else
            {
                trees[i].GetComponent<MeshRenderer>().enabled = false;
                //trees[i].SetActive(false);
                stumps[i].SetActive(true);
            }
        }

        if (woodInventory < trees.Length && !growing)
        {
            StartCoroutine(Grow());
        }
    }

    IEnumerator Grow()
    {
        growing = true;
        while (growing)
        {
            maturity += growthRate;

            if (maturity >= maxGrowth)
            {
                parentTile.AddItem("Wood", 1);
                maturity = 0;

                if (parentTile.inventory["Wood"] >= trees.Length)
                {
                    growing = false;
                }
                
            }



            yield return new WaitForSeconds(growthFrequency);
        }
    }

    public override void InitializePlant()
    {
        parentTile.AddItem("Wood", 3);
    }


}

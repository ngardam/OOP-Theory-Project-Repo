using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HexasphereGrid;

public class Berries : Plant
{
    //[SerializeField] InventoryManager inventoryManager;

    [SerializeField] GameObject[] ripeBerries;

   // public Tile parentTile;

    private Dictionary<string, int> inventory;

    public int ripeness = 0;

    private bool ripening = true;

    private int maxRipeness= 100;

    private float ripeningRate = 1f; //how long in seconds to gain one ripeness

    // Start is called before the first frame update
    void Start()
    {
        //inventory = parentTile.inventory;
        //inventoryManager = FindObjectOfType<InventoryManager>();



        DeactivateBerries();

        StartCoroutine(ripen());
    }

    IEnumerator ripen()
    {
        while(ripening && alive)
        {
            yield return new WaitForSeconds(ripeningRate);
            ripeness++;

            if (ripeness >= maxRipeness)
            {
                ProduceBerries();
                ripeness = 0;
                ripening = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetRipeness(int _ripeness)
    {
        if (_ripeness >= 0)
        {
            ripeness = _ripeness;
        }
    }

    private void DeactivateBerries()
    {
        foreach (GameObject berry in ripeBerries)
        {
            berry.SetActive(false);
        }
    }

    private void ProduceBerries()
    {
        if (parentTile == null)
        {
            Debug.Log("No Tile Warning");
        }
        foreach (GameObject berry in ripeBerries)
        {

            berry.SetActive(true);
            parentTile.AddItem("Food");
        }
    }

    protected override void InventoryChanged()
    {
        //Debug.Log("Item Removed Fired");
        int newQty = parentTile.inventory["Food"];

        for (int i = 0; i < ripeBerries.Length; i++)
        {
            if (i < newQty)
            {
                ripeBerries[i].SetActive(true);
            }
            else
            {
                ripeBerries[i].SetActive(false);
            }
        }
    }
}

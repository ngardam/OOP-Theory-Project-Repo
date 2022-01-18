using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace HexasphereGrid
{
    public partial class Tile
    {
        public GameObject gameObject;
        public string terrain;
        public string contents;
        public float generationHeight;
        //public HexasphereLogistics hexLogistics;

        public UnityEvent inventoryChanged = new UnityEvent();

        public Dictionary<string, int> inventory { get; private set; } = new Dictionary<string, int>() ;
        //public Dictionary<string, int> inventoryPendingPickup { get; private set; } = new Dictionary<string, int>();

        public void AddItem(string type, int qty)
        {
            if (inventory.ContainsKey(type))
            {
                inventory[type] += qty;
            }
            else
            {
                inventory.Add(type, qty);
            }

            inventoryChanged.Invoke();
        }

        public void AddItem(string type)
        {
            AddItem(type, 1);
        }

        public void RemoveItem(string type, int qty)
        {
            if (inventory.ContainsKey(type))
            {

                Debug.Log("Attempting to remove " + qty + " " + type + " from inventory of " + inventory[type]);
                if (inventory[type] >= qty)
                {
                    inventory[type] -= qty;
                }
                else
                {
                    Debug.Log("inventory remove error");
                    Debug.Log("Attempting to remove " + qty + " " + type + " from inventory of " + inventory[type]);
                }

                inventoryChanged.Invoke();
            }
            else
            {
                Debug.Log("Trying to remove item with no dict entry");
            }
        }

        public void RemoveItem(string type)
        {
            RemoveItem(type, 1);
        }

        public bool CheckForItem(string type, int qty)
        {

            if (inventory.ContainsKey(type))
            {
                if (inventory[type] >= qty)
                {
                    return true;
                }
            }
            //Debug.Log("Item not found");
            return false;
        }

        public bool CheckForItem(string type)
        {
            return CheckForItem(type, 1);
        }

        public int HowMany(string type)
        {
            if (CheckForItem(type))
            {
                return inventory[type];
            }
            else
            {
                return 0;
            }
        }

    //    public void SubmitPickupRequest(string type, int qty)
    //    {
    //        if (inventoryPendingPickup.ContainsKey(type))
    //        {
    //            inventoryPendingPickup[type] += qty;
    //        }
    //        else
    //        {
    //            inventoryPendingPickup.Add(type, qty);
    //        }
    //    }

     //   public void PickUpReservedItem(string type, int qty)
     //   {
     //       Debug.Log("Attempting to pick up " + type + " from tile " + contents);
     //       RemoveItem(type, qty);
     //       inventoryPendingPickup[type]--;
     //   }
     //   public void PickUpReservedItem(string type)
     //   {
     //       PickUpReservedItem(type, 1);
     //   }

       // public int HowManyPendingPickup(string type)
       // {
       //     if (inventoryPendingPickup.ContainsKey(type))
       //     {
       //         return inventoryPendingPickup[type];
       //     }
       //     else
       //     {
       //         return 0;
       //     }
       // }

    }
}

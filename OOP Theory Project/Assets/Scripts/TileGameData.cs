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

        public UnityEvent itemRemoved = new UnityEvent();

        public Dictionary<string, int> inventory { get; private set; } = new Dictionary<string, int>() ;

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
        }

        public void AddItem(string type)
        {
            AddItem(type, 1);
        }

        public void RemoveItem(string type, int qty)
        {
            if (inventory.ContainsKey(type))
            {

                if (inventory[type] >= qty)
                {
                    inventory[type] -= qty;
                }

                itemRemoved.Invoke();
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
            Debug.Log("Item not found");
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



    }
}

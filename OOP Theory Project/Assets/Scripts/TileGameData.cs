using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HexasphereGrid
{
    public partial class Tile
    {
        public GameObject gameObject;
        public string terrain;
        public string contents;
        public float generationHeight;

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
            }
        }
    }
}

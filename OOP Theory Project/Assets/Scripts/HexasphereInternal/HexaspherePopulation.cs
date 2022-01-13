using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexaspherePopulation : MonoBehaviour
{
    public List<GameObject> populationList { get; private set; } = new List<GameObject>();

    public void AddToPopulationList(GameObject person)
    {
        populationList.Add(person);
    }
}

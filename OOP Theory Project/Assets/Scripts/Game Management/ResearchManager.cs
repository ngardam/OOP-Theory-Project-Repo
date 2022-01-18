using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchManager : MonoBehaviour
{
    public Dictionary<string, int> researchPerformed { get; private set; } = new Dictionary<string, int>
    {
        {"Agriculture", 0 },
        {"Mining", 0 }
    };

    public void AddResearch(string researchType, int qty)
    {
        if (researchPerformed.ContainsKey(researchType))
        {
            researchPerformed[researchType] += qty;
        }
        else
        {
            Debug.LogWarning("Research entry not found");
        }
    }




}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResearchView : MonoBehaviour
{

    private float refreshRate = 1f;

    private ResearchManager researchManager;
    private Dictionary<string, int> researchInventory;

    private Text text;

    // Start is called before the first frame update
    void Start()
    {
      //  text = GetComponentInChildren<Text>();
      //  researchManager = GameObject.Find("Research Manager").GetComponent<ResearchManager>();
      //  researchInventory = researchManager.researchPerformed;

        StartCoroutine(DisplayInfo());
    }



    public void StartDisplay()
    {
        researchManager = GameObject.Find("Research Manager").GetComponent<ResearchManager>();
        researchInventory = researchManager.researchPerformed;
        text = GetComponentInChildren<Text>();
        StartCoroutine(DisplayInfo());

    }

    IEnumerator DisplayInfo()
    {
        bool displaying = true;
        while (displaying)
        {
            WriteDisplay();
            yield return new WaitForSeconds(refreshRate);
        }
    }

    private void WriteDisplay()
    {
        string newText = "";

        foreach(KeyValuePair<string, int> entry in researchInventory)
        {
            newText += entry.Key + ": " + entry.Value + "\n";
        }

        text.text = newText;
    }
}

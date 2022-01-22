using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResearchView : MonoBehaviour
{

    private float refreshRate = 1f;

    private ResearchManager researchManager;
    private Dictionary<string, int> researchInventory;

    ResearchIcon[] researchIcons;

    private Text text;

    // Start is called before the first frame update
    void Start()
    {
        //  text = GetComponentInChildren<Text>();
        //  researchManager = GameObject.Find("Research Manager").GetComponent<ResearchManager>();
        //  researchInventory = researchManager.researchPerformed;

        StartDisplay();
    }



    public void StartDisplay()
    {
        researchIcons = GameObject.FindObjectsOfType<ResearchIcon>();
        researchManager = GameObject.Find("Research Manager").GetComponent<ResearchManager>();
        researchInventory = researchManager.researchPerformed;
        text = GetComponentInChildren<Text>();
        StartCoroutine(DisplayInfo());

    }

    private void RefreshTiles()
    {
        foreach(ResearchIcon icon in researchIcons)
        {
            bool prereqsComplete = CheckPrerequisites(icon);

            bool complete = icon.complete;

            Button iconButton = icon.gameObject.GetComponent<Button>();

            iconButton.interactable = prereqsComplete && !complete;
        }
    }

    private bool CheckPrerequisites(ResearchIcon icon)
    {
        bool complete = true;

        if (icon.prerequisites.Length > 0)
        {
            foreach (ResearchIcon _icon in researchIcons)
            {
                foreach (string prereq in icon.prerequisites)
                {
                    if (_icon.researchName == prereq)
                    {
                        complete &= _icon.complete;
                    }
                }
            }
        }

        return complete;

    }


    IEnumerator DisplayInfo()
    {
        bool displaying = true;
        while (displaying)
        {
            WriteDisplay();
            RefreshTiles();
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

    private void OnDrawGizmos()
    {
        ResearchIcon[] researchIcons = GameObject.FindObjectsOfType<ResearchIcon>();

        foreach (ResearchIcon icon in researchIcons)
        {
            if (icon.prerequisites.Length > 0)
            {
                foreach (string prereq in icon.prerequisites)
                {
                    foreach (ResearchIcon _icon in researchIcons)
                    {
                        if (_icon.researchName == prereq)
                        {
                            Gizmos.DrawLine(icon.gameObject.transform.position, _icon.gameObject.transform.position);
                        }
                    }
                }
            }
        }
    }
}


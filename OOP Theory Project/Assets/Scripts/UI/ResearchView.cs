using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResearchView : MonoBehaviour
{

    private float refreshRate = 1f;

    private ResearchManager researchManager;
    private Dictionary<string, Research> researchTracker;

    [SerializeField] GameObject researchInfoPanel;

    private string selectedResearch;

    public Text infoTitleText;
    [SerializeField] Text infoBodyText;

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
        researchTracker = researchManager.researchTracker;
        text = GetComponentInChildren<Text>();
        StartCoroutine(DisplayInfo());

    }

    private void RefreshTiles()
    {
        foreach(ResearchIcon icon in researchIcons)
        {
            bool prereqsComplete = CheckPrerequisites(icon.researchName);

            bool complete = researchTracker[icon.researchName].isComplete;

            Button iconButton = icon.gameObject.GetComponent<Button>();

            iconButton.interactable = prereqsComplete && !complete  ;

            Slider slider = icon.slider;

            float completeRatio = researchManager.ResearchCompleteRatio(icon.researchName);

            if (completeRatio > 0 && completeRatio < 1)
            {
                slider.gameObject.SetActive(true);
                slider.value = completeRatio;
            }
            else
            {
                if (slider.gameObject.activeSelf)
                {
                    Debug.Log("setting slider inactive " + icon.researchName);
                    slider.gameObject.SetActive(false);
                }
            }
        }
    }

    private bool CheckPrerequisites(string researchName)
    {
        bool complete = true;
        Debug.Log("Checking " + researchName);
        Research research = researchTracker[researchName];

        if (research.prerequisites.Length > 0)
        {
            foreach (string prerequisite in research.prerequisites)
            {
                Debug.Log("Checking prereq: " + prerequisite);
                if (researchTracker.ContainsKey(prerequisite))
                {
                    complete &= researchTracker[prerequisite].isComplete;
                }
                else
                {
                    Debug.Log("Research " + prerequisite + " not found");
                }
            }
        }

        //    foreach (ResearchIcon _icon in researchIcons)
        //    {
        //        foreach (string prereq in icon.prerequisites)
        //        {
        //            if (_icon.researchName == prereq)
        //            {
        //                complete &= _icon.complete;
        //            }
        //        }
        //    }
      //  }

        return complete;

    }


    IEnumerator DisplayInfo()
    {
        bool displaying = true;
        while (displaying)
        {
           // WriteDisplay();
            RefreshTiles();
            DisplayResearchInfo(selectedResearch);
            yield return new WaitForSeconds(refreshRate);
        }
    }

//   private void WriteDisplay()
//   {
//       string newText = "";
//
//       foreach(KeyValuePair<string, int> entry in researchInventory)
//       {
//           newText += entry.Key + ": " + entry.Value + "\n";
//       }
//
//       text.text = newText;
//   }

    public void ResearchIconClicked(string researchType)
    {
        if (infoTitleText.text == researchType && researchInfoPanel.activeSelf)
        {
            researchInfoPanel.SetActive(false);
        }
        else
        {
            selectedResearch = researchType;
            DisplayResearchInfo(researchType);
        }
    }

    private void DisplayResearchInfo(string researchType)
    {
        if (researchType != null)
        {
            researchInfoPanel.SetActive(true);
            infoTitleText.text = researchType;

            infoBodyText.text = GenerateResearchInfo(researchType);
        }
    }

    private string GenerateResearchInfo(string researchType)
    {
        string text = "No Entry Found";
        if (ResearchTypes.ResearchDictionary.ContainsKey(researchType))
        {
            text = "Required for research: \n";

            Research research = researchManager.researchTracker[researchType];

            foreach (KeyValuePair<string, int> entry in research.RequiredForResearch)
            {
                text += research.completedResearch[entry.Key] + "/" + entry.Value + " " + entry.Key + "\n";
            }
        }
        return text;
    }

  //  private void OnDrawGizmos()
  //  {
  //      ResearchIcon[] researchIcons = GameObject.FindObjectsOfType<ResearchIcon>();
  //
  //      foreach (ResearchIcon icon in researchIcons)
  //      {
  //          if (icon.prerequisites.Length > 0)
  //          {
  //              foreach (string prereq in icon.prerequisites)
  //              {
  //                  foreach (ResearchIcon _icon in researchIcons)
  //                  {
  //                      if (_icon.researchName == prereq)
  //                      {
  //                          Gizmos.DrawLine(icon.gameObject.transform.position, _icon.gameObject.transform.position);
  //                      }
  //                  }
  //              }
  //          }
  //      }
  //  }
}


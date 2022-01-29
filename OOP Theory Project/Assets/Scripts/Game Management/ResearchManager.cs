using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResearchManager : MonoBehaviour
{

    [SerializeField] GameObject researchButtonPanel;
    [SerializeField] GameObject buildMenuButtonPanel;



    private string selectedResearch;



    private float researchRefreshRate = 1f;



    bool researchButtonComplete = false;



    public Dictionary<string, Research> researchTracker;

    private void Start()
    {
        researchTracker = BuildResearchTracker();
        selectedResearch = "Research Button";



        StartCoroutine(ResearchStory());
    }

    private Dictionary<string, Research> BuildResearchTracker()
    {
        Dictionary<string, Research> masterDict = ResearchTypes.ResearchDictionary;
        Dictionary<string, Research> myResearchTracker = new Dictionary<string, Research>();

        foreach (KeyValuePair<string, Research> entry in masterDict)
        {
            Research myResearch = new Research();
            myResearch.RequiredForResearch = entry.Value.RequiredForResearch;
            // create dictionary of completed research with values set to zero

            Dictionary<string, int> myFinishedResearch = new Dictionary<string, int>();

            foreach (KeyValuePair<string, int> researchType in myResearch.RequiredForResearch)

            {
                myFinishedResearch.Add(researchType.Key, 0);
            }
            myResearch.completedResearch = myFinishedResearch;
            myResearch.prerequisites = entry.Value.prerequisites;
            myResearch.isComplete = false;

            myResearchTracker.Add(entry.Key, myResearch);
        }

        return myResearchTracker;
    }

    public void AddToActiveResearch(string researchType, int qty)
    {
        if (researchTracker.ContainsKey(selectedResearch))
        {
            if (researchTracker[selectedResearch].completedResearch.ContainsKey(researchType))
            {
                int done = researchTracker[selectedResearch].completedResearch[researchType];
                int needed = researchTracker[selectedResearch].RequiredForResearch[researchType];
                if (done < needed)
                {
                    done += qty;
                }
                else
                {
                    done = needed;


                    researchTracker[selectedResearch].isComplete = (IsResearchComplete(selectedResearch));
                    
                }

                researchTracker[selectedResearch].completedResearch[researchType] = done;
                
            }
            else
            {
                Debug.Log("No need for " + researchType + " research");
            }
        }
        else
        {
            Debug.LogWarning("Research entry not found");
        }
    }

    private bool IsResearchComplete(string researchSelection)
    {
        bool isComplete = true;

        Research research = researchTracker[researchSelection];

        foreach (KeyValuePair<string, int> resourceRequirement in research.RequiredForResearch)
        {
            int needs = resourceRequirement.Value;
            int has = research.completedResearch[resourceRequirement.Key];

            isComplete &= has >= needs;
        }
        if (isComplete != research.isComplete)
        {
           // UpdateResearchView();
        }

        return isComplete;
    }

 //  private void UpdateResearchView()
 //  {
 //      
 //  }

    IEnumerator ResearchStory()
    {
        GrowButton(buildMenuButtonPanel, 0.0f);
        yield return StartCoroutine(ResearchButtonResearch());

        yield return StartCoroutine(BuildingMenuButtonAppears());
    }

    IEnumerator ResearchButtonResearch()
    {


        while (researchButtonComplete == false)
        {
            int ponderQTY = researchTracker["Research Button"].completedResearch["Ponder"];
            float completeRatio = (float)ponderQTY / researchTracker["Research Button"].RequiredForResearch["Ponder"];//researchButtonCost;

            Debug.Log("research button ratio: " + completeRatio);


            GrowButton(researchButtonPanel, completeRatio);
            if (completeRatio >= 1)
            {
                researchButtonComplete = true;
            }

            yield return new WaitForSeconds(researchRefreshRate);
        }
    }

    IEnumerator BuildingMenuButtonAppears()
    {
        bool buildMenuComplete = false;
        string[] researchToMonitor = new string[] { "Village" };

        float completeRatio;


        while (!buildMenuComplete)
        {

            float highestRatio = 0.0f;

            foreach (string researchType in researchToMonitor)
            {
                float thisRatio = ResearchCompleteRatio(researchType);
                if (thisRatio > highestRatio)
                {
                    highestRatio = thisRatio;
                }
            }

            completeRatio = highestRatio;

            GrowButton(buildMenuButtonPanel, completeRatio);

            buildMenuComplete = (completeRatio >= 1f);
            yield return new WaitForSeconds(researchRefreshRate);
        }
    }

    private void GrowButton(GameObject buttonPanel, float completeRatio)
    {
        Slider slider = buttonPanel.GetComponentInChildren<Slider>();


        Button button = buttonPanel.GetComponentInChildren<Button>();

        Image panelImage = buttonPanel.GetComponent<Image>();
        RawImage iconImage = buttonPanel.GetComponentInChildren<RawImage>();
        Image sliderImage = slider.image;


        panelImage.color = SetColorAlpha(panelImage.color, completeRatio);
        iconImage.color = SetColorAlpha(iconImage.color, completeRatio);
        sliderImage.color = SetColorAlpha(sliderImage.color, completeRatio);
        slider.value = completeRatio;



        if (completeRatio >= 1)
        {
            panelImage.color = SetColorAlpha(panelImage.color, 1f);
            iconImage.color = SetColorAlpha(iconImage.color, 1f);
            sliderImage.color = SetColorAlpha(sliderImage.color, 1f);

            slider.colors = SetColorMultiplier(slider.colors, 2f); //brighten up button when it's ready
            slider.value = 1f;
            button.interactable = true;
            
        }
    }

    private float ResearchCompleteRatio(string researchType)
    {
        float complete = 0f;
        int totalRequired = 0;
        int totalComplete = 0;
        Research research = researchTracker[researchType];

        foreach(KeyValuePair<string, int> m_research in research.RequiredForResearch)
        {
            totalRequired += m_research.Value;
            totalComplete += research.completedResearch[m_research.Key];
        }

        complete = (float)totalComplete / totalRequired;


        return complete;
    }

    private Color SetColorAlpha(Color baseColor, float alpha)
    {
        Color color = baseColor;
        color.a = Mathf.Clamp(alpha, 0, 1);
        return color;
    }

    private ColorBlock SetColorMultiplier(ColorBlock baseBlock, float multiplier)
    {
        ColorBlock colorBlock = baseBlock;
        colorBlock.colorMultiplier = Mathf.Clamp(multiplier, 1, 5);
        return colorBlock;
    }

    public void ConfirmResearchButton()
    {
        string selection = FindObjectOfType<ResearchView>().infoTitleText.text;
        selectedResearch = selection;

        if (ResearchTypes.ResearchDictionary.ContainsKey(selection))
        {
            Research researchEntry = ResearchTypes.ResearchDictionary[selection];

            SetAllResearchFacilities(selection);

            //activeResearch = researchEntry;
            //activeResearch.RequiredForResearch = researchEntry.RequiredForResearch;

        }
        else
        {
            Debug.Log("Research " + selection + " Not Found");
        }
    }

    private void SetAllResearchFacilities(string researchName)
    {

    }




}

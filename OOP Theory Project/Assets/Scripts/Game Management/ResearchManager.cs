using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResearchManager : MonoBehaviour
{

    [SerializeField] GameObject researchButtonPanel;

    Button researchButton;

    private float researchRefreshRate = 1f;

    private int researchButtonCost = 100;

    bool researchButtonComplete = false;
    public Dictionary<string, int> researchPerformed { get; private set; } = new Dictionary<string, int>
    {
        {"Agriculture", 0 },
        {"Mining", 0 },
        {"Ponder", 0 }
    };

    private void Start()
    {
        StartCoroutine(ResearchStory());
    }

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

    IEnumerator ResearchStory()
    {
        yield return StartCoroutine(ResearchButtonResearch());
    }

    IEnumerator ResearchButtonResearch()
    {
        //GameObject researchButtonPanel = GameObject.Find("Research Button Panel");


        //float buttonAppearPeriod = 1f; // of 1, how long this will take in the button appear animation
        researchButton = researchButtonPanel.GetComponent<Button>();

        //Color panelImageColor = researchButtonPanel.GetComponent<Image>().color;
        //Color testTubeImageColor = researchButtonPanel.GetComponentInChildren<RawImage>().color;
        

        Slider slider = researchButtonPanel.GetComponentInChildren<Slider>();
        //Color sliderImageColor = slider.colors.disabledColor;

        Image panelImage = researchButtonPanel.GetComponent<Image>();
        RawImage testTubeImage = researchButtonPanel.GetComponentInChildren<RawImage>();
        Image sliderImage = slider.image;

        while (researchButtonComplete == false)
        {
            int ponderQTY = researchPerformed["Ponder"];
            float completeRatio = (float)ponderQTY / researchButtonCost;

            Debug.Log("research button ratio: " + completeRatio);

            panelImage.color = SetColorAlpha(panelImage.color, completeRatio);
            testTubeImage.color = SetColorAlpha(testTubeImage.color, completeRatio);
            sliderImage.color = SetColorAlpha(sliderImage.color, completeRatio);
            slider.value = completeRatio;

            

            if (completeRatio >= 1)
            {
                panelImage.color = SetColorAlpha(panelImage.color, 1f);
                testTubeImage.color = SetColorAlpha(testTubeImage.color, 1f);
                sliderImage.color = SetColorAlpha(sliderImage.color, 1f);

                slider.colors = SetColorMultiplier(slider.colors, 2f); //brighten up button when it's ready
                slider.value = 1f;
                researchButton.interactable = true;
                researchButtonComplete = true;
            }

            yield return new WaitForSeconds(researchRefreshRate);
        }
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




}

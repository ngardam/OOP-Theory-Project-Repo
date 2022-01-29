using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResearchIcon : MonoBehaviour
{

    //public string[] prerequisites;

    //[SerializeField] string[] unlocks { get; [SerializeField] set; }

    public string researchName;

    //public bool complete;

    public void ButtonClicked()
    {
      //  complete = true;

        Button button = gameObject.GetComponent<Button>();
      //  button.interactable = false;



        ColorBlock colors = new ColorBlock();
        colors = button.colors;
        colors.disabledColor = Color.green;

        button.colors = colors;

        ResearchView researchView = FindObjectOfType<ResearchView>();

        researchView.ResearchIconClicked(researchName);
    }

}

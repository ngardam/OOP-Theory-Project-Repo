using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntityInfoPanel : MonoBehaviour
{
    private GameObject targetEntity;
    private Text infoText;
    private bool refreshRunning = false;

    private float refreshRate = 1.0f; //refresh rate in seconds


    void Start()
    {
        infoText = GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadEntity(GameObject go)
    {
        targetEntity = go;

        if (!refreshRunning)
        {
            StartCoroutine(RefreshInfoText());
        }
        ConstructEntityInfoText(targetEntity);


    }

    IEnumerator RefreshInfoText()
    {
        refreshRunning = true;
        while (targetEntity != null)
        {
            string text = ConstructEntityInfoText(targetEntity);
            infoText.text = text;
            yield return new WaitForSeconds(refreshRate);
        }
        refreshRunning = false;
    }

    private string AnimalBios(Animal animal)
    {
        string text = "Health: " + animal.health +
                      "\n Hunger: " + animal.hunger +
                      "\n Sleep: " + animal.sleep +
                      "\n Mode: " + animal.mode;

        return text;
    }

    private string ConstructEntityInfoText(GameObject entity)
    {
        string text = "";

        if(entity.TryGetComponent(out Animal animal))
        {
            text += AnimalBios(animal);
        }

        return text;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HexasphereGrid;

public class HexasphereBuilder : MonoBehaviour
{
    [SerializeField] GameObject seedHexasphere;
    [SerializeField] Slider divisionsSlider;

    GameObject newHex;

    private void Awake()
    {

    }

    private void Start()
    {

        seedHexasphere.SetActive(true);
    }

    public void buildNewHexasphere()
    {
        if (newHex != null)
        {
            Destroy(newHex);
        }
        seedHexasphere.SetActive(true);


        int divisions = Mathf.FloorToInt(divisionsSlider.value);
        newHex = Instantiate(seedHexasphere);

        Hexasphere hexa = newHex.GetComponent<Hexasphere>();
        hexa.numDivisions = divisions;

        seedHexasphere.SetActive(false);
        

    }

    public void deleteHexasphere()
    {
        
    }

}

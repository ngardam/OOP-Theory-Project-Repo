using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
//using System;


//using system

public class UIListener : MonoBehaviour
{

    //Class only reads ints from sliders at the moment, could be expanded

    [SerializeField] float valueMax = 200f;
    [SerializeField] float valueMin = 1f;



    [SerializeField] GameObject target;
    public Text displayText;

    private Slider thisSlider;
    private InputField thisInputField;

    //UnityEvent valueChanged = new UnityEvent();

    // private int value;

    // Start is called before the first frame update
    void Start()
    {
        thisInputField = GetComponent<InputField>();
        thisSlider = GetComponent<Slider>();
        //displayText = GetComponentInChildren<Text>();
        //int targetValue;
        if (target.TryGetComponent<Slider>(out Slider slider))
        {
           // Debug.Log("target slider found");
            int newValue = Mathf.FloorToInt(slider.value);
            UpdateValue(newValue);
            slider.onValueChanged.AddListener(ValueChanged);
        }
        else if (target.TryGetComponent<InputField>(out InputField inputField))
        {
           // Debug.Log("target input field found");
            
           // if (inputField.contentType.Equals("IntegerNumber"))
           //     {
                    //value =  inputField.textComponent.text.Parse();
                   // value = System.Convert.ToInt16(inputField.text);
                   // value = System.
                  //  UpdateValue(value);
                    inputField.onValueChanged.AddListener(ValueChanged);
            //    }
            
        }


        

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ValueChanged(float _value)
    {
        UpdateValue(Mathf.FloorToInt(_value));
    }

    void ValueChanged (string _value)
    {
        if (int.TryParse(_value, out int _intValue))
        {
            UpdateValue(_intValue);
        }
      //  UpdateValue(newValue);
    }

    void UpdateValue(int _value)
    {
        if (thisInputField != null)
        {
            thisInputField.text = "" + _value;
            //Debug.Log("Setting Text at " + _value);
        }
        if (thisSlider != null)
        {
            thisSlider.value = _value;
            //Debug.Log("Adjusting Slider");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{

    [SerializeField] float rotationPeriod = 60f; // time in seconds for object to rotate 
    [SerializeField] float direction = 1f; // should be -1 or 1
    // Start is called before the first frame update
    private float rotationStep; //how much to rotate every second
    private float step;

    private void Awake()
    {
        rotationStep = (rotationPeriod / 360f) * direction;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        step = rotationStep * Time.deltaTime;

        transform.Rotate(0f, step, 0f);


    }
}

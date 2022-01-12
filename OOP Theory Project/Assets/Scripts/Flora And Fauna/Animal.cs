using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HexasphereGrid;


public class Animal : MonoBehaviour
{

    [SerializeField] EntityInfoPanel entityInfoPanel;

    protected float speed = 1f;

    public string mode { get; private set; } = "Idle";  //what animal is doing

    private bool alive = true;

    public int health { get; private set; } = 100;

    public int hunger { get; private set; } = 65;

    public int sleep { get; private set; } = 100;

    private int maxStat = 100;

    private float metabolismRate = 1.0f; //number of seconds to lower hunger by one
    private int seekFoodThreshold = 50;

    private float tirednessRate = 2.0f; //number of seconds to lower sleep by one

    protected Vector3 destinationVector;
    protected bool isTraveling;

    private float reachedDestinationDistance = 0.1f;

    protected int searchLimit = 20; //how many steps distance animal will look for something

    public Hexasphere parentHexa;


    private void Awake()
    {
        StartCoroutine(metabolism());
        entityInfoPanel = GameObject.Find("Entity Info Panel").GetComponent<EntityInfoPanel>();
    }

    protected void Update()
    {
        if(isTraveling)
        {
            MoveTowardsDestination();
        }
    }

    private void MoveTowardsDestination()
    {
        //Vector3 direction = (destinationVector - transform.position).normalized;
        transform.LookAt(destinationVector, Vector3.back);
        // transform.

        Vector3 newPos = Vector3.MoveTowards(transform.position, destinationVector, speed * Time.deltaTime);

        transform.position = newPos;

        if (Vector3.Distance(newPos, destinationVector) <= reachedDestinationDistance)
        {
            isTraveling = false;
        }
        //transform.Translate(direction * speed * Time.deltaTime);
    }

    IEnumerator metabolism()
    {
        while (alive)
        {
            yield return new WaitForSeconds(metabolismRate);
            hunger--;

            if (hunger < seekFoodThreshold)
            {
                SeekFood();
            }
            if (hunger <= 0)
            {
                hunger = 0;
                health--;
            }
            else if (hunger >= 50 && health < maxStat)
            {
                health++;
            }
        }
    }

    protected virtual void SeekFood()
    {
        Debug.Log("This shouldn't be running");
    }

    private void OnMouseDown()
    {
        entityInfoPanel.LoadEntity(gameObject);
        Debug.Log("Entity Clicked");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HexasphereGrid;
using UnityEngine.EventSystems;


public class Animal : MonoBehaviour
{

    [SerializeField] EntityInfoPanel entityInfoPanel;

    protected float speed = 0.5f;

    public string mode { get; private set; } = "Idle";  //what animal is doing

    private bool alive = true;

    public int health { get; private set; } = 100;

    public int hunger { get; private set; } = 60;

    private int foodValue = 10;

    public int sleep { get; private set; } = 100;

    private int maxStat = 100;

    private float metabolismRate = 1.0f; //number of seconds to lower hunger by one
    private int seekFoodThreshold = 50;

    private float tirednessRate = 2.0f; //number of seconds to lower sleep by one

    //protected Vector3 destinationVector;
    protected bool isTraveling;
    public bool hasTask { get; protected set; } = false;

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

    }



    protected void MoveTowards(Vector3 destination)
    {
        transform.LookAt(destination, Vector3.back);
        Vector3 newPos = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);

        transform.position = newPos;

    }

    protected void EatFromTile(Tile tile)
    {
        if (tile.CheckForItem("Food"))
        {
            tile.RemoveItem("Food");
            hunger += foodValue;
        }
        else
        {
            Debug.Log("error eating from tile");
        }
    }

    protected bool AtLocation(Vector3 Vector3)
    {
        return (Vector3.Distance(transform.position, Vector3) <= reachedDestinationDistance);
    }

    IEnumerator metabolism()
    {
        while (alive)
        {
            yield return new WaitForSeconds(metabolismRate);
            hunger--;

            if (hunger < seekFoodThreshold && !hasTask)
            {
                SeekFood();  //turning off to track down logistics bug
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
        bool mouseOverUI = EventSystem.current.IsPointerOverGameObject();

        if (!mouseOverUI)
        {
            entityInfoPanel.LoadEntity(gameObject);
            Debug.Log("Entity Clicked");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HexasphereGrid;
using UnityEngine.EventSystems;


public class Animal : MonoBehaviour
{

    [SerializeField] EntityInfoPanel entityInfoPanel;

    protected float speed = 0.5f;

    public string mode { get;  protected set; } = "Idle";  //what animal is doing

    protected bool alive = true;

    private float decompositionTime = 10f;

    public int health { get; private set; } = 20;

    public int hunger { get; private set; } = 60;

    private int foodValue = 50;

    public int sleep { get; private set; } = 100;

    private int maxStat = 100;

    private float metabolismRate = 1f; //number of seconds to lower hunger by one
    private int seekFoodThreshold = 50;

    private float tirednessRate = 2.0f; //number of seconds to lower sleep by one

    //protected float idleBehaviorFrequency = 1.2f;

    protected float logicFrequency = 0.5f;

    //protected Vector3 destinationVector;
    protected bool isTraveling;
    //protected bool interrupt = false;
    //public bool hasTask { get; protected set; } = false;

    private float reachedDestinationDistance = 0.1f;

    protected int searchLimit = 20; //how many steps distance animal will look for something

    public Hexasphere parentHexa;


    private void Start()
    {
        StartCoroutine(metabolism());
        StartLogic();
        entityInfoPanel = GameObject.Find("Entity Info Panel").GetComponent<EntityInfoPanel>();
        parentHexa = GetComponentInParent<Hexasphere>();
    }

    public void InitializeAnimal()
    {
        parentHexa = GetComponentInParent<Hexasphere>();
    }

    protected virtual void StartLogic()
    {
        Debug.Log("Animal logic not found");
    }


    protected int MyTileIndex()
    {
        int index = parentHexa.GetTileAtPos(transform.position);
        //Debug.Log("My Tile: " + index);
        return index;
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

    protected bool isHungry()
    {
        return (hunger < seekFoodThreshold);
    }

    protected bool AtLocation(Vector3 Vector3)
    {
        return (Vector3.Distance(transform.position, Vector3) <= reachedDestinationDistance);
    }

    protected IEnumerator MoveToTile(int destinationIndex)
    {

        Vector3 destinationVector = parentHexa.GetTileCenter(destinationIndex);

        if (!AtLocation(destinationVector))
        {
            int[] pathfindingIndexes = GeneratePathfindingTileIndices(parentHexa.tiles[destinationIndex]);

            yield return StartCoroutine(FollowPathfindingIndices(pathfindingIndexes));
        }
        else
        {
            yield return null;
            //already arrived
        }

    }

    IEnumerator FollowPathfindingIndices(int[] tileIndexes)
    {
        //bool traveling = true;
        int step = 0;
        int numberOfSteps = tileIndexes.Length;

        while (step < numberOfSteps)
        {
            yield return StartCoroutine(MoveToTileCenter(tileIndexes[step]));
            step++;
        }

    }

    IEnumerator MoveToTileCenter(int tileIndex)
    {

        Vector3 destinationPosition = parentHexa.GetTileCenter(tileIndex);

        bool atDestination = AtLocation(destinationPosition);

        transform.LookAt(destinationPosition, Vector3.back); //Turn to look at current destination

        while (!atDestination)
        {
            //we read current world position, because everything will be moving. Local might be better, only need to check once.
            destinationPosition = parentHexa.GetTileCenter(tileIndex);

            //figure out new position
            Vector3 newPos = Vector3.MoveTowards(transform.position, destinationPosition, speed * Time.deltaTime);

            //move animal
            transform.position = newPos;

            //check if we have arrived
            atDestination = AtLocation(destinationPosition);

            //and do this every frame
            yield return null;
        }
    }

    protected int[] GeneratePathfindingTileIndices(Tile destinationTile)
    {
        if (MyTileIndex() == destinationTile.index)
        {
            Debug.Log("At tile already");
            return new int[1] { destinationTile.index };
        }
        else
        {


            List<int> tileIndicesList = parentHexa.FindPath(MyTileIndex(), destinationTile.index);

            int[] indexArray = new int[tileIndicesList.Count];

            for (int i = 0; i < indexArray.Length; i++)
            {
                indexArray[i] = tileIndicesList[i];
            }

            return indexArray;
        }

    }

    IEnumerator metabolism()
    {
        while (alive)
        {
            yield return new WaitForSeconds(metabolismRate);
            hunger--;

          //  if (hunger < seekFoodThreshold && !hasTask)
          //  {
          //      SeekFood();  //turning off to track down logistics bug
          //  }
            if (hunger <= 0)
            {
                hunger = 0;
                health--;
            }
            else if (hunger >= 50 && health < maxStat)  // at above 50 hunger, health will regenerate
            {
                health++;
            }

            if (health <= 0)
            {
                Die();
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

    private void Die()
    {
        Debug.Log("Ack!");
        alive = false;
        if (GetComponent<Person>() != null)
        {
            parentHexa.GetComponent<HexaspherePopulation>().RemoveFromPopulationList(gameObject);
        }
        transform.Rotate(new Vector3(-90f, 0f, 0f), Space.Self);
        StartCoroutine(Decompose());

    }

    IEnumerator Decompose()
    {
        yield return new WaitForSeconds(decompositionTime);

        Destroy(gameObject);
        
    }
}

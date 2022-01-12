using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Classes;

public class GameManager : MonoBehaviour
{
    private bool loadingFromSave = false;



    Vector3 seedVector;

    // Start is called before the first frame update
    void Start()
    {
        if (loadingFromSave)
        {
            LoadSaveData();
        }
        else
        {
            seedVector = Utilities.GenerateRandomVector3(-1000f, 1000f);
            GenerateNewGameWorld(seedVector);
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoadSaveData()
    {

    }

    private void GenerateNewGameWorld(Vector3 seedVector)
    {

        WorldGeneration worldGen = FindObjectOfType<WorldGeneration>();
        WorldGenSpecs specs = worldGen.GenerateRandomWorldGenSpecs();

        worldGen.GenerateWorld(specs);
    }


}

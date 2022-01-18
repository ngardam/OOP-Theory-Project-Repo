using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HexasphereGrid;

public class AnimalManager : MonoBehaviour
{

    private static float animalScale = 0.2f;


    public static GameObject CreateAnimal(Hexasphere hexa, int tileIndex, GameObject prefab)
    {

        GameObject newAnimal = PropPlacement.CreateProp(hexa, tileIndex, prefab);
        newAnimal.transform.localScale *= animalScale;
        newAnimal.GetComponent<Animal>().parentHexa = hexa;
        if (newAnimal.TryGetComponent<Person>(out Person person))
            {
            person.hexaLogistics = person.GetComponentInParent<HexasphereLogistics>();
        }
        return newAnimal;

    }

}

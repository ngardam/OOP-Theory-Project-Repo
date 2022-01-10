using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HexasphereGrid;

public class PropPlacement : MonoBehaviour
{
    public void PlaceProp(Hexasphere hexa, int tileIndex, GameObject prefab)
    {
        float randomRotation = Random.Range(0f, 360f);
        GameObject newProp = Instantiate(prefab);
        //newProp.transform.rotation = prefab.transform.rotation;
        hexa.ParentAndAlignToTile(newProp, tileIndex, snapRotationToVertex0: true);

        

        newProp.transform.Rotate(new Vector3(-90f, 0f, 0f), Space.Self);

        newProp.transform.Rotate(new Vector3(0f, 30f, 0f), Space.Self);

        
    }
}

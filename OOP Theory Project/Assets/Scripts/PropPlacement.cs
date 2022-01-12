using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HexasphereGrid;

public class PropPlacement : MonoBehaviour
{
    public GameObject fogOfWar;



    public static GameObject CreateProp(Hexasphere hexa, int tileIndex, GameObject prefab)
    {
        float randomRotation = Random.Range(0, 5) * 60;
        GameObject newProp = Instantiate(prefab);
        //newProp.transform.rotation = prefab.transform.rotation;
        hexa.ParentAndAlignToTile(newProp, tileIndex, snapRotationToVertex0: true);

        

        newProp.transform.Rotate(new Vector3(-90f, 0f, 0f), Space.Self);

        newProp.transform.Rotate(new Vector3(0f, 30f + randomRotation, 0f), Space.Self);

        return newProp;
    }

 //  public void PlaceFogOfWar(Hexasphere hexa)
 //  {
 //      Tile[] arrayOfTiles = hexa.tiles;
 //
 //      foreach(Tile tile in arrayOfTiles)
 //      {
 //          GameObject fogObject = Instantiate(fogOfWar);
 //          hexa.ParentAndAlignToTile(fogObject, tile.index, altitude: 1f);
 //      }
 //  }
}

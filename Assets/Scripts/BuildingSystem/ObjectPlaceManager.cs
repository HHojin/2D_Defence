using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlaceManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> placedGameObjets = new();

    public int PlaceObject(GameObject prefab, Vector3 position)
    {
        GameObject newObject = Instantiate(prefab);
        newObject.transform.position = position;
        placedGameObjets.Add(newObject);
        return placedGameObjets.Count - 1;
    }

    public void RemoveObjectAt(int gameObjectID)
    {
        if (placedGameObjets.Count <= gameObjectID || placedGameObjets[gameObjectID] == null)
            return;

        Destroy(placedGameObjets[gameObjectID]);
        placedGameObjets[gameObjectID] = null;
    }
}

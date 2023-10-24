using System.Collections.Generic;
using UnityEngine;

public class ObjectPlaceManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> placedGameObjets = new();

    //public int PlaceObject(GameObject prefab, Vector3 position)
    public int PlaceObject(ObjectData data, Vector3Int gridPosition, Vector3 position)
    {
        GameObject newObject = Instantiate(data.Prefab);
        placedGameObjets.Add(newObject);
        newObject.transform.position = position;
        newObject.GetComponent<Building>().data = data;
        newObject.GetComponent<Building>().GridPosition = gridPosition;
        newObject.GetComponent<Building>().PlacedObjectIndex = placedGameObjets.Count - 1;
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

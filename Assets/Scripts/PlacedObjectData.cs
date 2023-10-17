using System;
using System.Collections.Generic;
using UnityEngine;

public class PlacedObjectData
{
    Dictionary<Vector3Int, PlacementData> placedObjects = new();

    public void AddObject(Vector3Int gridPosition,
                          Vector2Int objectSize,
                          int ID,
                          int placedObjectID)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
        PlacementData data = new PlacementData(positionToOccupy, ID, placedObjectID);

        foreach(var pos in positionToOccupy)
        {
            if(placedObjects.ContainsKey(pos))
            {
                throw new Exception($"Position already occupied {pos}");
            }

            placedObjects[pos] = data;
        }
    }

    private List<Vector3Int> CalculatePositions(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> returnVal = new();

        Vector3Int startPos = new Vector3Int(gridPosition.x - (objectSize.x - 1) / 2,
                                             gridPosition.y - (objectSize.x - 1) / 2,
                                             gridPosition.z);

        for(int x = 0; x < objectSize.x; x++)
        {
            for(int y = 0; y < objectSize.y; y++)
            {
                returnVal.Add(new Vector3Int(startPos.x + x, startPos.y + y, gridPosition.z));
            }
        }

        return returnVal;
    }

    public bool CanPlaceObjectAt(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);

        foreach(var pos in positionToOccupy)
        {
            if(placedObjects.ContainsKey(pos))
                return false;
        }

        return true;
    }
}

public class PlacementData
{
    public List<Vector3Int> occupiedPositions;
    public int ID { get; set; }
    public int PlacedObjectID { get; private set; }

    public PlacementData(List<Vector3Int> occupiedPositions, int iD, int placedObjectID)
    {
        this.occupiedPositions = occupiedPositions;
        ID = iD;
        PlacedObjectID = placedObjectID;
    }
}
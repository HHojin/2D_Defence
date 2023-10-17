using UnityEngine;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{
    [HideInInspector] public UnityEvent<float, float> mapGenerated;
    public bool isMapGenerated = false;

    // MapGenerator.cs -> GenerateMap()
    // -> MouseController.cs -> SetCameraPos()
    public void MapGenerated(float x, float y)
    {
        mapGenerated.Invoke(x, y);
        isMapGenerated = true;
    }
}
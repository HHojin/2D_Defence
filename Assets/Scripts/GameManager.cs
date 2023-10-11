using UnityEngine;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{
    [HideInInspector] public UnityEvent<float, float> mapGenerated;

    // MapGenerator.cs -> GenerateMap()
    public void MapGenerated(float x, float y)
    {
        mapGenerated.Invoke(x, y);
    }
}
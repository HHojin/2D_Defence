using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }

            return instance;
        }
    }

    private static GameManager instance;

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    [HideInInspector]
    public UnityEvent<float, float> mapGenerated;

    // MapGenerator.cs -> GenerateMap()
    // -> TileManager.cs -> SetPolygonCollider()
    public void MapGenerated(float x, float y)
    {
        mapGenerated.Invoke(x, y);
    }
}
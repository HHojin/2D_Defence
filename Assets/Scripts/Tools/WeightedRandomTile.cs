using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public struct WeightedSprite
{
    public Sprite Sprite;
    public int Weight;
}

[CreateAssetMenu(menuName = "TileTools/Custom Tiles/Weighted Random Tile")]
public class WeightedRandomTile : Tile
{
    [SerializeField] public WeightedSprite[] Sprites;

    /// <summary>
    /// Retrieves any tile rendering data from the scripted tile.
    /// </summary>
    /// <param name="position">Position of the Tile on the Tilemap.</param>
    /// <param name="tilemap">The Tilemap the tile is present on.</param>
    /// <param name="tileData">Data to render the tile.</param>
    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);

        if (Sprites == null || Sprites.Length <= 0) return;

        var oldState = Random.state;
        long hash = position.x;
        hash = hash + 0xabcd1234 + (hash << 15);
        hash = hash + 0x0987efab ^ (hash >> 11);
        hash ^= position.y;
        hash = hash + 0x46ac12fd + (hash << 7);
        hash = hash + 0xbe9730af ^ (hash << 11);
        Random.InitState((int)hash);

        var cumulativeWeight = 0;
        foreach (var spriteInfo in Sprites) cumulativeWeight += spriteInfo.Weight;

        var randomWeight = Random.Range(0, cumulativeWeight);
        foreach (var spriteInfo in Sprites)
        {
            randomWeight -= spriteInfo.Weight;
            if (randomWeight < 0)
            {
                tileData.sprite = spriteInfo.Sprite;
                break;
            }
        }
        Random.state = oldState;
    }
}

[CustomEditor(typeof(WeightedRandomTile))]
public class WeightedRandomTileEditor : Editor
{
    private SerializedProperty m_Color;
    private SerializedProperty m_ColliderType;

    private WeightedRandomTile Tile
    {
        get { return target as WeightedRandomTile; }
    }

    public void OnEnable()
    {
        m_Color = serializedObject.FindProperty("m_Color");
        m_ColliderType = serializedObject.FindProperty("m_ColliderType");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUI.BeginChangeCheck();

        int count = EditorGUILayout.DelayedIntField("Number of Sprites", Tile.Sprites != null ? Tile.Sprites.Length : 0);
        if (count < 0)
            count = 0;

        if (Tile.Sprites == null || Tile.Sprites.Length != count)
        {
            System.Array.Resize(ref Tile.Sprites, count);
        }

        if (count == 0)
            return;

        EditorGUILayout.LabelField("Place random sprites.");
        EditorGUILayout.Space();

        for (int i = 0; i < count; i++)
        {
            Tile.Sprites[i].Sprite = (Sprite)EditorGUILayout.ObjectField("Sprite " + (i + 1), Tile.Sprites[i].Sprite, typeof(Sprite), false, null);
            Tile.Sprites[i].Weight = EditorGUILayout.IntField("Weight " + (i + 1), Tile.Sprites[i].Weight);
        }

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(m_Color);
        EditorGUILayout.PropertyField(m_ColliderType);

        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(Tile);
            serializedObject.ApplyModifiedProperties();
        }
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    //public Tilemap[] tileMap = new Tilemap[5];
    public Tilemap tileMap;
    public TileBase[] tileBase = new TileBase[5];

    [SerializeField] PolygonCollider2D polygonCollider2D;
    private MapGenerator mapGenerator;

    private void Awake()
    {
        mapGenerator = gameObject.GetComponent<MapGenerator>();
    }

    private void Start()
    {
        polygonCollider2D = GetComponentInChildren<PolygonCollider2D>();

        GameManager.Instance.mapGenerated.AddListener(SetPolygonCollider);
    }

    private void Update()
    {
        GridHeight();
    }

    //public int GetTileMapCount() { return tileMap.Length; }
    public int GetTileBaseCount() { return tileBase.Length; }

    public void ResetTileMap()
    {
        tileMap.ClearAllTiles();
        /*
        foreach (var tileMaps in tileMap)
        {
            tileMaps.ClearAllTiles();
        }
        */
    }

    public void DrawTile(int x, int y, int num)
    {
        tileMap.SetTile(new Vector3Int(x, y, 0), tileBase[num]);

        /*
        for(int i = num; i >= 0; i--)
        {
            tileMap[i].SetTile(new Vector3Int(x, y, 0), tileBase[i]);
        }
        */
    }

    private void SetPolygonCollider(float x, float y)
    {
        polygonCollider2D.points = new[] {new Vector2(x + 5, y + 5), new Vector2(x + 5, -5),
                                        new Vector2(-5, -5), new Vector2(-5, y + 5) };
        polygonCollider2D.SetPath(0, polygonCollider2D.points);
    }

    //tmp
    public TMP_Text terrainHeight;

    private void GridHeight()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (mouseWorldPos.x >= 0 && mouseWorldPos.y >= 0
                && mouseWorldPos.x <= tileMap.size.x && mouseWorldPos.y <= tileMap.size.y)
            {
                var tmp = tileMap.WorldToCell(mouseWorldPos);
                terrainHeight.text = "[" + tmp.x + "," + tmp.y + "]"
                                    + mapGenerator.grid.GetGridArray(tmp.x, tmp.y);
            }
        }
    }
}
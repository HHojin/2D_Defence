using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "TileTools/Custom Tiles/Advanced Rule Tile")]
public class MapCustomRuleTile : RuleTile<MapCustomRuleTile.Neighbor> {
    public TileBase[] tilesToConnect;

    public class Neighbor : RuleTile.TilingRule.Neighbor {
        //public const int Any = 3;
        //public const int Null = 4;
        //public const int NotNull = 5;
    }

    public override bool RuleMatch(int neighbor, TileBase tile) {
        switch (neighbor) {
            case Neighbor.This: return Check_This(tile);
            case Neighbor.NotThis: return Check_NotThis(tile);
            //case Neighbor.Any: return Check_Any(tile);
            //case Neighbor.Null: return tile == null;
            //case Neighbor.NotNull: return tile != null;
        }
        return base.RuleMatch(neighbor, tile);
    }

    bool Check_This(TileBase tile)
    {
        return tilesToConnect.Contains(tile) || tile == this;
    }

    bool Check_NotThis(TileBase tile)
    {
        return !tilesToConnect.Contains(tile) && tile != this;
    }
}
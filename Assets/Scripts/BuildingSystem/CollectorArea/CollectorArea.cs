using UnityEngine;

public class CollectorArea : MonoBehaviour
{
    private bool isInsideCircle(Vector3 center, Vector3 tile, float radius)
    {
        float distX = center.x - tile.x;
        float distY = center.y - tile.y;
        float distSquared = distX * distX + distY * distY;

        return distSquared <= radius * radius;
    }
}

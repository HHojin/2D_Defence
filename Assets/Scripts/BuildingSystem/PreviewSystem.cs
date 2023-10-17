using UnityEngine;

public class PreviewSystem : MonoBehaviour
{
    [SerializeField] private GameObject cellIndicator;
    private GameObject previewObject;

    private Color previewColor;

    private void Start()
    {
        cellIndicator.SetActive(false);
    }

    public void StartShowPlcaementPreview(GameObject prefab, Vector2Int size)
    {
        previewObject = Instantiate(prefab);
        PreparePreview(previewObject);
        PrepareCursor(size);
        cellIndicator.SetActive(true);
    }

    private void PrepareCursor(Vector2Int size)
    {
        if(size.x > 0 || size.y > 0)
        {
            cellIndicator.transform.localScale = new Vector3(size.x, size.y, 1);
        }
    }

    private void PreparePreview(GameObject previewObject)
    {
        previewColor = new Color(1f, 1f, 1f, 0.5f);
        previewObject.GetComponent<SpriteRenderer>().color = previewColor;
        previewObject.GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("Preview");
    }

    public void StopShowPreview()
    {
        cellIndicator.SetActive(false);
        Destroy(previewObject);
    }

    public void UpdatePosition(Vector3 position, bool validity)
    {
        MovePreview(position);
        MoveCursor(position);
        ApplyFeedBack(validity);
    }

    private void ApplyFeedBack(bool validity)
    {
        Color c = validity ? Color.green : Color.red;
        c.a = 0.5f;

        cellIndicator.GetComponent<SpriteRenderer>().color = c;
    }

    private void MoveCursor(Vector3 position)
    {
        cellIndicator.transform.position = position;
    }

    private void MovePreview(Vector3 position)
    {
        previewObject.transform.position = position;
    }
}

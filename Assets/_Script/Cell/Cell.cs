using UnityEngine;

public class Cell : MonoBehaviour
{
    public Vector2Int GridPosition { get; private set; }
    public CellType Type { get; private set; }

    public void Initialize(
        Vector2Int position,
        CellType type)
    {
        GridPosition = position;
        Type = type;

        SnapToGrid();
    }

    public void InitializeAt(
        Vector2Int position,
        CellType type,
        Vector3 worldPosition)
    {
        GridPosition = position;
        Type = type;

        transform.position = worldPosition;
    }

    public void SetGridPosition(Vector2Int position)
    {
        GridPosition = position;
    }

    public void SnapToGrid()
    {
        transform.position = new Vector3(
            GridPosition.x,
            GridPosition.y,
            0f
        );
    }
}
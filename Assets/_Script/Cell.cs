using UnityEngine;

public class Cell : MonoBehaviour
{
    public Vector2Int GridPosition { get; private set; }
    public CellType Type { get; private set; }

    public void Initialize(Vector2Int position, CellType type)
    {
        GridPosition = position;
        Type = type;

        transform.position = new Vector3(
            position.x,
            position.y,
            0f
        );
    }

    public void SetGridPosition(Vector2Int position)
    {
        GridPosition = position;

        transform.position = new Vector3(
            position.x,
            position.y,
            0f
        );
    }

    public void SetType(CellType type)
    {
        Type = type;
    }
}
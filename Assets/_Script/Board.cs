using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private int width = 8;
    [SerializeField] private int height = 8;

    private CellData[,] cells;

    public int Width => width;
    public int Height => height;

    private void Awake()
    {
        cells = new CellData[width, height];
    }

    public void Initialize()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                cells[x, y] = new CellData(GetRandomType());
            }
        }
    }

    private CellType GetRandomType()
    {
        // 1 to 4 because 0 = Empty
        return (CellType)Random.Range(1, 5);
    }

    public CellData GetCell(int x, int y)
    {
        return cells[x, y];
    }

    public void SetCell(int x, int y, CellData data)
    {
        cells[x, y] = data;
    }

    public void SetCell(int x, int y, CellType type)
    {
        cells[x, y] = new CellData(type);
    }

    public bool IsInside(int x, int y)
    {
        return x >= 0 &&
               x < width &&
               y >= 0 &&
               y < height;
    }
    public bool TrySwap(Vector2Int position, Vector2Int direction)
    {
        Vector2Int target = position + direction;

        if (!IsInside(target.x, target.y))
            return false;

        CellData first = cells[position.x, position.y];
        CellData second = cells[target.x, target.y];

        cells[position.x, position.y] = second;
        cells[target.x, target.y] = first;

        return true;
    }
}
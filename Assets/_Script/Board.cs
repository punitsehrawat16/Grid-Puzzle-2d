using System.Collections.Generic;
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
                cells[x, y] = new CellData(GetRandomType(x, y));
            }
        }
    }

    private CellType GetRandomType(int x, int y)
    {
        List<CellType> types = new List<CellType>
        {
            CellType.Orange,
            CellType.Green,
            CellType.Yellow,
            CellType.Blue
        };

        if (x >= 2 &&
            cells[x - 1, y].Type == cells[x - 2, y].Type)
        {
            types.Remove(cells[x - 1, y].Type);
        }

        if (y >= 2 &&
            cells[x, y - 1].Type == cells[x, y - 2].Type)
        {
            types.Remove(cells[x, y - 1].Type);
        }

        return types[Random.Range(0, types.Count)];
    }

    public CellData GetCell(int x, int y)
    {
        return cells[x, y];
    }

    public bool IsInside(int x, int y)
    {
        return x >= 0 && x < width &&
               y >= 0 && y < height;
    }

    public bool TrySwap(Vector2Int first, Vector2Int direction)
    {
        Vector2Int second = first + direction;

        if (!IsInside(second.x, second.y))
            return false;

        var temp = cells[first.x, first.y];
        cells[first.x, first.y] = cells[second.x, second.y];
        cells[second.x, second.y] = temp;

        return true;
    }

    public void ClearMatches(List<Vector2Int> matches)
    {
        foreach (Vector2Int position in matches)
        {
            cells[position.x, position.y] =
                new CellData(CellType.Empty);
        }
    }

    public CollapseResult CollapseAndFill()
    {
        CellData[,] newCells = new CellData[width, height];

        List<CellMove> moves = new List<CellMove>();
        List<CellSpawn> spawns = new List<CellSpawn>();

        for (int x = 0; x < width; x++)
        {
            int newY = 0;

            // Move existing cells down
            for (int y = 0; y < height; y++)
            {
                if (cells[x, y].Type == CellType.Empty)
                    continue;

                newCells[x, newY] = cells[x, y];

                if (y != newY)
                {
                    moves.Add(
                        new CellMove(
                            new Vector2Int(x, y),
                            new Vector2Int(x, newY)
                        )
                    );
                }

                newY++;
            }

            // Create new cells at the top
            int spawnOrder = 1;

            while (newY < height)
            {
                CellType type = GetRandomType();

                newCells[x, newY] =
                    new CellData(type);

                spawns.Add(
                    new CellSpawn(
                        new Vector2Int(x, newY),
                        type,
                        spawnOrder
                    )
                );

                newY++;
                spawnOrder++;
            }
        }

        cells = newCells;

        return new CollapseResult(moves, spawns);
    }

    private CellType GetRandomType()
    {
        return (CellType)Random.Range(1, 5);
    }
}


public struct CellMove
{
    public Vector2Int From;
    public Vector2Int To;

    public CellMove(Vector2Int from, Vector2Int to)
    {
        From = from;
        To = to;
    }
}


public struct CellSpawn
{
    public Vector2Int Position;
    public CellType Type;
    public int SpawnOrder;

    public CellSpawn(
        Vector2Int position,
        CellType type,
        int spawnOrder)
    {
        Position = position;
        Type = type;
        SpawnOrder = spawnOrder;
    }
}


public class CollapseResult
{
    public List<CellMove> Moves;
    public List<CellSpawn> Spawns;

    public CollapseResult(
        List<CellMove> moves,
        List<CellSpawn> spawns)
    {
        Moves = moves;
        Spawns = spawns;
    }
}
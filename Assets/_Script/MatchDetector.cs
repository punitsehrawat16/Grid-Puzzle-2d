using System.Collections.Generic;
using UnityEngine;

public class MatchDetector
{
    private readonly Board board;

    public MatchDetector(Board board)
    {
        this.board = board;
    }

    public List<Vector2Int> FindMatches()
    {
        HashSet<Vector2Int> matches =
            new HashSet<Vector2Int>();

        for (int x = 0; x < board.Width; x++)
        {
            for (int y = 0; y < board.Height; y++)
            {
                CellType type = board.GetCell(x, y).Type;

                if (type == CellType.Empty)
                    continue;

                // Horizontal
                if (x + 2 < board.Width &&
                    board.GetCell(x + 1, y).Type == type &&
                    board.GetCell(x + 2, y).Type == type)
                {
                    matches.Add(new Vector2Int(x, y));
                    matches.Add(new Vector2Int(x + 1, y));
                    matches.Add(new Vector2Int(x + 2, y));
                }

                // Vertical
                if (y + 2 < board.Height &&
                    board.GetCell(x, y + 1).Type == type &&
                    board.GetCell(x, y + 2).Type == type)
                {
                    matches.Add(new Vector2Int(x, y));
                    matches.Add(new Vector2Int(x, y + 1));
                    matches.Add(new Vector2Int(x, y + 2));
                }
            }
        }

        return new List<Vector2Int>(matches);
    }
}
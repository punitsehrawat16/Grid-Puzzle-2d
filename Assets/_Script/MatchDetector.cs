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
        HashSet<Vector2Int> matches = new HashSet<Vector2Int>();

        FindHorizontalMatches(matches);
        FindVerticalMatches(matches);

        return new List<Vector2Int>(matches);
    }

    private void FindHorizontalMatches(HashSet<Vector2Int> matches)
    {
        for (int y = 0; y < board.Height; y++)
        {
            int x = 0;

            while (x < board.Width)
            {
                CellType type = board.GetCell(x, y).Type;

                if (type == CellType.Empty)
                {
                    x++;
                    continue;
                }

                int startX = x;

                while (x < board.Width &&
                       board.GetCell(x, y).Type == type)
                {
                    x++;
                }

                int length = x - startX;

                if (length >= 3)
                {
                    for (int matchX = startX; matchX < x; matchX++)
                    {
                        matches.Add(
                            new Vector2Int(matchX, y)
                        );
                    }
                }
            }
        }
    }

    private void FindVerticalMatches(HashSet<Vector2Int> matches)
    {
        for (int x = 0; x < board.Width; x++)
        {
            int y = 0;

            while (y < board.Height)
            {
                CellType type = board.GetCell(x, y).Type;

                if (type == CellType.Empty)
                {
                    y++;
                    continue;
                }

                int startY = y;

                while (y < board.Height &&
                       board.GetCell(x, y).Type == type)
                {
                    y++;
                }

                int length = y - startY;

                if (length >= 3)
                {
                    for (int matchY = startY; matchY < y; matchY++)
                    {
                        matches.Add(
                            new Vector2Int(x, matchY)
                        );
                    }
                }
            }
        }
    }
}
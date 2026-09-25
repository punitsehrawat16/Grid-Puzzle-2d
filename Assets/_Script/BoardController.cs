using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    [SerializeField] private Board board;
    [SerializeField] private BoardRenderer boardRenderer;
    [SerializeField] private InputHandler inputHandler;

    private MatchDetector matchDetector;

    private void Start()
    {
        board.Initialize();

        matchDetector = new MatchDetector(board);

        boardRenderer.Render(board);

        CheckAndRemoveMatches();
    }

    private void Update()
    {
        if (!inputHandler.CheckInput())
            return;

        Cell cell = inputHandler.CurrentCell;

        if (cell == null)
            return;

        Vector2Int direction = Vector2Int.RoundToInt(
            inputHandler.Direction
        );

        bool swapped = board.TrySwap(
            cell.GridPosition,
            direction
        );

        if (swapped)
        {
            boardRenderer.Render(board);

            Debug.Log("Swap successful!");
        }
    }

    private void CheckAndRemoveMatches()
    {
        List<Vector2Int> matches =
            matchDetector.FindMatches();

        if (matches.Count == 0)
        {
            Debug.Log("No matches found.");
            return;
        }

        Debug.Log($"Matches found: {matches.Count}");

        RemoveMatches(matches);

        boardRenderer.Render(board);

        RefillBoard();

        boardRenderer.Render(board);
    }

    private void RemoveMatches(List<Vector2Int> matches)
    {
        foreach (Vector2Int position in matches)
        {
            board.SetCell(
                position.x,
                position.y,
                CellType.Empty
            );
        }
    }

    private void RefillBoard()
    {
        for (int x = 0; x < board.Width; x++)
        {
            for (int y = 0; y < board.Height; y++)
            {
                if (board.GetCell(x, y).Type == CellType.Empty)
                {
                    CellType newType =
                        (CellType)Random.Range(1, 5);

                    board.SetCell(
                        x,
                        y,
                        newType
                    );
                }
            }
        }
    }
}
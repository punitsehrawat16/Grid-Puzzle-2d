using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    [Header("Systems")]
    [SerializeField] private Board board;
    [SerializeField] private BoardRenderer boardRenderer;
    [SerializeField] private CellPool cellPool;
    [SerializeField] private InputHandler inputHandler;
    [SerializeField] private ScoreManager scoreManager;

    [Header("Animation")]
    [SerializeField] private float swapDuration = 0.2f;

    private MatchDetector matchDetector;

    private bool isBusy;

    private void Start()
    {
        // 8x8 = 64 cells.
        // Pre-create 64 of every color.
        cellPool.Initialize(
            board.Width * board.Height
        );

        board.Initialize();

        matchDetector =
            new MatchDetector(board);

        boardRenderer.Initialize(board);
    }

    private void Update()
    {
        if (isBusy)
            return;

        if (!inputHandler.CheckInput())
            return;

        Cell selectedCell =
            inputHandler.CurrentCell;

        if (selectedCell == null)
            return;

        Vector2Int direction =
            Vector2Int.RoundToInt(
                inputHandler.Direction
            );

        StartCoroutine(
            TryMove(
                selectedCell,
                direction
            )
        );
    }

    private IEnumerator TryMove(
        Cell selectedCell,
        Vector2Int direction)
    {
        isBusy = true;

        Vector2Int firstPosition =
            selectedCell.GridPosition;

        Vector2Int secondPosition =
            firstPosition + direction;

        if (!board.IsInside(
                secondPosition.x,
                secondPosition.y))
        {
            isBusy = false;
            yield break;
        }

        Cell secondCell =
            boardRenderer.GetVisual(
                secondPosition
            );

        if (secondCell == null)
        {
            isBusy = false;
            yield break;
        }

        // Change logical board.
        board.TrySwap(
            firstPosition,
            direction
        );

        // Check whether swap creates a match.
        List<Vector2Int> matches =
            matchDetector.FindMatches();

        bool successfulMove =
            matches.Count > 0;

        // Animate the swap.
        yield return StartCoroutine(
            boardRenderer.AnimateSwap(
                selectedCell,
                secondCell,
                firstPosition,
                secondPosition,
                swapDuration
            )
        );

        // Update visual grid references.
        boardRenderer.SwapVisualReferences(
            firstPosition,
            secondPosition
        );

        if (!successfulMove)
        {
            // Undo logical swap.
            board.TrySwap(
                secondPosition,
                -direction
            );

            // Animate candies back.
            yield return StartCoroutine(
                boardRenderer.AnimateSwap(
                    selectedCell,
                    secondCell,
                    secondPosition,
                    firstPosition,
                    swapDuration
                )
            );

            boardRenderer.SwapVisualReferences(
                firstPosition,
                secondPosition
            );

            isBusy = false;

            yield break;
        }

        // Successful match.
        scoreManager.AddScore(120);

        yield return StartCoroutine(
            ResolveMatches()
        );

        isBusy = false;
    }

    private IEnumerator ResolveMatches()
    {
        while (true)
        {
            List<Vector2Int> matches =
                matchDetector.FindMatches();

            if (matches.Count == 0)
                break;

            // Disable and return matched candies.
            boardRenderer.RemoveMatches(
                matches
            );

            // Remove from logical board.
            board.ClearMatches(
                matches
            );

            yield return null;

            // Gravity + new candies.
            CollapseResult result =
                board.CollapseAndFill();

            yield return StartCoroutine(
                boardRenderer.AnimateCollapse(
                    result,
                    board.Height
                )
            );
        }

        // Check if the player has any possible move.
        if (!HasPossibleMove())
        {
            Debug.Log("No possible moves!");
            // Need to add shuffle part
        }
    }

    private bool HasPossibleMove()
    {
        for (int x = 0; x < board.Width; x++)
        {
            for (int y = 0; y < board.Height; y++)
            {
                Vector2Int position =
                    new Vector2Int(x, y);

                // Try swapping with the right cell.
                if (x < board.Width - 1)
                {
                    board.TrySwap(
                        position,
                        Vector2Int.right
                    );

                    bool hasMatch =
                        matchDetector.FindMatches().Count > 0;

                    // Swap back.
                    board.TrySwap(
                        position + Vector2Int.right,
                        Vector2Int.left
                    );

                    if (hasMatch)
                        return true;
                }

                // Try swapping with the cell above.
                if (y < board.Height - 1)
                {
                    board.TrySwap(
                        position,
                        Vector2Int.up
                    );

                    bool hasMatch =
                        matchDetector.FindMatches().Count > 0;

                    // Swap back.
                    board.TrySwap(
                        position + Vector2Int.up,
                        Vector2Int.down
                    );

                    if (hasMatch)
                        return true;
                }
            }
        }

        return false;
    }
}
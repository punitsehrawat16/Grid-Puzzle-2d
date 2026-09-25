using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardRenderer : MonoBehaviour
{
    [SerializeField] private CellPool cellPool;

    private Cell[,] visuals;

    private class AnimationData
    {
        public Cell Cell;
        public Vector3 Start;
        public Vector3 Target;
        public float Duration;
    }

    public void Initialize(Board board)
    {
        visuals =
            new Cell[board.Width, board.Height];

        for (int x = 0; x < board.Width; x++)
        {
            for (int y = 0; y < board.Height; y++)
            {
                CellType type =
                    board.GetCell(x, y).Type;

                Cell cell =
                    cellPool.Get(type);

                cell.transform.SetParent(transform);

                cell.Initialize(
                    new Vector2Int(x, y),
                    type
                );

                visuals[x, y] = cell;
            }
        }
    }

    public Cell GetVisual(Vector2Int position)
    {
        return visuals[
            position.x,
            position.y
        ];
    }

    public void SwapVisualReferences(
        Vector2Int first,
        Vector2Int second)
    {
        Cell temp =
            visuals[first.x, first.y];

        visuals[first.x, first.y] =
            visuals[second.x, second.y];

        visuals[second.x, second.y] =
            temp;

        if (visuals[first.x, first.y] != null)
        {
            visuals[first.x, first.y]
                .SetGridPosition(first);
        }

        if (visuals[second.x, second.y] != null)
        {
            visuals[second.x, second.y]
                .SetGridPosition(second);
        }
    }

    public void RemoveMatches(
        List<Vector2Int> matches)
    {
        foreach (Vector2Int position in matches)
        {
            Cell cell =
                visuals[position.x, position.y];

            if (cell == null)
                continue;

            visuals[position.x, position.y] = null;

            cellPool.Release(cell);
        }
    }

    public IEnumerator AnimateSwap(
        Cell first,
        Cell second,
        Vector2Int firstPosition,
        Vector2Int secondPosition,
        float duration)
    {
        Vector3 firstStart =
            GridToWorld(firstPosition);

        Vector3 secondStart =
            GridToWorld(secondPosition);

        Vector3 firstTarget =
            GridToWorld(secondPosition);

        Vector3 secondTarget =
            GridToWorld(firstPosition);

        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;

            float t =
                Mathf.Clamp01(time / duration);

            t = Mathf.SmoothStep(0f, 1f, t);

            first.transform.position =
                Vector3.Lerp(
                    firstStart,
                    firstTarget,
                    t
                );

            second.transform.position =
                Vector3.Lerp(
                    secondStart,
                    secondTarget,
                    t
                );

            yield return null;
        }

        first.transform.position = firstTarget;
        second.transform.position = secondTarget;
    }

    public IEnumerator AnimateCollapse(
        CollapseResult result,
        int boardHeight)
    {
        List<AnimationData> animations =
            new List<AnimationData>();

        // Existing candies falling
        foreach (CellMove move in result.Moves)
        {
            Cell cell =
                visuals[
                    move.From.x,
                    move.From.y
                ];

            if (cell == null)
                continue;

            visuals[
                move.From.x,
                move.From.y
            ] = null;

            visuals[
                move.To.x,
                move.To.y
            ] = cell;

            Vector3 start =
                cell.transform.position;

            Vector3 target =
                GridToWorld(move.To);

            cell.SetGridPosition(move.To);

            animations.Add(
                new AnimationData
                {
                    Cell = cell,
                    Start = start,
                    Target = target,
                    Duration =
                        0.08f *
                        Mathf.Max(
                            1,
                            Vector2Int.Distance(
                                move.From,
                                move.To
                            )
                        )
                }
            );
        }

        // New candies coming from above
        foreach (CellSpawn spawn in result.Spawns)
        {
            Cell cell =
                cellPool.Get(spawn.Type);

            cell.transform.SetParent(transform);

            Vector3 start =
                new Vector3(
                    spawn.Position.x,
                    boardHeight + spawn.SpawnOrder,
                    0f
                );

            Vector3 target =
                GridToWorld(spawn.Position);

            cell.InitializeAt(
                spawn.Position,
                spawn.Type,
                start
            );

            visuals[
                spawn.Position.x,
                spawn.Position.y
            ] = cell;

            animations.Add(
                new AnimationData
                {
                    Cell = cell,
                    Start = start,
                    Target = target,
                    Duration =
                        0.08f *
                        Mathf.Max(
                            1,
                            spawn.SpawnOrder
                        )
                }
            );
        }

        yield return AnimateAll(animations);
    }

    private IEnumerator AnimateAll(
        List<AnimationData> animations)
    {
        float elapsed = 0f;

        float maxDuration = 0f;

        foreach (AnimationData animation in animations)
        {
            maxDuration =
                Mathf.Max(
                    maxDuration,
                    animation.Duration
                );
        }

        while (elapsed < maxDuration)
        {
            elapsed += Time.deltaTime;

            foreach (AnimationData animation
                     in animations)
            {
                float t =
                    Mathf.Clamp01(
                        elapsed /
                        animation.Duration
                    );

                t = Mathf.SmoothStep(0f, 1f, t);

                animation.Cell.transform.position =
                    Vector3.Lerp(
                        animation.Start,
                        animation.Target,
                        t
                    );
            }

            yield return null;
        }

        foreach (AnimationData animation in animations)
        {
            animation.Cell.transform.position =
                animation.Target;
        }
    }

    private Vector3 GridToWorld(
        Vector2Int position)
    {
        return new Vector3(
            position.x,
            position.y,
            0f
        );
    }
}
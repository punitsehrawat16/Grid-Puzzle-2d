using System.Collections.Generic;
using UnityEngine;

public class CellPool : MonoBehaviour
{
    [SerializeField] private GameObject[] cellPrefabs;
    [SerializeField] private Transform poolParent;

    private Dictionary<CellType, Queue<Cell>>
        pools = new Dictionary<CellType, Queue<Cell>>();

    public void Initialize(int amountPerType)
    {
        for (int i = 1; i <= 4; i++)
        {
            CellType type = (CellType)i;

            Queue<Cell> queue =
                new Queue<Cell>();

            pools[type] = queue;

            for (int j = 0; j < amountPerType; j++)
            {
                Cell cell = CreateCell(type);

                cell.gameObject.SetActive(false);

                queue.Enqueue(cell);
            }
        }
    }

    private Cell CreateCell(CellType type)
    {
        int index = (int)type - 1;

        GameObject obj = Instantiate(
            cellPrefabs[index],
            poolParent
        );

        Cell cell =
            obj.GetComponent<Cell>();

        if (cell == null)
        {
            Debug.LogError(
                $"{obj.name} needs a Cell component."
            );
        }

        return cell;
    }

    public Cell Get(CellType type)
    {
        if (!pools.ContainsKey(type))
        {
            Debug.LogError(
                $"No pool exists for {type}"
            );

            return null;
        }

        Queue<Cell> queue = pools[type];

        if (queue.Count == 0)
        {
            Debug.LogError(
                $"Pool exhausted for {type}"
            );

            return null;
        }

        Cell cell = queue.Dequeue();

        cell.gameObject.SetActive(true);

        return cell;
    }

    public void Release(Cell cell)
    {
        if (cell == null)
            return;

        CellType type = cell.Type;

        cell.gameObject.SetActive(false);

        cell.transform.SetParent(poolParent);

        pools[type].Enqueue(cell);
    }
}
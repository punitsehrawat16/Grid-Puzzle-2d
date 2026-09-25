using UnityEngine;

public class BoardRenderer : MonoBehaviour
{
    [SerializeField] private GameObject[] cellPrefabs;

    public void Render(Board board)
    {
        ClearBoard();

        for (int x = 0; x < board.Width; x++)
        {
            for (int y = 0; y < board.Height; y++)
            {
                CellData data = board.GetCell(x, y);

                if (data.Type == CellType.Empty)
                    continue;

                CreateCell(x, y, data.Type);
            }
        }
    }

    private void CreateCell(int x, int y, CellType type)
    {
        int prefabIndex = (int)type - 1;

        if (prefabIndex < 0 || prefabIndex >= cellPrefabs.Length)
        {
            Debug.LogError($"No prefab assigned for {type}");
            return;
        }

        Vector3 position = new Vector3(x, y, 0f);

        GameObject obj = Instantiate(
            cellPrefabs[prefabIndex],
            position,
            Quaternion.identity,
            transform
        );

        Cell cell = obj.GetComponent<Cell>();

        if (cell == null)
        {
            Debug.LogError(
                $"{obj.name} does not contain a Cell component."
            );

            return;
        }

        cell.Initialize(
            new Vector2Int(x, y),
            type
        );
    }

    private void ClearBoard()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}
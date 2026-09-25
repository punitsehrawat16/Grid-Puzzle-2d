using UnityEngine;

public class CameraAdjustment : MonoBehaviour
{
    [SerializeField] private int width = 8;
    [SerializeField] private int height = 8;
    [SerializeField] private float cellSize = 1f;
    [SerializeField] private float padding = 1f;

    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();

        AdjustCamera();
    }

    public void AdjustCamera()
    {
        // Center the board
        float boardWidth = width * cellSize;
        float boardHeight = height * cellSize;

        transform.position = new Vector3(
            boardWidth / 2f - cellSize / 2f,
            boardHeight / 2f - cellSize / 2f,
            transform.position.z
        );

        // Calculate required camera size
        float verticalSize = boardHeight / 2f;
        float horizontalSize = boardWidth / (2f * cam.aspect);

        cam.orthographicSize = Mathf.Max(verticalSize, horizontalSize) + padding;
    }
}
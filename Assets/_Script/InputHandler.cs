using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private InputAction mouseClick;
    [SerializeField] private float minimumSwipeDistance = 0.25f;

    private Camera mainCamera;
    private Vector2 startWorldPosition;

    public Cell CurrentCell { get; private set; }
    public Vector2 Direction { get; private set; }

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        mouseClick.Enable();
    }

    private void OnDisable()
    {
        mouseClick.Disable();
    }

    public bool CheckInput()
    {
        if (mouseClick.WasPressedThisFrame())
        {
            StartSwipe();
        }

        if (mouseClick.WasReleasedThisFrame())
        {
            return EndSwipe();
        }

        return false;
    }

    private void StartSwipe()
    {
        Vector2 screenPosition =
            Mouse.current.position.ReadValue();

        Vector2 worldPosition =
            mainCamera.ScreenToWorldPoint(screenPosition);

        Collider2D hit =
            Physics2D.OverlapPoint(worldPosition);

        if (hit == null)
        {
            CurrentCell = null;
            return;
        }

        CurrentCell = hit.GetComponent<Cell>();

        if (CurrentCell == null)
            return;

        startWorldPosition = worldPosition;
    }

    private bool EndSwipe()
    {
        if (CurrentCell == null)
            return false;

        Vector2 screenPosition =
            Mouse.current.position.ReadValue();

        Vector2 endWorldPosition =
            mainCamera.ScreenToWorldPoint(screenPosition);

        Vector2 swipe =
            endWorldPosition - startWorldPosition;

        if (swipe.magnitude < minimumSwipeDistance)
        {
            CurrentCell = null;
            return false;
        }

        Direction = GetCardinalDirection(swipe);

        return true;
    }

    private Vector2 GetCardinalDirection(Vector2 swipe)
    {
        if (Mathf.Abs(swipe.x) > Mathf.Abs(swipe.y))
        {
            return swipe.x > 0
                ? Vector2.right
                : Vector2.left;
        }

        return swipe.y > 0
            ? Vector2.up
            : Vector2.down;
    }
}
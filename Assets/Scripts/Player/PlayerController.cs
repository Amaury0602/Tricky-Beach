using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerController : MonoBehaviour
{
    private PlayerMovement playerMovement;

    //touch controls
    [SerializeField] private float minSwipeDelta = 100f;
    private Vector2 firstTouchPosition;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        RegisterMovementInputs();
    }

    private void RegisterMovementInputs()
    {
        Orientation orientation = playerMovement.orientation;
        if (orientation == Orientation.Vertical)
        {
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                playerMovement.MoveToNextCell(orientation, Input.GetAxisRaw("Horizontal"));
            }

            if (Mathf.Abs(SwipeDirection().x) > minSwipeDelta)
            {
                float dir = SwipeDirection().x > 0 ? 1 : -1;
                playerMovement.MoveToNextCell(orientation, dir);
            }
        }
        else
        {
            if (Input.GetAxisRaw("Vertical") != 0)
            {
                playerMovement.MoveToNextCell(orientation, Input.GetAxisRaw("Vertical"));
            }

            if (Mathf.Abs(SwipeDirection().y) > minSwipeDelta)
            {
                float dir = SwipeDirection().y > 0 ? 1 : -1;
                playerMovement.MoveToNextCell(orientation, dir);
            }
        }
    }

    private Vector2 SwipeDirection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            firstTouchPosition = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            Vector2 touchDelta = (Vector2)Input.mousePosition - firstTouchPosition;
            return touchDelta;
        }
        else
        {
            return Vector2.zero;
        }
    }
}

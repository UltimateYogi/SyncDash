using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 10f;
    public float speed = 5f;
    public bool isOnGround = true;

    private Rigidbody playerRb;
    private bool gameOver = false;

    private int desiredLane = 1;      // -1: left, 0: middle, 1: right
    public float laneDistance = 2.5f;  // The distance between two lanes
    public float laneChangeSpeed = 10f; // Speed to move towards target lane
    private Vector2 touchStartPos;
    private bool isSwiping = false;
    public float swipeThreshold = 50f; // Minimum swipe distance in pixels

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        
    }

    void Update()
    {
        if(!GameManager.instance.isGamerunning)
            return;

        transform.Translate(Vector3.forward * GameManager.instance.gameSpeed * Time.deltaTime);
        

        if (Input.GetKeyDown(KeyCode.Space) && isOnGround && !gameOver)
        {
            Jump();
        }
        else if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            // If a tap without significant swipe distance, consider it a jump.
            if (touch.phase == TouchPhase.Ended && !isSwiping && isOnGround && !gameOver)
            {
                Jump();
            }
        }

        // Handle lane change input for PC (A/D keys)
        if (Input.GetKeyDown(KeyCode.A))
        {
            ChangeLane(-1);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            ChangeLane(1);
        }

        // Handle mobile swipe input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            HandleSwipe(touch);
        }

        // Smoothly move to target lane (modify only the x-coordinate)
        Vector3 targetPosition = transform.position;
        float targetX = (desiredLane - 1) * laneDistance;
        targetPosition.x = Mathf.Lerp(transform.position.x, targetX, laneChangeSpeed * Time.deltaTime);
        transform.position = targetPosition;

        // Record current state for ghost syncing (lane change is reflected in position)
        PlayerState currentState = new PlayerState(transform.position, false);
        GhostController.Instance.ReceiveState(currentState);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            gameOver = true;
            Debug.Log("Game Over!");
             GameManager.instance.GameOver();
        }
    }
    void Jump()
    {
        playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isOnGround = false;
        // Record jump state for ghost sync
        PlayerState currentState = new PlayerState(transform.position, true);
        GhostController.Instance.ReceiveState(currentState);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Gems"))
        {
            AudioManager.instance.PlaySound("PickUp");
            GameManager.instance.AddScore(10);
             other.gameObject.SetActive(false);
        }
    }
    void ChangeLane(int direction)
    {
        // direction: -1 for left, +1 for right
        desiredLane = Mathf.Clamp(desiredLane + direction, 0, 2);
    }

    void HandleSwipe(Touch touch)
    {
        switch (touch.phase)
        {
            case TouchPhase.Began:
                isSwiping = false;
                touchStartPos = touch.position;
                break;
            case TouchPhase.Moved:
                // Check if the swipe distance is significant
                if (Mathf.Abs(touch.position.x - touchStartPos.x) > swipeThreshold)
                {
                    isSwiping = true;
                    // Determine swipe direction
                    if (touch.position.x - touchStartPos.x > 0)
                    {
                        ChangeLane(1);
                    }
                    else
                    {
                        ChangeLane(-1);
                    }
                    // Reset start position to avoid multiple lane changes in one swipe
                    touchStartPos = touch.position;
                }
                break;
            case TouchPhase.Ended:
                isSwiping = false;
                break;
        }
    }
}


[System.Serializable]
public struct PlayerState
{
    public Vector3 position;
    public bool jumped;

    public PlayerState(Vector3 pos, bool jumped)
    {
        position = pos;
        this.jumped = jumped;
    }
}

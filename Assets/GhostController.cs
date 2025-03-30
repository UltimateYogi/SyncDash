using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    public static GhostController Instance;

    [Header("Ghost Settings")]
    public float interpolationSpeed = 10f;    // Controls how quickly the ghost moves to the target position
    public float simulatedLatency = 0.1f;       // Simulated network delay in seconds

    private Queue<PlayerState> stateQueue = new Queue<PlayerState>();
    private Vector3 targetPosition;
    private bool targetJumped;

    private Rigidbody ghostRb;
    public bool isOnGround = true;              // Determines if the ghost is on the ground and can jump

    // Use the same jump force as the player to maintain consistency
    private float jumpForce = 10f;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        ghostRb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        // Initialize target position with the ghost's starting position
        targetPosition = transform.position;
    }

    void Update()
    {
        if (!GameManager.instance.isGamerunning)
            return;
       
        InterpolateMovement();
    }

    /// <summary>
    /// Called by PlayerController to send a new PlayerState.
    /// </summary>
    /// <param name="state">The current state of the player.</param>
    public void ReceiveState(PlayerState state)
    {
        stateQueue.Enqueue(state);
        // Process the state after the simulated latency
        StartCoroutine(ProcessStateWithDelay());
    }

    /// <summary>
    /// Processes one state from the queue after waiting for simulated latency.
    /// </summary>
    private IEnumerator ProcessStateWithDelay()
    {
        yield return new WaitForSeconds(simulatedLatency);
        if (stateQueue.Count > 0)
        {
            PlayerState state = stateQueue.Dequeue();
            targetPosition = state.position;
            targetJumped = state.jumped;
            // If the received state indicates a jump and the ghost is on the ground, trigger a jump.
            if (targetJumped && isOnGround)
            {
                Jump();
            }
        }
    }

    /// <summary>
    /// Smoothly interpolates the ghost's position toward the target position.
    /// </summary>
    private void InterpolateMovement()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, interpolationSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Applies an upward force to simulate a jump.
    /// </summary>
    private void Jump()
    {
        ghostRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isOnGround = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
        }
    }

    
}
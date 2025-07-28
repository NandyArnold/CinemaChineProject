using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    [SerializeField] private float rotationSpeed = 180f;
    [SerializeField] private float sprintMultiplier = 1.5f;
    public LayerMask groundLayer;

    public Transform groundCheck;


    private Rigidbody rb;
    private Vector2 moveInput;
    private bool isOnGround;
    private float detectionRange = 0.5f; // Distance to check for ground
    private bool isSprinting;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        PlayerInputHandler.OnMove += HandleMove;
        PlayerInputHandler.OnJump += HandleJump;
        PlayerInputHandler.OnDance += HandleDance;
        PlayerInputHandler.OnSprint += HandleSprint;
    }

    private void OnDisable()
    {
        PlayerInputHandler.OnMove -= HandleMove;
        PlayerInputHandler.OnJump -= HandleJump;
        PlayerInputHandler.OnDance -= HandleDance;
        PlayerInputHandler.OnSprint -= HandleSprint;
    }

    private void FixedUpdate()
    {

        // Rotate left/right
        float rotation = moveInput.x * rotationSpeed * Time.fixedDeltaTime;
        transform.Rotate(0, rotation, 0);

        // Determine speed
        float speed = moveSpeed;
        if (isSprinting && moveInput.y > 0f)
        {
            speed *= sprintMultiplier;
        }

        // Forward/backward movement
        Vector3 forwardMovement = transform.forward * moveInput.y * speed;
        rb.linearVelocity = new Vector3(forwardMovement.x, rb.linearVelocity.y, forwardMovement.z);
        //Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);

        //// Apply movement
        //Vector3 velocity = move * moveSpeed;
        //rb.linearVelocity = new Vector3(velocity.x, rb.linearVelocity.y, velocity.z);

        //// Rotate towards movement direction (only if there's input)
        //if (move != Vector3.zero)
        //{
        //    Quaternion targetRotation = Quaternion.LookRotation(move, Vector3.up);
        //    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 10f);
        //}
    }

    private void HandleMove(Vector2 input)
    {
        moveInput = input;
    }

    private void HandleJump()
    {
        if (IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void HandleDance()
    {
        Debug.Log("Dance action triggered!");
        // Later: play animation here
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f, groundLayer);
    }

    private void HandleSprint(bool sprinting)
    {
        isSprinting = sprinting;
    }

    void CheckGround()
    {
        isOnGround = Physics.CheckSphere(groundCheck.position, detectionRange, groundLayer);
    }

    // Visualiser la zone de détection du sol dans l'éditeur
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = isOnGround ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, detectionRange);
        }
    }
}


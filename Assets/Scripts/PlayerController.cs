using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private uint maxJumps;

    [Header("Ground Detection")]
    [SerializeField] private Transform groundDetectionPoint;
    [SerializeField] private float groundDetectionRadius;
    [SerializeField] private LayerMask[] includeJumpableLayers;
    [SerializeField] private LayerMask[] excludeJumpableLayers;

    private Rigidbody2D rb;
    private InputActions inputActions;

    private float moveInput;
    private bool jumpInput;
    private bool isGrounded;
    private uint remainingJumps;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        inputActions = new();
        
        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<float>();
        inputActions.Player.Move.canceled += _ => moveInput = 0;
        inputActions.Player.Jump.performed += _ => jumpInput = true;
    }

    private void Start()
    {
        ResetJumps();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
    }

    private void OnDrawGizmos()
    {
        if (groundDetectionPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundDetectionPoint.position, groundDetectionRadius);
        }
    }

    private void Update()
    {
        HandleJump();
        HandleFlip();
    }

    private void FixedUpdate()
    {
        HandleMovement();
        CheckGrounded();
    }

    private void HandleMovement()
    {
        float velocityX = moveInput * moveSpeed;
        rb.linearVelocity = new Vector2(velocityX, rb.linearVelocity.y);
    }

    private void HandleJump()
    {
        if (jumpInput && remainingJumps > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            remainingJumps--;
        }

        jumpInput = false;
    }

    private void CheckGrounded()
    {
        int includeMask = 0;
        foreach (var layer in includeJumpableLayers)
        {
            includeMask |= layer.value;
        }

        int excludeMask = 0;
        foreach (var layer in excludeJumpableLayers)
        {
            excludeMask |= layer.value;
        }

        isGrounded = Physics2D.OverlapCircle(groundDetectionPoint.position, groundDetectionRadius, includeMask & ~excludeMask);

        if (isGrounded)
        {
            ResetJumps();
        }
    }

    private void HandleFlip()
    {
        if (Mathf.Abs(moveInput) > 0)
        {
            Vector3 newRotation = transform.localRotation.eulerAngles;
            newRotation.y = moveInput > 0 ? 0 : 180;
            transform.localRotation = Quaternion.Euler(newRotation);
        }
    }

    private void ResetJumps()
    {
        remainingJumps = (uint)Mathf.Max(maxJumps - 1, 0);
    }
}

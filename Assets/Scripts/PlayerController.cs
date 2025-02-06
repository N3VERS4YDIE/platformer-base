using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] byte maxJumps;

    [Header("Ground Detection")]
    [SerializeField] Transform groundDetectionPoint;
    [SerializeField] float groundDetectionRadius;
    [SerializeField] LayerMask[] includeJumpableLayers;
    [SerializeField] LayerMask[] excludeJumpableLayers;

    Rigidbody2D rb;
    float moveInput;
    bool jumpInput;
    bool isGrounded;
    byte remainingJumps;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        GameManager.Instance.InputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<float>();
        GameManager.Instance.InputActions.Player.Move.canceled += _ => moveInput = 0;
        GameManager.Instance.InputActions.Player.Jump.performed += _ => jumpInput = true;
    }

    void Start()
    {
        ResetJumps();
    }

    void OnEnable()
    {
        GameManager.Instance.InputActions.Player.Enable();
    }

    void OnDisable()
    {
        GameManager.Instance.InputActions.Player.Disable();
    }

    void OnDrawGizmos()
    {
        if (groundDetectionPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundDetectionPoint.position, groundDetectionRadius);
        }
    }

    void Update()
    {
        HandleJump();
        HandleFlip();
    }

    void FixedUpdate()
    {
        HandleMovement();
        CheckGrounded();
    }

    void HandleMovement()
    {
        float velocityX = moveInput * moveSpeed;
        rb.linearVelocity = new Vector2(velocityX, rb.linearVelocity.y);
    }

    void HandleJump()
    {
        if (jumpInput && remainingJumps > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            --remainingJumps;
        }

        jumpInput = false;
    }

    void CheckGrounded()
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

        bool previouslyGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(groundDetectionPoint.position, groundDetectionRadius, includeMask & ~excludeMask);

        if (!previouslyGrounded && isGrounded || isGrounded && rb.linearVelocity.y == 0)
        {
            ResetJumps();
        }

        if (previouslyGrounded && !isGrounded && rb.linearVelocity.y < 0)
        {
            remainingJumps = (byte)Mathf.Max(remainingJumps - 1, 0);
        }
    }


    void HandleFlip()
    {
        if (Mathf.Abs(moveInput) > 0)
        {
            Vector3 newRotation = transform.localRotation.eulerAngles;
            newRotation.y = moveInput > 0 ? 0 : 180;
            transform.localRotation = Quaternion.Euler(newRotation);
        }
    }

    void ResetJumps()
    {
        remainingJumps = maxJumps;
    }
}

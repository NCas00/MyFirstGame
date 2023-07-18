using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private bool isMovingForward;
    [SerializeField] private bool isMovingRight;
    [SerializeField] private bool isSprinting;
    [SerializeField] private bool isJumping;

    [SerializeField] [Range(1f, 100f)] private float speed;
    [SerializeField] [Range(1f, 200f)] private float sprintSpeed;
    [SerializeField] [Range(1f, 20f)] private float groundDrag;

    [SerializeField] [Range(1f, 100f)] private float jumpForce;
    [SerializeField] [Range(1f, 20f)] private float jumpCooldown = 2f;
    [SerializeField] [Range(1f, 10f)] private float maxJumpHeight;

    private float lastJumpTime;
    private float jumpStartPosition;
    private float right;
    private float forward;

    public Rigidbody rb;
    public InputManager actions;
    public GroundCheck groundCheck;
    public ShootingController shootingController;

    private void Awake()
    {
        actions = new InputManager();
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        actions = new InputManager();
        actions.Player.Enable();
        lastJumpTime = -jumpCooldown;
    }

    private void OnEnable()
    {
        actions.Enable();

        actions.Player.Forward.started += ForwardPressed;
        actions.Player.Forward.canceled += ForwardReleased;

        actions.Player.Right.started += RightPressed;
        actions.Player.Right.canceled += RightReleased;

        actions.Player.Sprint.started += SprintPressed;
        actions.Player.Sprint.canceled += SprintReleased;

        actions.Player.Jump.started += JumpPressed;
        actions.Player.Jump.canceled += JumpReleased;
    }

    private void OnDisable()
    {
        actions.Disable();

        actions.Player.Forward.started -= ForwardPressed;
        actions.Player.Forward.canceled -= ForwardReleased;

        actions.Player.Right.started -= RightPressed;
        actions.Player.Right.canceled -= RightReleased;

        actions.Player.Sprint.started -= SprintPressed;
        actions.Player.Sprint.canceled -= SprintReleased;

        actions.Player.Jump.started -= JumpPressed;
        actions.Player.Jump.canceled -= JumpReleased;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    public void ForwardPressed(InputAction.CallbackContext context)
    {
        isMovingForward = true;
        forward = context.ReadValue<float>();
    }

    public void ForwardReleased(InputAction.CallbackContext context)
    {
        isMovingForward = false;
        forward = 0f;

    }

    public void RightPressed(InputAction.CallbackContext context)
    {
        isMovingRight = true;
        right = context.ReadValue<float>();
    }

    public void RightReleased(InputAction.CallbackContext context)
    {
        isMovingRight = false;
        right = 0f;
    }

    public void SprintPressed(InputAction.CallbackContext context)
    {
        isSprinting = true;
    }

    public void SprintReleased(InputAction.CallbackContext context)
    {
        isSprinting = false;
    }

    public void JumpPressed(InputAction.CallbackContext context)
    {
        if (Time.time - lastJumpTime < jumpCooldown)
        {
            return;
        }
        if (groundCheck.coyoteTimeCounter > 0)
        {
            isJumping = true;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            lastJumpTime = Time.time;
            jumpStartPosition = transform.position.y;
        }
    }

    public void JumpReleased(InputAction.CallbackContext context)
    {
        isJumping = false;
        groundCheck.coyoteTimeCounter = 0f;
    }

    public void MovePlayer()
    {
        Vector3 movement = transform.right * right + transform.forward * forward;

        if (groundCheck.isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }

        if (isSprinting)
        {
            rb.AddForce(movement.normalized * sprintSpeed, ForceMode.Force);
        }
        else
        {
            rb.AddForce(movement.normalized * speed, ForceMode.Force);
        }

        if (movement.magnitude > 0.1f)
        {
            transform.rotation = (Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(movement.x, 0, movement.z)), 0.15f));
        }
    }
}


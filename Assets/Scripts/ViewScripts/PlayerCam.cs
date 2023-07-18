using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCam : MonoBehaviour
{
    [SerializeField] [Range(1f, 100f)] private float sensitivity;
    [SerializeField] [Range(1f, 100f)] private float clampAngle;
    [SerializeField] private float verticalRotation;
    [SerializeField] private float horizontalRotation;
    
    public InputManager actions;
    private Vector2 lookInput;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        actions = new InputManager();
    }

    private void Awake()
    {
        actions = new InputManager();
    }

    private void FixedUpdate()
    {
        onLook();
    }

    private void OnEnable()
    {
        actions.Enable();
        actions.Player.Look.performed += LookPerformed;
    }

    private void OnDisable()
    {
        actions.Disable();
        actions.Player.Look.performed -= LookPerformed;
    }

    public void LookPerformed(InputAction.CallbackContext context)
    {
        lookInput = new Vector2(-context.ReadValue<Vector2>().x, context.ReadValue<Vector2>().y) * sensitivity;
    }

    public void onLook()
    {
        float mouseX = lookInput.x;
        float mouseY = lookInput.y;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -clampAngle, clampAngle);
        //verticalRotation = Mathf.Repeat(verticalRotation, 360f);

        horizontalRotation -= mouseX;
        //horizontalRotation = Mathf.Clamp(horizontalRotation, -clampAngle, clampAngle);
        horizontalRotation = Mathf.Repeat(horizontalRotation, 360f);

        transform.rotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0f);

        lookInput = Vector2.zero;
    }
}

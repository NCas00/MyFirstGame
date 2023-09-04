using UnityEngine;
using UnityEngine.InputSystem;

public class RotateScript : MonoBehaviour
{
    [SerializeField] [Range(1f, 100f)] private float sensitivity;
    [SerializeField] [Range(1f, 100f)] private float clampAngle;
    [SerializeField] private float verticalRotation;
    [SerializeField] private float horizontalRotation;

    public InputManager actions;
    private Vector2 rotateInput;

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
        onRotate();
    }

    private void OnEnable()
    {
        actions.Enable();
        actions.Player.Rotate.performed += RotatePerformed;
    }

    private void OnDisable()
    {
        actions.Disable();
        actions.Player.Rotate.performed -= RotatePerformed;
    }

    public void RotatePerformed(InputAction.CallbackContext context)
    {
        rotateInput = new Vector2(-context.ReadValue<Vector2>().x, context.ReadValue<Vector2>().y) * sensitivity;
    }

    public void onRotate()
    {
        float mouseX = rotateInput.x;
        float mouseY = rotateInput.y;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -clampAngle, clampAngle);
        //verticalRotation = Mathf.Repeat(verticalRotation, 360f);

        horizontalRotation -= mouseX;
        //horizontalRotation = Mathf.Clamp(horizontalRotation, -clampAngle, clampAngle);
        horizontalRotation = Mathf.Repeat(horizontalRotation, 360f);

        transform.rotation = Quaternion.Euler(0f, horizontalRotation, 0f);

        rotateInput = Vector2.zero;
    }
}

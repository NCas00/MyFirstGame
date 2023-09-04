using UnityEngine;
using UnityEngine.InputSystem;

public class BWPlayerCam : MonoBehaviour
{
    [SerializeField] [Range(1f, 100f)] private float sensitivity;
    [SerializeField] [Range(1f, 100f)] private float clampAngle;
    [SerializeField] private float verticalRotation;
    [SerializeField] private float horizontalRotation;

    private Vector2 lookInput;

    public InputManagerSingleton inputManagerSingleton;

    private void Awake()
    {
        inputManagerSingleton = InputManagerSingleton.Instance;
    }

    private void OnEnable()
    {
        inputManagerSingleton.Actions.UI.Disable();
        inputManagerSingleton.Actions.Player.Enable();

        inputManagerSingleton.Actions.Player.Look.performed += LookPerformed;
    }

    private void OnDisable()
    {
        inputManagerSingleton.Actions.Player.Disable();
        inputManagerSingleton.Actions.UI.Enable();

        inputManagerSingleton.Actions.Player.Look.performed -= LookPerformed;
    }

    private void FixedUpdate()
    {
        OnLook();
    }

    public void LookPerformed(InputAction.CallbackContext context)
    {
        lookInput = new Vector2(-context.ReadValue<Vector2>().x, context.ReadValue<Vector2>().y) * sensitivity;
    }

    public void OnLook()
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

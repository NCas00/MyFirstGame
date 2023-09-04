using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class BRPlayerController : MonoBehaviour
{
    [SerializeField] private GameObject igOptionsPanel;

    //Movement References and Variables

    public Rigidbody rb;
    public InputManager actions;
    public GroundCheck groundCheck;

    [SerializeField] private bool isSprinting;
    [SerializeField] [Range(1f, 100f)] private float speed;
    [SerializeField] [Range(1f, 200f)] private float sprintSpeed;
    [SerializeField] [Range(1f, 20f)] private float groundDrag;

    [SerializeField] [Range(1f, 100f)] private float jumpForce;
    [SerializeField] [Range(1f, 20f)] private float jumpCooldown = 2f;
    [SerializeField] [Range(1f, 10f)] private float maxJumpHeight;

    private float lastJumpTime, jumpStartPosition;
    private float right, forward;


    // View References and Variables

    private Vector2 lookInput;

    [SerializeField] [Range(1f, 100f)] private float sensitivity;
    [SerializeField] [Range(1f, 100f)] private float clampAngle;
    [SerializeField] private float verticalRotation;
    [SerializeField] private float horizontalRotation;

    //Shooting References and Variables
    [SerializeField] [Range(1f, 200f)] private float shootingDistance = 100f;
    public Action<RaycastHit> BRonDummyHit;

    //Grapple/Swing References and Variables

    [SerializeField] private Vector3 grapplePoint;
    [SerializeField] private LayerMask whatIsGrappleable;
    [SerializeField] private Transform gunMuzzle, cameraPos, player, orientation;
    [SerializeField] private LineRenderer lr;
    private SpringJoint joint;

    [SerializeField] public bool isGrappling;
    [SerializeField] [Range(1f, 200f)] private float maxDistance = 100f;
    [SerializeField] [Range(0.1f, 1f)] private float maxJointDistanceMultiplier = 0.8f;
    [SerializeField] [Range(0.1f, 1f)] private float minJointDistanceMultiplier = 0.25f;
    public Action<AudioClip> BROnGrappleHit;
    private AudioClip audioClip;

    [SerializeField] [Range(1000f, 20000f)] private float forwardThrustForce;
    [SerializeField] [Range(1000f, 20000f)] private float horizontalThrustForce;
    [SerializeField] [Range(1000f, 50000f)] private float pullThrustForce;
    [SerializeField] [Range(1f, 200f)] private float extendCableSpeed;

    public InputActionAsset Actions;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        lastJumpTime = -jumpCooldown;
        lr.positionCount = 2;
        Time.timeScale = 1f;
    }

    private void OnEnable()
    {
        Actions.FindActionMap("UI").Disable();
        Actions.FindActionMap("Player").Enable();

        Actions.FindActionMap("Player").FindAction("OpenOptions").performed += OpenOptions_Performed;

        //Movement
        Actions.FindActionMap("Player").FindAction("Forward").started += ForwardPressed;
        Actions.FindActionMap("Player").FindAction("Forward").canceled += ForwardReleased;

        Actions.FindActionMap("Player").FindAction("Right").started += RightPressed;
        Actions.FindActionMap("Player").FindAction("Right").canceled += RightReleased;

        Actions.FindActionMap("Player").FindAction("Sprint").started += SprintPressed;
        Actions.FindActionMap("Player").FindAction("Sprint").canceled += SprintReleased;

        Actions.FindActionMap("Player").FindAction("Jump").started += JumpPressed;
        Actions.FindActionMap("Player").FindAction("Jump").canceled += JumpReleased;

        //View
        Actions.FindActionMap("Player").FindAction("Look").performed += LookPerformed;

		//Shooting
		Actions.FindActionMap("Player").FindAction("Shoot").performed += ShootPerformed;

		//Grapple
		Actions.FindActionMap("Player").FindAction("Grab").started += GrabStarted;
        Actions.FindActionMap("Player").FindAction("Grab").canceled += GrabCanceled;

        //Swing
        Actions.FindActionMap("Player").FindAction("ForwardSwing").performed += FSPerformed;
        Actions.FindActionMap("Player").FindAction("LeftSwing").performed += LSPerformed;
        Actions.FindActionMap("Player").FindAction("RightSwing").performed += RSPerformed;
        Actions.FindActionMap("Player").FindAction("ShortenCable").performed += SCPerformed;
        Actions.FindActionMap("Player").FindAction("ExtendCable").performed += ECPerformed;

    }

    private void OnDisable()
    {
        Actions.FindActionMap("Player").Disable();
        Actions.FindActionMap("UI").Enable();

        Actions.FindActionMap("Player").FindAction("OpenOptions").performed -= OpenOptions_Performed;

        //Movement
        Actions.FindActionMap("Player").FindAction("Forward").started -= ForwardPressed;
        Actions.FindActionMap("Player").FindAction("Forward").canceled -= ForwardReleased;

        Actions.FindActionMap("Player").FindAction("Right").started -= RightPressed;
        Actions.FindActionMap("Player").FindAction("Right").canceled -= RightReleased;

        Actions.FindActionMap("Player").FindAction("Sprint").started -= SprintPressed;
        Actions.FindActionMap("Player").FindAction("Sprint").canceled -= SprintReleased;

        Actions.FindActionMap("Player").FindAction("Jump").started -= JumpPressed;
        Actions.FindActionMap("Player").FindAction("Jump").canceled -= JumpReleased;

        //View
        Actions.FindActionMap("Player").FindAction("Look").performed -= LookPerformed;

		//Shooting
		Actions.FindActionMap("Player").FindAction("Shoot").performed -= ShootPerformed;

		//Grapple
		Actions.FindActionMap("Player").FindAction("Grab").started -= GrabStarted;
        Actions.FindActionMap("Player").FindAction("Grab").canceled -= GrabCanceled;

        //Swing
        Actions.FindActionMap("Player").FindAction("ForwardSwing").performed -= FSPerformed;
        Actions.FindActionMap("Player").FindAction("LeftSwing").performed -= LSPerformed;
        Actions.FindActionMap("Player").FindAction("RightSwing").performed -= RSPerformed;
        Actions.FindActionMap("Player").FindAction("ShortenCable").performed -= SCPerformed;
        Actions.FindActionMap("Player").FindAction("ExtendCable").performed -= ECPerformed;

        GrabCanceled(default);
    }

    private void FixedUpdate()
    {
        MovePlayer();
        OnLook();
    }

    private void LateUpdate()
    {
        if (lr != null)
        {
            DrawRope();
        }
    }


    private void OpenOptions_Performed(InputAction.CallbackContext context)
    {
        OpenOptionsPanel();
    }

    //OpenPanel
    public void OpenOptionsPanel()
    {
        Time.timeScale = 0f;
        igOptionsPanel.SetActive(true);

        Actions.FindActionMap("Player").Disable();
        Actions.FindActionMap("UI").Enable();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }


    //Movement Methods

    public void ForwardPressed(InputAction.CallbackContext context)
    {
        forward = context.ReadValue<float>();
    }

    public void ForwardReleased(InputAction.CallbackContext context)
    {
        forward = 0f;

    }

    public void RightPressed(InputAction.CallbackContext context)
    {
        //isMovingRight = true;
        right = context.ReadValue<float>();
    }

    public void RightReleased(InputAction.CallbackContext context)
    {
        //isMovingRight = false;
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
            //isJumping = true;

            if (rb != null)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                lastJumpTime = Time.time;
                jumpStartPosition = transform.position.y;
            }
        }
    }

    public void JumpReleased(InputAction.CallbackContext context)
    {
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
    }

    //View Methods

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


	//Shooting Methods

	public void ShootPerformed(InputAction.CallbackContext context)
	{
		Shoot();
	}

	public void Shoot()
	{
		Ray ray = new Ray(player.position, player.forward);
		RaycastHit hit;

		Debug.DrawLine(ray.origin, ray.origin + ray.direction * shootingDistance, Color.red, 1f);

		if (Physics.Raycast(ray, out hit, shootingDistance))
		{
			Debug.Log("colpito");
			Transform hitTransform = hit.collider.transform;
			var normal = hit.normal;
			BRonDummyHit?.Invoke(hit);
		}
	}


	//Grapple/Swing Methods

	public void GrabStarted(InputAction.CallbackContext context)
    {
        RaycastHit hit;

        if (cameraPos == null)
        {
            return;
        }

        if (Physics.Raycast(cameraPos.position, cameraPos.forward, out hit, maxDistance, whatIsGrappleable))
        {
            BROnGrappleHit?.Invoke(audioClip);

            isGrappling = true;
            grapplePoint = hit.point;

            if (Application.isPlaying && joint == null && player != null)
            {
                CreateJoint();
            }

            if (lr != null)
            {
                lr.positionCount = 2;
            }
        }
    }

    public void GrabCanceled(InputAction.CallbackContext context)
    {
        if (joint != null)
        {
            isGrappling = false;
            DestroyJoint();
            lr.positionCount = 0;
        }
    }

    public void FSPerformed(InputAction.CallbackContext context) //swing forward
    {
        if (isGrappling == true)
        {
            rb.AddForce(orientation.forward * forwardThrustForce * Time.deltaTime);
        }
    }

    public void LSPerformed(InputAction.CallbackContext context) //swing left
    {
        if (isGrappling == true)
        {
            rb.AddForce(-orientation.right * horizontalThrustForce * Time.deltaTime);

        }
    }

    public void RSPerformed(InputAction.CallbackContext context) //swing right
    {
        if (isGrappling == true)
        {
            rb.AddForce(orientation.right * horizontalThrustForce * Time.deltaTime);
        }
    }

    public void SCPerformed(InputAction.CallbackContext context) // shorten cable
    {
        if (isGrappling == true && joint != null)
        {
            Vector3 directionToPoint = grapplePoint - transform.position;
            rb.AddForce(directionToPoint.normalized * pullThrustForce * Time.deltaTime);

            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

            joint.maxDistance = distanceFromPoint * maxJointDistanceMultiplier;
            joint.minDistance = distanceFromPoint * minJointDistanceMultiplier;
        }
    }

    public void ECPerformed(InputAction.CallbackContext context) //extend cable
    {
        if (isGrappling == true)
        {
            float extendedDistanceFromPoint = Vector3.Distance(player.position, grapplePoint) + extendCableSpeed;

            joint.maxDistance = extendedDistanceFromPoint * maxJointDistanceMultiplier;
            joint.minDistance = extendedDistanceFromPoint * minJointDistanceMultiplier;
        }
    }

    private void DrawRope()
    {
        if (joint != null && gunMuzzle != null) // Controlla sia il Joint che il gunMuzzle
        {
            lr.SetPosition(0, gunMuzzle.position);
            lr.SetPosition(1, grapplePoint);
        }
    }

    public Vector3 getGrapplePoint()
    {
        return grapplePoint;
    }

    private void CreateJoint()
    {
        if (joint == null)
        {
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);


            joint.maxDistance = distanceFromPoint * maxJointDistanceMultiplier;
            joint.minDistance = distanceFromPoint * minJointDistanceMultiplier;

            joint.spring = 5f;
            joint.damper = 3f;
            joint.massScale = 1f;
        }
    }

    private void DestroyJoint()
    {
        if (joint != null)
        {
            Destroy(joint);
        }
    }
}

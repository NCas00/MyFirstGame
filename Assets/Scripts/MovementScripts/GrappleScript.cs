using UnityEngine;
using UnityEngine.InputSystem;

public class GrappleScript : MonoBehaviour
{
    //Grapple
    [SerializeField] [Range(1f, 200f)] private float maxDistance = 100f;
    [SerializeField] [Range(0.1f, 1f)] private float maxJointDistanceMultiplier = 0.8f;
    [SerializeField] [Range(0.1f, 1f)] private float minJointDistanceMultiplier = 0.25f;
    [SerializeField] public bool isGrappling;

    //Swing
    [SerializeField] [Range(1000f, 20000f)] private float forwardThrustForce;
    [SerializeField] [Range(1000f, 20000f)] private float horizontalThrustForce;
    [SerializeField] [Range(1000f, 20000f)] private float pullThrustForce;
    [SerializeField] [Range(1f, 200f)] private float extendCableSpeed;

    //references
    [SerializeField] private Vector3 grapplePoint;
    [SerializeField] private LayerMask whatIsGrappleable;
    [SerializeField] private Transform gunMuzzle, cameraPos, player, orientation;
    [SerializeField] private LineRenderer lr;
    [SerializeField] private Rigidbody rb;
    private SpringJoint joint;

    public InputManager actions;

    private void Awake()
    {
        actions = new InputManager();
    }

    void Start()
    {
        actions = new InputManager();
        actions.Player.Enable();
    }

    private void LateUpdate()
    {
        DrawRope();
    }

    private void OnEnable()
	{
		actions.Enable();

		actions.Player.Grab.started += GrabStarted;
        actions.Player.Grab.canceled += GrabCanceled;

        actions.Player.ForwardSwing.performed += FSPerformed;
        actions.Player.LeftSwing.performed += LSPerformed;
        actions.Player.RightSwing.performed += RSPerformed;
        actions.Player.ShortenCable.performed += SCPerformed;
        actions.Player.ExtendCable.performed += ECPerformed;
    }

	private void OnDisable()
	{
		actions.Disable();

		actions.Player.Grab.started -= GrabStarted;
        actions.Player.Grab.canceled -= GrabCanceled;

        actions.Player.ForwardSwing.performed -= FSPerformed;
        actions.Player.LeftSwing.performed -= LSPerformed;
        actions.Player.RightSwing.performed -= RSPerformed;
        actions.Player.ShortenCable.performed -= SCPerformed;
        actions.Player.ExtendCable.performed -= ECPerformed;

        if (joint != null)
        {
            GrabCanceled(default);
        }
    }

	public void GrabStarted(InputAction.CallbackContext context)
	{
        
        RaycastHit hit;

        if(Physics.Raycast(cameraPos.position, cameraPos.forward, out hit, maxDistance, whatIsGrappleable))
        {
            isGrappling = true;
            grapplePoint = hit.point;
            CreateJoint();
            lr.positionCount = 2;
        }
	}

    public void GrabCanceled(InputAction.CallbackContext context)
    {
        isGrappling = false;
        DestroyJoint();
        lr.positionCount = 0;
    }

    public void FSPerformed(InputAction.CallbackContext context) //swing forward
    {
        if (isGrappling == true)
        {
            rb.AddForce( orientation.forward * forwardThrustForce * Time.deltaTime);
        }
    }

    public void LSPerformed(InputAction.CallbackContext context) //swing left
    {
        if (isGrappling == true)
        {
            rb.AddForce( -orientation.right * horizontalThrustForce * Time.deltaTime);

        }
    }

    public void RSPerformed(InputAction.CallbackContext context) //swing right
    {
        if (isGrappling == true)
        {
            rb.AddForce( orientation.right * horizontalThrustForce * Time.deltaTime);
        }
    }

    public void SCPerformed(InputAction.CallbackContext context) // shorten cable
    {
        if (isGrappling == true)
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
            float extendedDistanceFromPoint = Vector3.Distance(player.position, grapplePoint) + extendCableSpeed ;

            joint.maxDistance = extendedDistanceFromPoint * maxJointDistanceMultiplier;
            joint.minDistance = extendedDistanceFromPoint * minJointDistanceMultiplier;
        }
    }

    private void DrawRope()
    {
        if (joint != null)
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
        if(joint == null)
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
        if(joint != null)
        {
            Destroy(joint);
            joint = null;
        }
    }
}

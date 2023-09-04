using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class GrappleScript : MonoBehaviour
{
    //Grapple
    [SerializeField] public bool isGrappling;
    [SerializeField] [Range(1f, 200f)] private float maxDistance = 100f;
    [SerializeField] [Range(0.1f, 1f)] private float maxJointDistanceMultiplier = 0.8f;
    [SerializeField] [Range(0.1f, 1f)] private float minJointDistanceMultiplier = 0.25f;
    public Action<AudioClip> OnGrappleHit;
    private AudioClip audioClip;
    

    //Swing
    [SerializeField] [Range(1000f, 20000f)] private float forwardThrustForce;
    [SerializeField] [Range(1000f, 20000f)] private float horizontalThrustForce;
    [SerializeField] [Range(1000f, 50000f)] private float pullThrustForce;
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
        lr.positionCount = 2;
    }

    private void LateUpdate()
    {
        if (lr != null) // Verifica se il componente LineRenderer esiste ancora
        {
            DrawRope();
        }
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

        GrabCanceled(default);
    }

    public void GrabStarted(InputAction.CallbackContext context)
    {
        RaycastHit hit;

        if (cameraPos == null)
        {
            return; 
        }

        if (Physics.Raycast(cameraPos.position, cameraPos.forward, out hit, maxDistance, whatIsGrappleable))
        {
            OnGrappleHit?.Invoke(audioClip);

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
            float extendedDistanceFromPoint = Vector3.Distance(player.position, grapplePoint) + extendCableSpeed ;

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
        }
    }
}

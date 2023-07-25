using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public float distanceToCheck = 0.5f;
    public bool isGrounded;

    [SerializeField][Range(0.1f, 1f)] private float coyoteTime = 0.5f;

    public float coyoteTimeCounter;

    private void Update()
    {
        if (Physics.Raycast(transform.position, Vector2.down, distanceToCheck))
        {
            isGrounded = true;
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            isGrounded = false;
            coyoteTimeCounter -= Time.deltaTime;
        }
    }
}

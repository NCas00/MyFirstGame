using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGun : MonoBehaviour
{
    public GrappleScript grapple;

    private Quaternion desideredRotation;
    private float rotationSpeed = 5f;

    private void Update()
    {
        if (!grapple.isGrappling)
        {
            desideredRotation = transform.parent.rotation;
        }
        else
        {
            desideredRotation = Quaternion.LookRotation(grapple.getGrapplePoint() - transform.position);
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, desideredRotation, Time.deltaTime * rotationSpeed);
    }
}

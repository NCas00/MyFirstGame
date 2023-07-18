using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictimController : MonoBehaviour
{
    [SerializeField] [Range(1f,100f)] private float impactForce = 1f;

    private void Start()
    {
        ShootingController shootingController = FindObjectOfType<ShootingController>();
        shootingController.onDummyHit += HandleCubeHit;
    }
    private void HandleCubeHit(RaycastHit hit)
    {
        if (hit.transform.gameObject == transform.gameObject)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.AddForce(-hit.normal * impactForce, ForceMode.Impulse);
        }
    }
}

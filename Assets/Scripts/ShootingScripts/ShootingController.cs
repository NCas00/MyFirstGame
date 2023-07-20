using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShootingController : MonoBehaviour
{
    [SerializeField][Range(1f, 200f)] private float shootingDistance = 100f;

    private Transform pistolero;

    public Action<RaycastHit> onDummyHit;

    public InputManager actions;

    private List<VictimController> activeEnemies = new List<VictimController>();

    private void Awake()
    {
        pistolero = transform;
        actions = new InputManager();
    }

    private void OnEnable()
    {
        actions.Enable();

        actions.Player.Shoot.performed += ShootPerformed;
    }

    private void OnDisable()
    {
        actions.Disable();

        actions.Player.Shoot.performed -= ShootPerformed;
    }

    public void ShootPerformed(InputAction.CallbackContext context)
    {
        Shoot();
    }

    public void Shoot()
    {
        Ray ray = new Ray(pistolero.position, pistolero.forward);
        RaycastHit hit;

        Debug.DrawLine(ray.origin, ray.origin + ray.direction * shootingDistance, Color.red, 1f);

        if (Physics.Raycast(ray, out hit, shootingDistance))
        {
            Transform hitTransform = hit.collider.transform;
            var normal = hit.normal; 
            onDummyHit?.Invoke(hit);
        }
    }
}

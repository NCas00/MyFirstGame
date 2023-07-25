using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictimController : MonoBehaviour
{
    [SerializeField] [Range(1f,100f)] private float impactForce;
    private EnemyAi enemy;

    private void Start()
    {
        ShootingController shootingController = FindObjectOfType<ShootingController>();
        shootingController.onDummyHit += HandleCubeHit;
    }

    private void HandleCubeHit(RaycastHit hit)
    {
        if (hit.transform.CompareTag("Enemy"))
        {
            Debug.Log("HandleCubeHit");
            enemy = hit.transform.GetComponent<EnemyAi>();
            Rigidbody rb = hit.transform.GetComponent<Rigidbody>();
            rb.AddForce(-hit.normal * impactForce, ForceMode.Impulse);

            enemy.DealDamageToEnemy();
            
            if (enemy != null)
            {
                
                /*
                for (int i = 0; i < 3; i++)
                {
                    enemy.DealDamageToEnemy();
                }*/
            }
        }
    }
}

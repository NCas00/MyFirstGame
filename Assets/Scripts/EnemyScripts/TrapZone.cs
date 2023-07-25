using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyAi enemy = other.GetComponent<EnemyAi>();
            if(enemy != null)
            {
                enemy.DealDamageToEnemy();
            }
        }
        
    }
}

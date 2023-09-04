using UnityEngine;

public class BWVictimController : MonoBehaviour
{
    [SerializeField] [Range(1f,100f)] private float impactForce;
    private EnemyAi enemy;
    private BWPlayerController playerController;

    private void Start()
    {
        playerController = FindObjectOfType<BWPlayerController>();
        playerController.BWonDummyHit += HandleCubeHit;
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

            /*
            if (enemy != null)
            {
                
                
                for (int i = 0; i < 3; i++)
                {
                    enemy.DealDamageToEnemy();
                }
            }*/
        }
    }
}

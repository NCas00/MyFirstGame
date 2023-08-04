using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    

    [SerializeField] private float enemySpeed;

    [SerializeField] private int enemyMaxHP, enemyHP;

    [SerializeField] private Transform player;

    [SerializeField] private LayerMask whatIsGround, whatIsPlayer;

    [SerializeField] GameOverManager gameOverManager;
    [SerializeField] EnemySpawner spawner;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] public EnemyHPBar healthBar;

    //Patroling
    [SerializeField] private Vector3 walkPoint;
    [SerializeField] private bool walkPointSet;
    [SerializeField] public float walkPointRange;

    //Attacking
    [SerializeField] private float timeBetweenAttacks;
    //[SerializeField] private bool alreadyAttacked;

    //States
    [SerializeField] private float sightRange, attackRange;
    [SerializeField] private bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = enemySpeed;
        gameOverManager = GameObject.FindAnyObjectByType<GameOverManager>();
        spawner = FindObjectOfType<EnemySpawner>();
        healthBar = GetComponentInChildren<EnemyHPBar>();
    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange  && !playerInAttackRange) Patroling();
        if ( playerInSightRange  && !playerInAttackRange) ChasePlayer();
        if ( playerInSightRange && playerInAttackRange) AttackPlayer();
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet) agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f) walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if(Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }
    
    private void AttackPlayer()
    {
        agent.SetDestination(player.position);

        transform.LookAt(player);

        EndGame();

        /*if (!alreadyAttacked)
        {

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);

            EndGame();
        }*/
    }

    /*
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
    */

    public void DealDamageToEnemy()
    {
        enemyHP--;

        healthBar.UpdateHealthBar(enemyMaxHP, enemyHP);

        if (enemyHP == 0)
        {
            DestroyEnemy();
        }
    }

    public int GetEnemyHP()
    {
        return enemyHP;
    }
    
    public void DestroyEnemy()
    {
        if(gameObject != null)
        {
            if(spawner != null)
            {
                spawner.RemoveEnemy(gameObject);
            }

            gameObject.SetActive(false);
            //Destroy(gameObject);
        }
    }

    private void EndGame()
    {
        Debug.Log("Endgame");
        gameOverManager.Loss();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}

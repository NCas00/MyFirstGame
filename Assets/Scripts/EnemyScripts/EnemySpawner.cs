using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] [Range(1f, 10f)] private float spawnInterval = 2f;
    [SerializeField] [Range(1f, 50f)] private float maxEnemies;

    public List<GameObject> activeEnemies = new List<GameObject>();
    public List<GameObject> destroyedEnemies = new List<GameObject>();

    private void Start()
    {
        // Avvia la generazione dei nemici
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            if(activeEnemies.Count < maxEnemies)
            {
                Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

                GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

                activeEnemies.Add(enemy);
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void RemoveEnemy(GameObject enemy)
    {
        activeEnemies.Remove(enemy);
        destroyedEnemies.Add(enemy);
    }
}

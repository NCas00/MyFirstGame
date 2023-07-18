using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyCounter : MonoBehaviour
{
    private TextMeshProUGUI counterText;
    public EnemySpawner enemySpawner;

    private void Start()
    {
        counterText = GetComponent<TextMeshProUGUI>();
    }
    private void Update()
    {
        if (enemySpawner != null)
        {
            int destroyedEnemiesCount = enemySpawner.destroyedEnemies.Count;
            counterText.text = "Destroyed Enemies: " + destroyedEnemiesCount.ToString();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    public void UpdateHealthBar(float maxHealth, float currentHealth)
    {
        slider.value = currentHealth / maxHealth;
        transform.position = target.position + offset;
    }
}

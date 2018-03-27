using System.Collections;
using System.Collections.Generic;
using TDC;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviourSingleton<EnemyUI>
{
    [SerializeField]
    private Image enemyHealthBar;

    [HideInInspector]
    public Health health;

    private void Start()
    {
        health = GetComponent<Health>();
    }

    public void HealthBarDamage(uint currentHealth)
    {
        enemyHealthBar.fillAmount = (float)currentHealth / 100;
    }
}

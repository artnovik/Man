using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Tooltip("Locomotion reference")]
    [HideInInspector]
    public Locomotion locomotion;

    [Header("Health Manager")]
    public int maxHealth = 100;
    public int currentHealth = 100;
    public int minHealth = 0;

    public bool isDead;

    #region HealthManager

    public void Heal(int healValue)
    {
        currentHealth += healValue;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public void Damage(int damageValue)
    {
        if (currentHealth == 0)
        {
            return;
        }

        currentHealth -= damageValue;

        if (currentHealth <= minHealth)
        {
            currentHealth = 0;
            Death();
        }

        // Temp
        if (gameObject.CompareTag("Enemy"))
        {
            GetComponent<EnemyUI>().HealthBarDamage(currentHealth);
        }
    }

    private void Death()
    {
        locomotion.animControl.SetTrigger("Death");
        isDead = true;
    }

    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Tooltip("Locomotion reference")]
    [HideInInspector]
    public Locomotion locomotion;

    [Header("Health Manager")]
    public uint maxHealth = 100;
    public uint currentHealth = 100;

    #region HealthManager

    public void Heal(uint healValue)
    {
        currentHealth += healValue;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public void Damage(uint damage)
    {
        if (currentHealth == 0)
        {
            return;
        }

        currentHealth -= damage;

        if (damage >= currentHealth)
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
    }

    #endregion
}

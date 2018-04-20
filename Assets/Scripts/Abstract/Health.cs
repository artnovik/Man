using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Health : MonoBehaviour
{
    [Tooltip("Locomotion reference")]
    public Locomotion locomotion;

    [Header("Health Manager")]
    public int maxHealth;
    public int currentHealth;
    public int minHealth;

    public bool isDead;

    #region HealthManager

    public virtual void Start()
    {
        currentHealth = maxHealth;
        locomotion = GetComponent<Locomotion>();
    }

    public virtual void Heal(int healValue)
    {
        if (isDead)
            return;

        currentHealth += healValue;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public virtual void Damage(int damageValue)
    {
        if (isDead)
        {
            return;
        }

        currentHealth -= damageValue;
        Debug.Log(gameObject.name + " took " + damageValue + " damage. Current HP: " + currentHealth);

        if (currentHealth <= minHealth)
        {
            Death();
            Debug.Log(gameObject.name + " died.");
        }
    }

    public virtual void Death()
    {
        locomotion.animControl.SetTrigger("Death");
        locomotion.targetLocomotion = null;

        currentHealth = 0;
        PlayerControl.Instance.inBattle = false;
        isDead = true;
    }

    #endregion
}

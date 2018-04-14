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

    public void StartI()
    {
        currentHealth = maxHealth;
        locomotion = GetComponent<Locomotion>();
    }

    public abstract void Heal(int healValue);
    protected void HealI(int healValue)
    {
        if (isDead)
            return;

        currentHealth += healValue;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public abstract void Damage(int damageValue);
    protected void DamageI(int damageValue)
    {
        if (isDead)
        {
            return;
        }

        currentHealth -= damageValue;

        if (currentHealth <= minHealth)
        {
            Death();
        }
    }

    public abstract void Death();
    protected void DeathI()
    {
        locomotion.animControl.SetTrigger("Death");
        locomotion.targetLocomotion = null;

        currentHealth = 0;
        PlayerControl.Instance.inFightStatus = false;
        isDead = true;
    }

    #endregion
}

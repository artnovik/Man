using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Health : MonoBehaviour
{
    [Tooltip("Locomotion reference")]
    public Locomotion locomotion;

    [Tooltip("CStats reference")]
    public CharacterStats characterStats;

    [Header("Health Manager")]
    public int maxHealth;

    public int currentHealth { get; private set; }
    public int minHealth;

    public bool isDead;

    #region HealthManager

    public virtual void Start()
    {
        currentHealth = maxHealth;
        locomotion = GetComponent<Locomotion>();
        characterStats = GetComponent<CharacterStats>();
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

        if (currentHealth <= minHealth)
        {
            Death();
        }

        Debug.Log(string.Format("{0} took {1} damage. Current HP: {2}", gameObject.name, damageValue, currentHealth));
        if (isDead)
        {
            Debug.Log(string.Format("{0} died.", gameObject.name));
        }
    }

    protected virtual void Death()
    {
        locomotion.animControl.SetTrigger("Death");
        locomotion.targetLocomotion = null;

        currentHealth = 0;
        PlayerControl.Instance.inBattle = false;
        isDead = true;
    }

    #endregion
}

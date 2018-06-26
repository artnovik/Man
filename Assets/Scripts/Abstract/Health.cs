using System;
using UnityEngine;

public abstract class Health : MonoBehaviour
{
    [Tooltip("CStats reference")] private CharacterStats characterStats;

    public bool isDead;

    [Tooltip("Locomotion reference")] public Locomotion locomotion;

    [Header("Health Manager")] public int maxHealth;

    public int minHealth;

    public int currentHealth { get; private set; }

    [Range(0f, 1f)] public float powerArmor = 0.5f;

    #region HealthManager

    public virtual void Start()
    {
        currentHealth = maxHealth;
        locomotion = GetComponent<Locomotion>();
        characterStats = GetComponent<CharacterStats>();
    }

    public virtual void Heal(int healValue)
    {
        if (isDead && healValue < 0)
        {
            return;
        }

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

        if (locomotion.typeLocomotion == Locomotion.TLocomotion.Attack && locomotion.animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.4f)
        {
            Debug.Log("Crytical damage ->>>>>>>>>>>>>>>>>>>>>> " + (int)(damageValue * 1.5f));
            currentHealth -= (int)(damageValue * 1.5f);
        }
        else
        {
            if (locomotion.typeLocomotion != Locomotion.TLocomotion.Block)
            {
                currentHealth -= damageValue;
            }
            else
            {
                currentHealth -= Mathf.CeilToInt(damageValue * (1 - powerArmor));
            }
        }

        if (currentHealth <= minHealth)
        {
            Death();
        }

        Debug.Log($"{gameObject.name} took {damageValue} damage. Current HP: {currentHealth}");
        if (isDead)
        {
            Debug.Log($"{gameObject.name} died.");
        }
    }

    protected virtual void Death()
    {
        locomotion.animator.SetTrigger("Death");
        locomotion.targetLocomotion = null;

        currentHealth = 0;
        PlayerData.Instance.inBattle = false;
        isDead = true;
    }

    #endregion
}
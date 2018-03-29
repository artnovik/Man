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

    private const float destroyDuration = 5f;

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
        if (isDead)
        {
            return;
        }

        currentHealth -= damageValue;

        if (currentHealth <= minHealth)
        {
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
        locomotion.target = null;

        currentHealth = 0;
        isDead = true;

        // Temp
        if (gameObject.CompareTag("Enemy"))
        {
            GetComponent<EnemyUI>().DestroyEnemyUI(destroyDuration);
            DestroyBody(destroyDuration);
        }
    }

    private void DestroyBody(float delay)
    {
        StartCoroutine(Destroy(delay));
    }

    private IEnumerator Destroy(float delay)
    {
        yield return new WaitForSeconds(delay);
        // ToDo nice corpse disappearing animation, with blood puddle for X seconds
        Destroy(gameObject);
    }

    #endregion
}

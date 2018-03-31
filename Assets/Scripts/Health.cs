using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Tooltip("Locomotion reference")]
    [HideInInspector]
    public Locomotion locomotion;

    private Collider playerCollider;

    [Header("Health Manager")]
    public int maxHealth = 100;
    public int currentHealth;
    public int minHealth = 0;

    private const float destroyDuration = 5f;

    public bool isDead;

    #region HealthManager

    private void Start()
    {
        currentHealth = maxHealth;
        locomotion = GetComponent<Locomotion>();
        playerCollider = PlayerControl.Instance.playerCollider;
    }

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
            GetComponent<AIEnemy>().SetRagdoll(true);
            DisableCollidersBetweenEnemyAndPlayer(2f);
            DestroyComponents();
            DestroyBody(destroyDuration);
        }
    }

    private void DestroyComponents()
    {
        Destroy(GetComponent<Locomotion>());
        Destroy(GetComponent<AIEnemy>());
        Destroy(GetComponent<CapsuleCollider>());
    }

    private void DestroyBody(float delay)
    {
        StartCoroutine(Destroy(delay));
    }

    private void DisableCollidersBetweenEnemyAndPlayer(float delay)
    {
        StartCoroutine(DisableColliderRoutine(delay));
    }

    private IEnumerator DisableColliderRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);

        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            Physics.IgnoreCollision(collider, playerCollider);
        }
    }

    private IEnumerator Destroy(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Nice spawning
        SpawnManager.Instance.SpawnWithMessageIfNoEnemies(SpawnManager.Instance.enemyZombie, 0.5f);
        // ToDo nice corpse disappearing animation, with blood puddle for X seconds
        Destroy(gameObject);
    }

    #endregion
}

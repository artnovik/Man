using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthEnemy : Health
{
    private Collider playerCollider;

    #region HealthManager

    private void Start()
    {
        StartI();

        playerCollider = PlayerControl.Instance.playerCollider;
    }

    public override void Heal(int healValue)
    {
        HealI(healValue);
        // Custom implementation
    }

    public override void Damage(int damageValue)
    {
        DamageI(damageValue);

        GetComponent<EnemyUI>().HealthBarDamage(currentHealth);
    }

    public override void Death()
    {
        DeathI();

        GetComponent<EnemyUI>().DestroyEnemyUI(SpawnManager.Instance.GetDeadBodyDeleteDuration());
        GetComponent<AIEnemy>().SetRagdoll(true);
        DisableCollidersBetweenEnemyAndPlayer(2f);
        DestroyComponents();
        DestroyBody(SpawnManager.Instance.GetDeadBodyDeleteDuration());
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

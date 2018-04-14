using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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

        GetComponent<EnemyUI>().HealthBarValueChange(currentHealth);
        // Custom implementation
    }

    public override void Damage(int damageValue)
    {
        DamageI(damageValue);

        GetComponent<EnemyUI>().HealthBarValueChange(currentHealth);

        if (Cheats.Instance.LIFESTEAL)
        {
            PlayerControl.Instance.playerHealth.Heal((int)(damageValue * 0.5));
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        foreach (var weapon in PlayerControl.Instance.listWeapons)
        {
            if (collider.gameObject == weapon /*&& collider is BoxCollider*/ && !isDead)
            {
                Damage(Random.Range(15, 35));
            }
        }
    }

    public override void Death()
    {
        DeathI();

        GetComponent<EnemyUI>().DestroyEnemyUI(SpawnManager.Instance.GetDeadBodyDeleteDuration());
        GetComponent<AIBattle>().SetRagdoll(true);
        DisableCollidersBetweenEnemyAndPlayer(2f);
        DestroyComponents();
        DestroyBody(SpawnManager.Instance.GetDeadBodyDeleteDuration());
    }

    private void DestroyComponents()
    {
        Destroy(GetComponent<Locomotion>());
        Destroy(GetComponent<AIBattle>());
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

        Collider[] enemyColliders = GetComponentsInChildren<Collider>();
        foreach (Collider collider in enemyColliders)
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

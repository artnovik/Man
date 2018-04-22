using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class HealthEnemy : Health
{
    private Collider playerCollider;

    // ToDo Move into EnemyContainer (Inventory)
    public Weapon enemyWeapon;

    #region HealthManager

    public override void Start()
    {
        base.Start();

        // Custom implementation
        playerCollider = PlayerControl.Instance.playerCollider;
    }

    public override void Heal(int healValue)
    {
        base.Heal(healValue);

        // Custom implementation
        GetComponent<EnemyUI>().HealthBarValueChange(currentHealth);
    }

    public override void Damage(int damageValue)
    {
        base.Damage(damageValue);

        // Custom implementation
        GetComponent<EnemyUI>().HealthBarValueChange(currentHealth);

        if (Cheats.Instance.LIFESTEAL)
        {
            PlayerControl.Instance.playerHealth.Heal((int) (damageValue * 0.5));
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        foreach (GameObject weapon in PlayerControl.Instance.listWeapons)
        {
            if (collider.gameObject == weapon && !isDead)
            {
                var takenDamage = PlayerControl.Instance.GetCurrentWeapon().GetDamage();
                Damage(takenDamage);

                EffectsManager.Instance.ActivateBloodEffect(collider.transform);
            }
        }
    }

    protected override void Death()
    {
        base.Death();

        // Custom implementation
        GetComponent<AIBattle>().SetRagdoll(true);
        DisableCollidersBetweenEnemyAndPlayer(2f);

        Instantiate(enemyWeapon.weaponStats.gamePrefab, enemyWeapon.transform.position, enemyWeapon.transform.rotation,
            null);
        Destroy(enemyWeapon.gameObject);

        GetComponent<EnemyUI>().DestroyEnemyUI(SpawnManager.Instance.GetDeadBodyDeleteDuration());
        DestroyBody(SpawnManager.Instance.GetDeadBodyDeleteDuration());
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

        var enemyColliders = GetComponentsInChildren<Collider>();
        foreach (Collider col in enemyColliders)
        {
            Physics.IgnoreCollision(col, playerCollider);
        }
    }

    private IEnumerator Destroy(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Nice spawning
        if (!SpawnManager.Instance.CheckForEnemies(SpawnManager.Instance.enemyZombie))
        {
            SpawnManager.Instance.SpawnEnemies(SpawnManager.Instance.enemyZombie, 0.5f, true);
        }

        // ToDo nice corpse disappearing animation, with blood puddle for X seconds
        GenerateContainer();

        Destroy(gameObject);
    }

    private static void GenerateContainer()
    {
    }

    #endregion
}
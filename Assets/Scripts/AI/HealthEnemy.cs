using System;
using System.Collections;
using UnityEngine;

public class HealthEnemy : Health
{
    public WeaponData enemyWeaponData;
    public Transform enemyWeaponParentTransform;
    [HideInInspector] public GameObject passiveEnemyWeaponGO;
    public Transform containerPlaceTransform;
    private Collider playerCollider;

    public ContainerTypeEnum.Enum enemyType;

    #region HealthManager

    public override void Start()
    {
        base.Start();

        // Custom implementation
        playerCollider = PlayerData.Instance.playerCollider;
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

        if (CheatManager.Instance.LIFESTEAL)
        {
            PlayerData.Instance.playerHealth.Heal((int) (damageValue * 0.5));
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if ((collider.gameObject == PlayerData.Instance.GetCurrentWeaponGO() ||
             collider.gameObject == PlayerData.Instance.rightHandFist ||
             collider.gameObject == PlayerData.Instance.leftHandFist) && !isDead)
        {
            var takenDamage = PlayerData.Instance.GetCurrentWeaponDamage();
            Damage(takenDamage);

            EffectsManager.Instance.ActivateBloodEffect(collider.transform);
        }
    }

    protected override void Death()
    {
        base.Death();

        // Custom implementation
        GetComponent<AIBattle>().SetRagdoll(true);
        DisableCollidersBetweenEnemyAndPlayer(2f);
        DestroyComponents();

        #region Weapon Drop Physics

        passiveEnemyWeaponGO = Instantiate(enemyWeaponData.weaponData.itemPassivePrefab,
            enemyWeaponData.gameObject.transform.position,
            enemyWeaponData.gameObject.transform.rotation);

        Destroy(enemyWeaponData.gameObject);

        #endregion

        GetComponent<EnemyUI>().DestroyEnemyUI(SpawnManager.Instance.GetDeadBodyDeleteDuration());
        DestroyBody(SpawnManager.Instance.GetDeadBodyDeleteDuration());
    }

    private void DestroyComponents()
    {
        Destroy(GetComponent<Locomotion>());
        Destroy(GetComponent<CapsuleCollider>());
        Destroy(GetComponent<AIBattle>());
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
        foreach (Collider col in enemyColliders) Physics.IgnoreCollision(col, playerCollider);
    }

    private IEnumerator Destroy(float delay)
    {
        var enemyContainer = ContainerGenerator.Instance.GenerateAndFillContainer(
            ContainerGenerator.Instance.containerCorpsePrefab,
            containerPlaceTransform, enemyType);

        ContainerGenerator.Instance.AddToContainer(enemyContainer, enemyWeaponData.weaponData);
        enemyContainer.associatedEnemyWeapon = passiveEnemyWeaponGO;

        yield return new WaitForSeconds(delay);

        // Spawning enemies, if none left
        if (!SpawnManager.Instance.CheckForEnemies(SpawnManager.Instance.enemyZombie))
        {
            SpawnManager.Instance.SpawnEnemies(SpawnManager.Instance.enemyZombie, 0.5f, true);
        }

        // ToDo: CorpseDisappearing Animation (under ground), with blood puddle for X seconds.

        //Destroy(passiveEnemyWeaponGO);
        Destroy(gameObject);
    }

    private void OnMouseDown()
    {
        if (CheatManager.Instance.FAST_TESTING)
        {
            Death();
        }
    }

    #endregion
}
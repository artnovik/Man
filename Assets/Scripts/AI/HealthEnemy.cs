using System;
using System.Collections;
using UnityEngine;

public class HealthEnemy : Health
{
    [Header("Data")]
    public bool randomWeapon = true;

    public WeaponData enemyWeaponData;
    public Transform enemyWeaponParentTransform;
    [HideInInspector] public GameObject passiveEnemyWeaponGO;
    public Transform containerPlaceTransform;
    private Collider playerCollider;

    public ContainerTypeEnum.Enum enemyType;
    public CharacterAudio characterAudio;

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

        SquadData.Instance.GetCurrentCharacter().locomotion.GetComponent<LocomotionPlayer>().AddSpecialPower();

        characterAudio.PlayHit();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if ((collider.gameObject == PlayerData.Instance.GetCurrentWeaponGO() ||
             collider.gameObject == PlayerData.Instance.rightHandFist ||
             collider.gameObject == PlayerData.Instance.leftHandFist) && !isDead)
        {
            var takenDamage = PlayerData.Instance.GetCurrentWeaponDamage();
            Damage(takenDamage);

            if (locomotion.typeLocomotion != Locomotion.TLocomotion.Block)
            {
                EffectsManager.Instance.ActivateBloodEffect(collider.transform);
            }
            else
            {
                EffectsManager.Instance.ActivateHitBlockEffect(collider.transform);
            }
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

        if (enemyWeaponData)
        {
            passiveEnemyWeaponGO = Instantiate(enemyWeaponData.weaponData.itemPassivePrefab,
                enemyWeaponData.gameObject.transform.position,
                enemyWeaponData.gameObject.transform.rotation);

            Destroy(enemyWeaponData.gameObject);
        }

        #endregion

        GetComponent<EnemyUI>().DestroyEnemyUI(SpawnManager.Instance.GetDeadBodyDeleteDuration());
        //DestroyBody(SpawnManager.Instance.GetDeadBodyDeleteDuration());
        DestroyBody(5f);
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

        if (enemyWeaponData) { ContainerGenerator.Instance.AddToContainer(enemyContainer, enemyWeaponData.weaponData); }
        ((ContainerCorpse) enemyContainer).associatedEnemyWeapon = passiveEnemyWeaponGO;

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
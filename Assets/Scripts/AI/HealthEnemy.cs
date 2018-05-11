using System.Collections;
using UnityEngine;

public class HealthEnemy : Health
{
    public WeaponData weaponData;
    public Transform containerPlaceTransform;
    private Collider playerCollider;

    [SerializeField] private ContainerTypeEnum.Enum enemyType;

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
        if (collider.gameObject == PlayerData.Instance.GetCurrentWeaponGO() && !isDead)
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
        // ToDo: Improve. Swap between Active/Passive Prefabs.
        
        weaponData.gameObject.transform.SetParent(null);
        weaponData.gameObject.AddComponent<Rigidbody>();
        weaponData.gameObject.GetComponent<BoxCollider>().isTrigger = false;

        #endregion


        GetComponent<EnemyUI>().DestroyEnemyUI(SpawnManager.Instance.GetDeadBodyDeleteDuration());
        DestroyBody(SpawnManager.Instance.GetDeadBodyDeleteDuration());
    }

    private void DestroyComponents()
    {
        Destroy(GetComponent<Locomotion>());
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
        yield return new WaitForSeconds(delay);

        // Nice spawning
        if (!SpawnManager.Instance.CheckForEnemies(SpawnManager.Instance.enemyZombie))
        {
            SpawnManager.Instance.SpawnEnemies(SpawnManager.Instance.enemyZombie, 0.5f, true);
        }

        // ToDo: CorpseDisappearing Animation (under ground), with blood puddle for X seconds.

        Destroy(weaponData.gameObject);
        ContainerGenerator.Instance.GenerateAndFillContainer(ContainerGenerator.Instance.containerCorpsePrefab,
            containerPlaceTransform, enemyType);
        Destroy(gameObject);
    }

    private void OnMouseDown()
    {
        if (CheatManager.Instance.FAST_TESTING)
            Death();
    }

    #endregion
}
using System.Collections;
using UnityEngine;

public class HealthEnemy : Health
{
    public Weapon enemyWeapon;
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

    public override void Heal(uint healValue)
    {
        base.Heal(healValue);

        // Custom implementation
        GetComponent<EnemyUI>().HealthBarValueChange(currentHealth);
    }

    public override void Damage(uint damageValue)
    {
        base.Damage(damageValue);

        // Custom implementation
        GetComponent<EnemyUI>().HealthBarValueChange(currentHealth);

        if (CheatManager.Instance.LIFESTEAL)
        {
            PlayerData.Instance.playerHealth.Heal((uint) (damageValue * 0.5));
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        foreach (GameObject weapon in PlayerData.Instance.listWeapons)
            if (collider.gameObject == weapon && !isDead)
            {
                var takenDamage = PlayerData.Instance.GetCurrentWeapon().GetDamage();
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

        // ToDo nice corpse disappearing animation, with blood puddle for X seconds

        ContainerGenerator.Instance.GenerateContainerObject(containerPlaceTransform, enemyType);
        Destroy(gameObject);
    }

    private void OnMouseDown()
    {
        Death();
    }

    #endregion
}
using UnityEngine;

public class HealthPlayer : Health
{
    [SerializeField] private Transform hitTransform;

    public override void Start()
    {
        base.Start();
    }

    public override void Heal(int healValue)
    {
        base.Heal(healValue);

        // Custom implementation
        GameplayUI.Instance.HealthBarValueChange(currentHealth);
    }

    public override void Damage(int damageValue)
    {
        if (!PlayerData.Instance.isBlock && !CheatManager.Instance.GOD_MODE)
        {
            base.Damage(damageValue);

            // Custom implementation
            GameplayUI.Instance.HealthBarValueChange(currentHealth);
            GameplayUI.Instance.ActivatePlayerHitScreenEffect();

            EffectsManager.Instance.ActivateBloodEffect(hitTransform);
        }
    }

    protected override void Death()
    {
        base.Death();

        // Custom implementation
        GameplayUI.Instance.ShowDeathScreen();
    }
}
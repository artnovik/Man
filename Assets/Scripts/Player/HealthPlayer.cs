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
        UIGamePlay.Instance.HealthBarValueChange(currentHealth);
    }

    public override void Damage(int damageValue)
    {
        if (!PlayerControl.Instance.isBlock && !Cheats.Instance.GOD_MODE)
        {
            base.Damage(damageValue);

            // Custom implementation
            UIGamePlay.Instance.HealthBarValueChange(currentHealth);
            UIGamePlay.Instance.ActivatePlayerHitScreenEffect();

            EffectsManager.Instance.ActivateBloodEffect(hitTransform);
        }
    }

    protected override void Death()
    {
        base.Death();

        // Custom implementation
        UIGamePlay.Instance.ShowDeathScreen();
    }
}
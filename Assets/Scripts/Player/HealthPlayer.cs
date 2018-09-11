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

    public override void Damage(int damageValue, bool anim = true)
    {
        if (!CheatManager.Instance.GOD_MODE)
        {
                base.Damage(damageValue);

            // Custom implementation
            GameplayUI.Instance.HealthBarValueChange(currentHealth);
            GameplayUI.Instance.ActivatePlayerHitScreenEffect();

            if (locomotion.typeLocomotion != Locomotion.TLocomotion.Block)
            {
                EffectsManager.Instance.ActivateBloodEffect(hitTransform);
            }
            else
            {
                EffectsManager.Instance.ActivateHitBlockEffect(hitTransform);
            }
        }

        SquadData.Instance.GetCurrentCharacter().locomotion.AddSpecialPower();
    }

    protected override void Death()
    {
        base.Death();

        // Custom implementation
        GameplayUI.Instance.ShowDeathScreen();
    }
}
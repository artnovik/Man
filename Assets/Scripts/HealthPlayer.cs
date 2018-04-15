using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPlayer : Health
{
    [SerializeField]
    private Transform hitTransform;

    private void Start()
    {
        StartI();
    }

    public override void Heal(int healValue)
    {
        HealI(healValue);

        // Custom implementation
        UIGamePlay.Instance.HealthBarValueChange(currentHealth);
    }

    public override void Damage(int damageValue)
    {
        if (!PlayerControl.Instance.isBlock && !Cheats.Instance.GOD_MODE)
        {
            DamageI(damageValue);

            // Custom implementation
            UIGamePlay.Instance.HealthBarValueChange(currentHealth);
            UIGamePlay.Instance.ActivatePlayerHitScreenEffect();

            EffectsManager.Instance.ActivateBloodEffect(hitTransform);
        }
    }

    public override void Death()
    {
        DeathI();

        // Custom implementation
        UIGamePlay.Instance.ShowDeathScreen();
    }
}

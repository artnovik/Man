using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPlayer : Health
{
    private void Start()
    {
        StartI();
    }

    public override void Heal(int healValue)
    {
        HealI(healValue);

        UIGamePlay.Instance.HealthBarValueChange(currentHealth);
        // Custom implementation
    }

    public override void Damage(int damageValue)
    {
        if (!PlayerControl.Instance.isBlock && !Cheats.Instance.GOD_MODE)
        {
            DamageI(damageValue);

            UIGamePlay.Instance.HealthBarValueChange(currentHealth);
            // Custom implementation
        }
    }

    public override void Death()
    {
        DeathI();
        // Custom implementation
    }
}

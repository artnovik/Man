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
        // Custom implementation
    }

    public override void Damage(int damageValue)
    {
        DamageI(damageValue);
        // Custom implementation
    }

    public override void Death()
    {
        DeathI();
        // Custom implementation
    }
}

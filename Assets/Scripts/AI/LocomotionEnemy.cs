using UnityEngine;

public class LocomotionEnemy : Locomotion
{
    public override void AttackControl()
    {
        animator.SetTrigger(ATTACK_STATE);
    }

    public override void Attack()
    {
        base.Attack();

        // When Enemy attacks
        if (targetLocomotion && !targetLocomotion.health.isDead)
        {
            targetLocomotion.health.Damage(gameObject.GetComponent<HealthEnemy>().enemyWeaponData.weaponData
                .GetDamage());
        }
    }
}
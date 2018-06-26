using UnityEngine;

public class LocomotionEnemy : Locomotion
{
    private HealthEnemy healthEnemy;

    private void Awake()
    {
        healthEnemy = GetComponent<HealthEnemy>();
    }

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
            if (gameObject.GetComponent<HealthEnemy>().enemyWeaponData)
            {
                targetLocomotion.health.Damage(gameObject.GetComponent<HealthEnemy>().enemyWeaponData.weaponData.GetDamage());
            }
            else
            {
                targetLocomotion.health.Damage(15);
            }
        }

        if (healthEnemy.enemyWeaponData) { healthEnemy.enemyWeaponData.PlaySound(); }
    }
}
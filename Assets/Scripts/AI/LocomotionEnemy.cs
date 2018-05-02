using UnityEngine;

public class LocomotionEnemy : Locomotion
{
    public override void Attack()
    {
        base.Attack();

        // When Enemy attacks
        if (targetLocomotion && !targetLocomotion.health.isDead)
        {
            targetLocomotion.health.Damage(gameObject.GetComponent<HealthEnemy>().weaponObject.weaponData.GetDamage());
        }
    }
}
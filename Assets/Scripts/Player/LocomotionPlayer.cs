using UnityEngine;

public class LocomotionPlayer : Locomotion
{
    public override void AttackControl()
    {
        animator.SetTrigger(PlayerData.Instance.bareHands ? ATACK_HANDS_STATE : ATTACK_STATE);
    }

    public override void Attack()
    {
        // When Player attacks
        PlayerData.Instance.SwitchWeaponColliders();
    }

    public void AttackRightFist()
    {
        PlayerData.Instance.SwitchRightHandCollider();
    }

    public void AttackLeftFist()
    {
        PlayerData.Instance.SwitchLeftHandCollider();
    }

    // ToDo: Squad Menu.
    /*private void OnMouseDown()
    {
        if (gameObject.CompareTag("Player"))
        {
            Debug.Log(gameObject.name);
        }
    }*/
}
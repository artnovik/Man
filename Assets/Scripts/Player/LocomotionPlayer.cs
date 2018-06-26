using UnityEngine;

public class LocomotionPlayer : Locomotion
{
    public int specialPower;

    public override void AttackControl()
    {
        animator.SetTrigger(PlayerData.Instance.bareHands ? ATACK_HANDS_STATE : ATTACK_STATE);
    }

    public override void SpecialAttack()
    {
        if (specialPower < 100) { return; }

        animator.SetTrigger(ATTACK_STATE_SPECIAL);

        specialPower = 0;
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

    public void AddSpecialPower()
    {
        int valuePower = 15;

        specialPower += valuePower;
        Debug.Log(gameObject.name + " added special power " + valuePower);
    }

    // ToDo: Squad Menu.
    private void OnMouseDown()
    {
        if (gameObject.CompareTag("Player"))
        {
            GameplayUI.Instance.CellWindowSquad(true);
            Debug.Log(gameObject.name);
        }
    }
}
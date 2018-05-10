using UnityEngine;

public class LocomotionPlayer : Locomotion
{
    public override void Attack()
    {
        base.Attack();

        // When Player attacks
        PlayerData.Instance.SwitchWeaponColliders();
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
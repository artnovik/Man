using UnityEngine;

public class LocomotionPlayer : Locomotion
{
    public override void Attack()
    {
        base.Attack();

        PlayerData.Instance.SwitchWeaponColliders();
    }

    // ToDo Squad menu
    /*private void OnMouseDown()
    {
        if (gameObject.CompareTag("Player"))
        {
            Debug.Log(gameObject.name);
        }
    }*/
}
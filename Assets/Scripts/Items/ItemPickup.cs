public class ItemPickup : Interactable
{
    public Weapon weaponStats;

    public override void Interact()
    {
        base.Interact();

        PickUp();
    }

    private void PickUp()
    {
        if (Inventory.Instance.weapons.Count >= Inventory.Instance.inventoryCapacity)
        {
            UIGamePlay.Instance.DisplayMessage("Inventory is full.", Colors.redMessage, 2f, false);
            return;
        }

        // Add to Inventory
        weaponStats = GetComponent<WeaponOld>().weaponStats;
        Inventory.Instance.Add(weaponStats);
        Destroy(gameObject);
    }
}
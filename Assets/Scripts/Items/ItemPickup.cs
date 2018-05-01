public class ItemPickup : Interactable
{
    public Item item;

    protected override void Interact()
    {
        base.Interact();

        PickUp();
    }

    private void PickUp()
    {
        if (Inventory.Instance.items.Count >= Inventory.Instance.inventoryCapacity)
        {
            UIGamePlay.Instance.DisplayMessage("Inventory is full.", Colors.redMessage, 2f, false);
            return;
        }

        // Add to Inventory
        
        Inventory.Instance.Add(item);
        Destroy(gameObject);
    }
}
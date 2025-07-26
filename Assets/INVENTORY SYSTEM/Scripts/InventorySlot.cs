// 3. INVENTORY SLOT CLASS
[System.Serializable]
public class InventorySlot
{
    public Item item;
    public int quantity;

    public InventorySlot()
    {
        item = null;
        quantity = 0;
    }

    public InventorySlot(Item newItem, int newQuantity)
    {
        item = newItem;
        quantity = newQuantity;
    }

    public bool IsEmpty()
    {
        return item == null || quantity <= 0;
    }

    public bool CanAddItem(Item itemToAdd, int quantityToAdd)
    {
        if (IsEmpty()) return true;

        return item == itemToAdd &&
               item.isStackable &&
               quantity + quantityToAdd <= item.maxStackSize;
    }

    public void AddItem(Item itemToAdd, int quantityToAdd)
    {
        if (IsEmpty())
        {
            item = itemToAdd;
            quantity = quantityToAdd;
        }
        else if (CanAddItem(itemToAdd, quantityToAdd))
        {
            quantity += quantityToAdd;
        }
    }

    public void RemoveItem(int quantityToRemove)
    {
        quantity -= quantityToRemove;
        if (quantity <= 0)
        {
            item = null;
            quantity = 0;
        }
    }
}
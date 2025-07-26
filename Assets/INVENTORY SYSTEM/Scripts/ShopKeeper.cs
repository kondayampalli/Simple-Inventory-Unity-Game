// 8. SHOP KEEPER
using UnityEngine;

public class ShopKeeper : MonoBehaviour, IInteractable
{
    [Header("Shop Settings")]
    public Item[] shopItems;
    public int[] itemPrices;

    public void Interact(PlayerController player)
    {
        ShopUI.Instance?.OpenShop(this);
    }

    public bool BuyItem(Item item, int quantity = 1)
    {
        int itemIndex = System.Array.IndexOf(shopItems, item);
        if (itemIndex >= 0 && itemIndex < itemPrices.Length)
        {
            int totalCost = itemPrices[itemIndex] * quantity;
            if (InventoryManager.Instance.SpendMoney(totalCost))
            {
                Debug.Log($"Bought {quantity} {item.itemName} for ${totalCost}");
                return InventoryManager.Instance.AddItem(item, quantity);
            }
        }
        return false;
    }

    public bool SellItem(Item item, int quantity = 1)
    {
        return InventoryManager.Instance.SellItem(item, quantity);
    }
}
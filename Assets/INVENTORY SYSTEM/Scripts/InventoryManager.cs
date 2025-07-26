// 4. INVENTORY MANAGER
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [Header("Inventory Settings")]
    public int inventorySize = 20;
    public int playerMoney = 100;

    [Header("Events")]
    public UnityEngine.Events.UnityEvent<Item, int> OnItemAdded;
    public UnityEngine.Events.UnityEvent<Item, int> OnItemRemoved;
    public UnityEngine.Events.UnityEvent<int> OnMoneyChanged;

    private List<InventorySlot> inventory;

    public static InventoryManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeInventory();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeInventory()
    {
        inventory = new List<InventorySlot>();
        for (int i = 0; i < inventorySize; i++)
        {
            inventory.Add(new InventorySlot());
        }
    }

    public bool AddItem(Item item, int quantity = 1)
    {
        if (item == null) return false;

        // Try to stack with existing items first
        if (item.isStackable)
        {
            foreach (var slot in inventory)
            {
                if (slot.CanAddItem(item, quantity))
                {
                    slot.AddItem(item, quantity);
                    OnItemAdded?.Invoke(item, quantity);
                    return true;
                }
            }
        }

        // Find empty slot
        var emptySlot = inventory.FirstOrDefault(slot => slot.IsEmpty());
        if (emptySlot != null)
        {
            emptySlot.AddItem(item, quantity);
            OnItemAdded?.Invoke(item, quantity);
            return true;
        }

        return false; // Inventory full
    }

    public bool RemoveItem(Item item, int quantity = 1)
    {
        var slot = inventory.FirstOrDefault(s => s.item == item && s.quantity >= quantity);
        if (slot != null)
        {
            slot.RemoveItem(quantity);
            OnItemRemoved?.Invoke(item, quantity);
            return true;
        }
        return false;
    }

    public bool SellItem(Item item, int quantity = 1)
    {
        if (RemoveItem(item, quantity))
        {
            AddMoney(item.value * quantity);
            return true;
        }
        return false;
    }

    public void AddMoney(int amount)
    {
        playerMoney += amount;
        OnMoneyChanged?.Invoke(playerMoney);
    }

    public bool SpendMoney(int amount)
    {
        if (playerMoney >= amount)
        {
            playerMoney -= amount;
            OnMoneyChanged?.Invoke(playerMoney);
            return true;
        }
        return false;
    }

    public List<InventorySlot> GetInventory()
    {
        return inventory;
    }

    public int GetItemCount(Item item)
    {
        return inventory.Where(slot => slot.item == item).Sum(slot => slot.quantity);
    }
}
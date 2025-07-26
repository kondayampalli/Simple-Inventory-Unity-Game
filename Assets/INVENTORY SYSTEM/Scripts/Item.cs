using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    [Header("Item Info")]
    public string itemName;
    public string description;
    public Sprite icon;
    public int value;
    public bool isStackable = true;
    public int maxStackSize = 99;

    [Header("Item Type")]
    public ItemType itemType;
}

public enum ItemType
{
    Consumable,
    Equipment,
    Material,
    Misc
}
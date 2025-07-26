// 9. INVENTORY UI (UI TOOLKIT)
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryUI : MonoBehaviour
{
    [Header("UI Documents")]
    public UIDocument inventoryDocument;

    private VisualElement root;
    private VisualElement inventoryPanel;
    private VisualElement inventoryGrid;
    private Label moneyLabel;

    public static InventoryUI Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SetupUI();
        UpdateMoneyDisplay();
        RefreshInventory();

        // Subscribe to inventory events
        InventoryManager.Instance.OnItemAdded.AddListener((item, quantity) => RefreshInventory());
        InventoryManager.Instance.OnItemRemoved.AddListener((item, quantity) => RefreshInventory());
        InventoryManager.Instance.OnMoneyChanged.AddListener(money => UpdateMoneyDisplay());
    }

    private void SetupUI()
    {
        root = inventoryDocument.rootVisualElement;
        inventoryPanel = root.Q("InventoryPanel");
        inventoryGrid = root.Q("InventoryGrid");
        moneyLabel = root.Q<Label>("MoneyLabel");

        // Hide inventory initially
        inventoryPanel.style.display = DisplayStyle.None;

        // Setup close button
        var closeButton = root.Q<Button>("CloseButton");
        closeButton.clicked += () => ToggleInventory();
    }

    public void ToggleInventory()
    {
        bool isVisible = inventoryPanel.style.display == DisplayStyle.Flex;
        inventoryPanel.style.display = isVisible ? DisplayStyle.None : DisplayStyle.Flex;

        // Pause/unpause game
        Time.timeScale = isVisible ? 1f : 0f;

        if (!isVisible)
        {
            RefreshInventory();
        }
    }

    private void RefreshInventory()
    {
        inventoryGrid.Clear();

        var inventory = InventoryManager.Instance.GetInventory();
        for (int i = 0; i < inventory.Count; i++)
        {
            var slot = inventory[i];
            var slotElement = CreateSlotElement(slot, i);
            inventoryGrid.Add(slotElement);
        }
    }

    private VisualElement CreateSlotElement(InventorySlot slot, int index)
    {
        var slotElement = new VisualElement();
        slotElement.AddToClassList("inventory-slot");

        if (!slot.IsEmpty())
        {
            var itemElement = new VisualElement();
            itemElement.AddToClassList("item-icon");

            if (slot.item.icon != null)
            {
                itemElement.style.backgroundImage = new StyleBackground(slot.item.icon);
            }

            var quantityLabel = new Label(slot.quantity.ToString());
            quantityLabel.AddToClassList("quantity-label");

            itemElement.Add(quantityLabel);
            slotElement.Add(itemElement);

            // Add sell button
            var sellButton = new Button(() => SellItem(slot.item, 1));
            sellButton.text = "Sell";
            sellButton.AddToClassList("sell-button");
            slotElement.Add(sellButton);
        }

        return slotElement;
    }

    private void SellItem(Item item, int quantity)
    {
        InventoryManager.Instance.SellItem(item, quantity);
        RefreshInventory();
    }

    private void UpdateMoneyDisplay()
    {
        if (moneyLabel != null)
        {
            moneyLabel.text = $"Money: ${InventoryManager.Instance.playerMoney}";
        }
    }
}
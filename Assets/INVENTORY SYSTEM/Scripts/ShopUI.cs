// 10. SHOP UI (UI TOOLKIT)
using UnityEngine;
using UnityEngine.UIElements;

public class ShopUI : MonoBehaviour
{
    [Header("UI Documents")]
    public UIDocument shopDocument;

    private VisualElement root;
    private VisualElement shopPanel;
    private VisualElement shopGrid;
    private ShopKeeper currentShop;

    public static ShopUI Instance { get; private set; }

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
    }

    private void SetupUI()
    {
        root = shopDocument.rootVisualElement;
        shopPanel = root.Q("ShopPanel");
        shopGrid = root.Q("ShopGrid");

        // Hide shop initially
        shopPanel.style.display = DisplayStyle.None;

        // Setup close button
        var closeButton = root.Q<Button>("CloseButton");
        closeButton.clicked += () => CloseShop();
    }

    public void OpenShop(ShopKeeper shop)
    {
        currentShop = shop;
        shopPanel.style.display = DisplayStyle.Flex;
        Time.timeScale = 0f; // Pause game

        RefreshShop();
    }

    public void CloseShop()
    {
        shopPanel.style.display = DisplayStyle.None;
        Time.timeScale = 1f; // Resume game
        currentShop = null;
    }

    private void RefreshShop()
    {
        shopGrid.Clear();

        if (currentShop == null) return;

        for (int i = 0; i < currentShop.shopItems.Length; i++)
        {
            var item = currentShop.shopItems[i];
            var price = i < currentShop.itemPrices.Length ? currentShop.itemPrices[i] : 0;

            var itemElement = CreateShopItemElement(item, price);
            shopGrid.Add(itemElement);
        }
    }

    private VisualElement CreateShopItemElement(Item item, int price)
    {
        var itemElement = new VisualElement();
        itemElement.AddToClassList("shop-item");

        var nameLabel = new Label(item.itemName);
        var priceLabel = new Label($"${price}");
        var buyButton = new Button(() => BuyItem(item, 1));
        buyButton.text = "Buy";

        itemElement.Add(nameLabel);
        itemElement.Add(priceLabel);
        itemElement.Add(buyButton);

        return itemElement;
    }

    private void BuyItem(Item item, int quantity)
    {
        if (currentShop != null)
        {
            if (currentShop.BuyItem(item, quantity))
            {
                Debug.Log($"Bought {quantity} {item.itemName}");
            }
            else
            {
                Debug.Log("Not enough money or inventory full!");
            }
        }
    }
}
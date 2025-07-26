// 7. COLLECTIBLE ITEM
using UnityEngine;

public class CollectibleItem : MonoBehaviour, IInteractable
{
    [Header("Item Settings")]
    public Item item;
    public int quantity = 1;

    [Header("Visual")]
    public float rotationSpeed = 50f;
    public float bobSpeed = 1f;
    public float bobHeight = 0.5f;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        // Rotate the item
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        // Bob up and down
        float newY = startPosition.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    public void Interact(PlayerController player)
    {
        if (InventoryManager.Instance.AddItem(item, quantity))
        {
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Inventory is full!");
        }
    }
}
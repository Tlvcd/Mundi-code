using UnityEngine;
using Axis.Abstractions;
using Axis.Items;

public class ItemContainer : MonoBehaviour, IInteractable
{
    [SerializeField]
    private Item item;

    [SerializeField] private int amber;

    [SerializeField]
    private InventoryAsset inventory;

    public event System.Action OnPickup;

    public void Interaction()
    {
        inventory.AddItem(item);
        inventory.Amber += amber;
        OnPickup?.Invoke();

        Destroy(gameObject);
    }
    public string GetName() => item.ItemName;

    public bool IsActive() => enabled;
}

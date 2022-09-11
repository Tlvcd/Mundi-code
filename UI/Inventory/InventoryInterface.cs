using Axis.Items;
using System;
using UnityEngine;

[CreateAssetMenu(menuName ="Axis Mundi/Internal/InventoryInterface")]
public class InventoryInterface : ScriptableObject
{
    public ItemSort SelectedItemSort { get; private set; }
    public Item SelectedItem { get { return SelectedItemSort.ItemObject; }}

    public InventoryItemButton CurrentItemContainer { get; private set; }


    public event Action OnSelectedItemChange;

    public void ChangeSelectedContainer(InventoryItemButton butt) => CurrentItemContainer = butt;

    public void ChangeSelectedItem(ItemSort item)
    {
        SelectedItemSort = item;
        OnSelectedItemChange?.Invoke();
    }

}

using Axis.Items;
using System;
using System.Collections.Generic;
using UnityEngine;
using Axis.Abstractions;
using System.Runtime.Serialization;

public class InventoryGearEquipper : MonoBehaviour
{
    [SerializeField]
    InventoryInterface bridge;

    [SerializeField]
    InventoryEquipHandler equipHandler;

    [SerializeField]
    GameObject equipButton, useButton;

    [SerializeField]
    InventoryAsset inventory;


    private ItemSort currentItem;

    private void OnEnable()
    {
        bridge.OnSelectedItemChange += CheckSelectable;
    }

    private void OnDisable()
    {
        bridge.OnSelectedItemChange -= CheckSelectable;
    }

    private void CheckSelectable()
    {
        var item = bridge.SelectedItemSort;

        var itemObject = item.ItemObject;
        equipButton.SetActive((itemObject is Equipable)|| (itemObject is Spell));
        useButton.SetActive((itemObject is HealItem));

        currentItem = item;

    }

    private bool CheckIfEquipped(ItemSort item)
    {
        if (inventory.EquippedItems.Contains(item.ItemObject as Equipable)) return true;

        return false;
    }

    public void Heal()
    {
        if (!(currentItem.ItemObject is HealItem)) return;
        (currentItem.ItemObject as HealItem).Heal();

        inventory.RemoveItem(currentItem.ItemObject);
        bridge.ChangeSelectedItem(inventory.GetItem(currentItem.GetType()));
    }

    public void Equip()
    {
        if(currentItem.ItemObject is Spell)
        {
            EquipSpell();
            return;
        }

        if (!(currentItem.ItemObject is Equipable)) return;

        if (CheckIfEquipped(currentItem))
        {
            equipHandler.UnEquip(currentItem);
            return;
        }
        equipHandler.TryEquip(currentItem);

    }

    private void EquipSpell()
    {
        var spell = currentItem.ItemObject as Spell;

        if (inventory.EquippedSpells.Contains(spell))
        {
            equipHandler.UnEquipSpell(spell);
            return;
        }

        equipHandler.TryEquipSpell(spell);
    }
}

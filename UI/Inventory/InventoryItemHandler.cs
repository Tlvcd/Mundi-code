using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Axis.Items;
using System;
using System.Linq;
using Axis.Abstractions;

public class InventoryItemHandler : MonoBehaviour
{
    List<ItemSort> _cachedItems;

    [SerializeField]
    InventoryAsset inventory;

    [SerializeField]
    private InventoryItemButton itemContainer;

    private Type currentSortType= typeof(Weapon);
    private Type secondarySortType;

    private List<InventoryItemButton> itemContainerPool= new List<InventoryItemButton>();


    private void DebugEquippedItems()
    {
        foreach (var item in inventory.EquippedItems)
        {
            Debug.Log(item);
        }
    }

    private void OnEnable()
    {
        DisplayInventory(currentSortType,secondarySortType);
        inventory.OnInventoryChange += RefreshInv;
    }
    private void OnDisable()
    {
        inventory.OnInventoryChange -= RefreshInv;
    }

    private void RefreshInv()
    {
        DisplayInventory(currentSortType,secondarySortType);
    }



    [ContextMenu("Clear inventory")]
    void ClearInv()
    {
        inventory.ClearInventory();
    }
    /// <summary>
    /// Wyswietla przyciski z przedmiotami w ekwipunku.
    /// </summary>
    public void DisplayInventory(Type type = null, Type type2 = null)
    {
        currentSortType = type;
        secondarySortType = type2;
        if (type == null) _cachedItems = inventory.Inventory;
        else
        {
            _cachedItems = inventory.
                Inventory.Where(
                    x => x.ItemObject.
                        GetType() == type || x.ItemObject.GetType() == type2)
                .ToList();
        }

        foreach (InventoryItemButton button in itemContainerPool)
        {
            button.gameObject.SetActive(false);
        }

        int iteration = 0;
        foreach(var item in _cachedItems)
        {
            InventoryItemButton currCont;
            if (iteration > itemContainerPool.Count - 1)
            {
                var container = Instantiate(itemContainer, transform);
                itemContainerPool.Add(container);
                currCont = container;
            }
            else
            {
                currCont = itemContainerPool[iteration];
                currCont.gameObject.SetActive(true);

            }

            
            currCont.UpdateItemField(item);

            
            iteration++;
            if (!inventory.EquippedItems.Contains(item.ItemObject as Equipable)&& !inventory.EquippedSpells.Contains(item.ItemObject as Spell))
            {
                currCont.ChangeContainerEquippedState(false);
                continue;
            }

            currCont.ChangeContainerEquippedState(true);
            
            
        }

    }

    public void DisplayWeapons() => DisplayInventory(typeof(Weapon));
    public void DisplaySpells() => DisplayInventory(typeof(Spell));
    public void DisplayBoots() => DisplayInventory(typeof(Boots), typeof(Breastplate));
    public void DisplayBreastplate() => DisplayInventory(typeof(Breastplate));

    public void DisplayMisc() => DisplayInventory(typeof(Item), typeof(HealItem));

}



using Axis.Items;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Axis.Abstractions;
using System.Linq;

public class InventoryEquipHandler : MonoBehaviour
{

	[SerializeField]
	InventoryAsset inventory;

	[SerializeField]
	InventoryInterface bridge;

	private void Start()
	{
		//RefreshItemContainers();
	}

	public void UnEquip(ItemSort item)
	{
		if (!item.ItemObject) return;
		var stat = (item.ItemObject as Equipable);

		foreach (var obj in inventory.EquipTypes)
		{
			if (obj.CurrID!= item.GetHashCode()) continue;

			obj.ChangeSlotState(false);
			inventory.EquippedItems.Remove(stat);

			inventory.CountEquippedStatistics();

			bridge.CurrentItemContainer.ChangeContainerEquippedState(false);
			break;
		}

		
	}

	public void UnEquipSpell(Spell spell)
    {
		inventory.EquippedSpells.Remove(spell);
		bridge.CurrentItemContainer.ChangeContainerEquippedState(false);
    }

	public void TryEquipSpell(Spell spell)
    {
		if (inventory.EquippedSpells.Count >= 4) return;

		inventory.EquippedSpells.Add(spell);
		bridge.CurrentItemContainer.ChangeContainerEquippedState(true);
    }


	public void TryEquip(ItemSort obj)
	{
		Type typ = obj.ItemObject.GetType();
		
		foreach(var container in inventory.EquipTypes)
		{
			if (container.ItemType.GetType() != typ || container.Taken) continue;

			//container.UpdateItemField(obj as Item);

			inventory.EquippedItems.Add(obj.ItemObject as Equipable);


			container.ChangeSlotState(true);
			container.ChangeCurrentID(obj.GetHashCode());

			bridge.CurrentItemContainer.ChangeContainerEquippedState(true);
			Debug.Log("hash: "+container.CurrID);
			inventory.CountEquippedStatistics();
			return;
		}

		

	}
}

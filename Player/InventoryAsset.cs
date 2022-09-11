using UnityEngine;
using System.Collections.Generic;
using Axis.Abstractions;
using Axis.Utilities;
using Axis.Items;
using System;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
#endif

namespace Axis.Items
{
    [CreateAssetMenu(fileName = "Inventory asset", menuName = "Axis Mundi/Items/Inventory Asset")]
    public class InventoryAsset : ScriptableObject
#if UNITY_EDITOR
        , IPreprocessBuildWithReport
#endif
    {
        [field: SerializeField]
        public Ekwipunek eq { get; private set; }

        [SerializeField]
        Statistics itemStats, baseStats;

        public event Action OnInventoryChange;
        public event Action<Item> NewItem;

        public List<ItemSort> Inventory => eq.inventoryContainer;
        public List<Equipable> EquippedItems => eq.equippedItemsContainer;
        public List<Spell> EquippedSpells => eq.equippedSpellsContainer;
        public List<Talent> Talents => eq.talentsUnlocked;
        public List<EquipableSlot> EquipTypes => eq.equipableItems;

        public int Amber { get => eq.Amber;
            set { eq.Amber = value; } }



        public void LoadInventory(Ekwipunek inv) 
        { 
            eq = inv;
            OnInventoryChange?.Invoke();
            
        }

        public void AddTalent(Talent obj)
        {
            Talents.Add(obj);
            OnInventoryChange?.Invoke();
        }

        public void RemoveTalent(Talent obj)
        {
            Talents.Remove(obj);
            OnInventoryChange?.Invoke();
        }

        public ItemSort GetItem(Type typ) => Inventory.Find(x => x.ItemObject.GetType() == typ);
        public void CountTalentStats()
        {
            foreach (var item in Talents)
            {
                if (!item.Stat) continue;
                baseStats.AddStatistics(item.Stat);
            }
        }

        public void AddSpell(Spell obj, int index = 0)
        {
            if (EquippedSpells.Contains(obj)) return;

            EquippedSpells.Insert(index, obj);
            obj.ResetCooldown();
            OnInventoryChange?.Invoke();
        }

        public void RemoveSpell(Spell obj) { EquippedSpells.Remove(obj); OnInventoryChange?.Invoke(); }

        public void RemoveSpell(int index) { EquippedSpells.RemoveAt(index); OnInventoryChange?.Invoke(); }

        /// <summary>
        /// Przelicza wszystkie statystyki w ekwipowanych przedmiotach.
        /// </summary>
        /// <returns></returns>
        public Statistics CountEquippedStatistics()
        {
            itemStats.ClearStats();
            foreach (var stat in EquippedItems)
            {
                itemStats.AddStatistics(stat.GetItemStats());
            }

            return itemStats;
        }

        /// <summary>
        /// calkowicie czysci ekwipunek.
        /// </summary>
        [ContextMenu("Clear inventory")]
        public void ClearInventory()
        {
            Inventory.Clear();
            EquippedItems.Clear();
            EquippedSpells.Clear();

            OnInventoryChange?.Invoke();
        }



        /// <summary>
        /// dodaje Item do ekwipunku.
        /// </summary>
        /// <param name="item">ScriptableObject z bazowa klasa Item.</param>
        public void AddItem(Item item, int amount = 1)
        {
        
                for (int i = 0; i < Inventory.Count; ++i)
                {
                    if (Inventory[i].ItemObject != item) continue;
        
                    Inventory[i] = new ItemSort(item, Inventory[i].Amount + amount);
        
                    OnInventoryChange?.Invoke();
                    NewItem?.Invoke(item);
                    return;
                }
                
            var sortedItem = new ItemSort(item, amount);
            
            Inventory.Add(sortedItem);
            NewItem?.Invoke(item);

            OnInventoryChange?.Invoke();
        }

        /// <summary>
        /// Usuwa przedmiot z ekwipunku, jezeli istnieje.
        /// </summary>
        /// <param name="item">Item ze ScriptableObject</param>
        /// <param name="amount">ilosc do usuniecia, 1 to default</param>
        /// <returns>bool mowiacy czy zostal usuniety</returns>
        public bool RemoveItem(Item item, int amount = 1)
        {
            if (amount == 0) return true;
            for (int i = 0; i < Inventory.Count; ++i)
            {
                var currIndex = Inventory[i];
                if (currIndex.ItemObject != item) continue;

                if (currIndex.Amount - amount < 0) return false;

                if (currIndex.Amount - amount == 0)
                {
                    Inventory.RemoveAt(i);
                    OnInventoryChange?.Invoke();
                    return true;
                }

                Inventory[i] = new ItemSort(item, currIndex.Amount - amount);
                OnInventoryChange?.Invoke();
                return true;
            }
            return false;
        }

        /// <summary>
        /// usun item z listy na podanym indexie
        /// </summary>
        /// <param name="index"></param>
        public void RemoveItemAtIndex(int index)
        {
            if (Inventory.Count < index) return;

            Inventory.RemoveAt(index);
            OnInventoryChange?.Invoke();
           
        }
        
        
        private void ClearEquipped()
        {
            eq.equippedItemsContainer.Clear();
        }

#if UNITY_EDITOR
        #region onBuild
        public int callbackOrder => 50;

        public void OnPreprocessBuild(BuildReport report)
        {
            eq = new Ekwipunek();
            
        }
        #endregion
#endif
    }


    [Serializable]
    public class Ekwipunek
    {
        public List<ItemSort> inventoryContainer = new List<ItemSort>();
        
        public List<Equipable> equippedItemsContainer = new List<Equipable>();
        
        public List<Spell> equippedSpellsContainer = new List<Spell>();
        
        public List<Talent> talentsUnlocked = new List<Talent>();

        public List<EquipableSlot> equipableItems = new List<EquipableSlot>();

        [field: SerializeField]
        public int Amber { get; set; }

        
    }

    [Serializable]
    public class EquipableSlot
    {
        public Item ItemType;

        [field:SerializeField]
        public int CurrID { get; private set; }

        [field: SerializeField]
        public bool Taken { get; private set; }

        public void ChangeSlotState(bool state) => Taken = state;

        public void ChangeCurrentID(int id) => CurrID = id;
    }
    [Serializable]
    public struct ItemSort
    {
        public Item ItemObject;
        public int Amount;

        public ItemSort(Item item, int amount)
        {
            this.ItemObject = item;
            this.Amount = amount;
        }

    }
}

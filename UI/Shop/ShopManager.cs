using Axis.Items;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{

    ShopAsset shop;

    [SerializeField]
    InventoryAsset playerInventory;

    [SerializeField]
    ShopItemContainer prefabContainer;

    [SerializeField]
    InventoryInterface selectionInterface;

    ShopItemContainer[] _cachedContainer;

    [SerializeField]
    Transform layout;

    [SerializeField]
    CanvasGroup group;

    [SerializeField]
    Button BuyButton;

    [SerializeField]
    ModalWindow prompt;

    private Dictionary<ShopEntry, int> _cachedEntries = new Dictionary<ShopEntry, int>();

    [SerializeField]
    UnityEvent OnBuy;

    public void InitShop(ShopAsset shop)
    {
        gameObject.SetActive(true);
        LeanTween.alphaCanvas(group, 1, 0.25f).setEaseOutCirc().setIgnoreTimeScale(true);
        Time.timeScale = 0;

        this.shop = shop;

        ClearContainers();

        int length = shop.Entries.Count;
        _cachedContainer = new ShopItemContainer[length];
        _cachedEntries.Clear();

        for (int i = 0; i < length; i++)
        {
            var item = shop.Entries[i];

            var butt = Instantiate(prefabContainer, layout);
            butt.UpdateContainer(item);
            butt.OnSelect = OnSelection;

            _cachedContainer[i] = butt;
            _cachedEntries.Add(item, i);
        }

    }

    private void ClearContainers()
    {
        if (_cachedContainer == null) return;

        foreach (var item in _cachedContainer)
        {
            Destroy(item.gameObject);
        }
    }

    ShopEntry currentEntry;
    private void OnSelection(ShopEntry obj)
    {
        selectionInterface.ChangeSelectedItem(new ItemSort(obj.ItemObject, obj.Amount));
        currentEntry = obj;
        if(playerInventory.Amber < currentEntry.Price)
        {
            BuyButton.interactable = false;
            return;
        }

        BuyButton.interactable = true;
        //BuyItem(obj);
    }

    private void DisplayPrompt(string title, string desc, Action act)
    {
        var window = Instantiate(prompt, transform.parent);
        window.PopulateButtons("Tak", "Powrót");
        window.PopulateButtonActions(act);
        window.SetTitle(title);
        window.SetDescription(desc);
    }

    public void BuyItemButton()
    {
        DisplayPrompt("Zakup", 
            $"Czy chcesz kupi? {currentEntry.Amount}x" +
            $" {currentEntry.ItemObject.ItemName}" +
            $" za {currentEntry.Price} bursztynów?",
            delegate { BuyItem(currentEntry); }
            ) ;
    }

    private void BuyItem(ShopEntry obj)
    {
        if (playerInventory.Amber < obj.Price) return;

        playerInventory.Amber -= obj.Price;
        playerInventory.AddItem(obj.ItemObject, obj.Amount);

        Debug.Log($"bought {obj.ItemObject}!");
        OnBuy.Invoke();
        OnSelection(currentEntry);

        ClearItemFromShop(obj); 
        
    }

    private void ClearItemFromShop(ShopEntry obj)
    {
        if (!obj.CalculateStorage()) return;

        int index = _cachedEntries[obj];

        Destroy(_cachedContainer[index].gameObject);

        shop.Entries.RemoveAt(index);
    }
}

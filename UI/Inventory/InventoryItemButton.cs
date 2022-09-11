using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Axis.Items;
using UnityEngine.EventSystems;
using System;
using System.Linq;
using Axis.Utilities;


public class InventoryItemButton : MonoBehaviour, ISelectHandler
{
    [SerializeField]
    InventoryInterface bridge;

    [SerializeField] 
    TMP_Text itemNameDisplay;

    [SerializeField] 
    Image itemSpriteDisplay, backdrop, equipBG;

    [SerializeField] 
    Button _button;

    [SerializeField] private List<GameObject> spriteIcons;

    private Color32 _defaultCol = new Color32(0,0,0,128);

    ItemSort storedItem;
    public bool Empty { get; private set; } = true;

    [field: SerializeField]
    public bool Equipped { get; private set; }
    public void ChangeContainerEquippedState(bool state)
    {

         Equipped = state;
         equipBG.gameObject.SetActive(state);
    }

    private void Awake()
    {
        _button.onClick.AddListener(() => OnButton());
    }

    /// <summary>
    /// ustawia kontener itemu na pusty.
    /// </summary>
    public void BlankItemField()
    {
        storedItem = new ItemSort();
        Empty = true;

        itemNameDisplay.text = string.Empty;
        itemSpriteDisplay.sprite = null;
        itemSpriteDisplay.enabled = false;

        backdrop.color = _defaultCol;

        TurnOffSpriteIcons();
    }

    private void TurnOffSpriteIcons(int index=-1) //enables correct icon for item type, disables all when -1
    {
        for (int i = 0; i < 4; i++)
        {
            if (i == index)
            {
                spriteIcons[i].SetActive(true);
                continue;
            }

            spriteIcons[i].SetActive(false);
        }
    }

    /// <summary>
    /// Odswieza wartosci kontenera itemu w UI.
    /// </summary>
    /// <param name="item"></param>
    public void UpdateItemField(ItemSort item)
    {

        storedItem = item;
        Empty = false;

        itemNameDisplay.text = item.ItemObject.ItemName;

        var itemColor = item.ItemObject
            .GetColorByRarity()
            .ModifyAlpha(0.5f);

        backdrop.color = itemColor;

        TurnOffSpriteIcons(ReturnSpriteIndex(item.ItemObject.GetType()));

        itemSpriteDisplay.sprite = item.ItemObject.Sprite;
        itemSpriteDisplay.enabled = true;
    }

    public void UpdateItemField(Item item) 
    {
        storedItem = new ItemSort(item,1);
        Empty = false;

        itemNameDisplay.text = item.ItemName;

        var itemColor = item
            .GetColorByRarity()
            .ModifyAlpha(0.5f);

        backdrop.color = itemColor;

        TurnOffSpriteIcons(ReturnSpriteIndex(item.GetType()));

        itemSpriteDisplay.sprite = item.Sprite;
        itemSpriteDisplay.enabled = true;
    }

    private void OnButton()
    {
        bridge.ChangeSelectedItem(storedItem);
        bridge.ChangeSelectedContainer(this);
    }

    void ISelectHandler.OnSelect(BaseEventData eventData)
    {
        OnButton();
    }


    private static readonly Dictionary<Type, int> itemTypesIndexary = new Dictionary<Type, int>
    {
        { typeof(Weapon), 0},
        { typeof(Breastplate), 1},
        { typeof(Boots), 1},
        { typeof(Spell), 2},
        { typeof(Item), 3}
    };
    private int ReturnSpriteIndex(Type typ)
    {
        if (itemTypesIndexary.TryGetValue(typ, out var value))
        {
            return value;
        }

        return 3;
    }
}

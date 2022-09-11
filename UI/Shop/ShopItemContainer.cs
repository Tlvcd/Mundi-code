using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class ShopItemContainer : MonoBehaviour
{

    private ShopEntry entry;

    [SerializeField]
    Image itemImage;

    [SerializeField]
    TMP_Text itemName, itemCount, itemPrice;

    [SerializeField]
    Button containerButton;

    public Action<ShopEntry> OnSelect;

    private void Awake()
    {
        containerButton.onClick.AddListener(() => OnSelect.Invoke(entry));
    }

    public void UpdateContainer(ShopEntry obj)
    {
        entry = obj;
        var itemAsset = obj.ItemObject;
        itemImage.sprite = itemAsset.Sprite;
        itemName.text = itemAsset.ItemName;
        itemCount.text = obj.Amount.ToString();
        itemPrice.text = obj.Price.ToString();
    }


}

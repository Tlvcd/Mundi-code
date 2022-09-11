using UnityEngine;
using System;

[CreateAssetMenu(fileName ="ShopInterface", menuName ="Axis Mundi/Internal/Shop interface")]
public class ShopInterface : ScriptableObject
{
    public void OpenShop(ShopAsset shop)
    {
        CurrentShop = shop;
        OnShopOpen?.Invoke(shop);
    }

    public ShopAsset CurrentShop { get; private set; }

    public Action<ShopAsset> OnShopOpen;

}

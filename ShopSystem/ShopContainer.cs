using UnityEngine;

public class ShopContainer : MonoBehaviour
{
    [SerializeField]
    ShopAsset shop;

    [SerializeField]
    ShopInterface reciever;

    public void OpenShop()
    {
        reciever.OpenShop(shop);
    }

}

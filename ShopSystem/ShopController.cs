using UnityEngine;

public class ShopController : MonoBehaviour
{
    [SerializeField]
    ShopManager manager;

    [SerializeField]
    ShopInterface sender;

    private void OnEnable()
    {
        sender.OnShopOpen += manager.InitShop;
    }

    private void OnDisable()
    {
        sender.OnShopOpen -= manager.InitShop;
    }

}

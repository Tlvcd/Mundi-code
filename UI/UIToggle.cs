using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIToggle : MonoBehaviour
{
    PlayerInputs _input;
    [SerializeField]
    CanvasGroup inventory, shop;

    private void Awake()
    {
        _input = PlayerInputManagerClass.GetInputClass();
    }

    private void OnEnable()
    {
        _input.UI.Escape.performed += ToggleInventoryUI;
    }

    private void OnDisable()
    {
        _input.UI.Escape.performed -= ToggleInventoryUI;
    }


    private void ToggleInventoryUI(InputAction.CallbackContext obj) => OpenInv(); //just for these pesky callbackcontext from input system. :(

    private bool debounce;
    public void OpenInv()
    {
        if (debounce) return;
        StartDebounce();
        if (shop.gameObject.activeInHierarchy)
        {
            LeanTween.alphaCanvas(shop, 0, 0.25f)
                .setEaseOutCirc()
                .setIgnoreTimeScale(true)
                .setOnComplete(() => { shop.gameObject.SetActive(false); });
            PlayerInputManagerClass.EnablePlayerInput();

            Time.timeScale = 1;
            return;
        }

        if (inventory.gameObject.activeInHierarchy)
        {
            LeanTween.alphaCanvas(inventory, 0, 0.25f)
                .setEaseOutCirc()
                .setIgnoreTimeScale(true)
                .setOnComplete(() => { inventory.gameObject.SetActive(false); });

            Time.timeScale = 1;
            PlayerInputManagerClass.EnablePlayerInput();
            return;
            
        }

        PlayerInputManagerClass.DisablePlayerInput();
        inventory.gameObject.SetActive(true);

        LeanTween.alphaCanvas(inventory, 1, 0.25f)
            .setIgnoreTimeScale(true)
            .setEaseOutCirc();

        Time.timeScale = 0;
    }

    private async void StartDebounce()
    {
        debounce = true;
        await Task.Delay(500);
        debounce = false;
    }
}

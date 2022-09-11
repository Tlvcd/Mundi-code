using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ButtonBindAction : MonoBehaviour
{
    [SerializeField] private InputAction bind;
    [SerializeField] private UnityEvent onClick;

    private static bool debounce;
    private void OnEnable()
    {
        bind.started += InvokeAction;
        bind.Enable();
    }

    private void OnDisable()
    {
        bind.started -= InvokeAction;
        bind.Disable();
    }

    private void InvokeAction(InputAction.CallbackContext obj)
    {
        if (debounce) return;
        onClick.Invoke();

        StartDebounce();

    }

    private async void StartDebounce()
    {
        debounce = true;
        await Task.Delay(300);
        debounce = false;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuQuitPrompt : MonoBehaviour
{

    [SerializeField]
    ModalWindow prompt;

    [SerializeField]
    private UnityEvent onQuit, onCancel;
    private void DisplayPrompt(string title, string desc, Action act)
    {
        var window = Instantiate(prompt, transform.parent);
        window.PopulateButtons("Tak", "Powrót");
        window.PopulateButtonActions(act, ()=>onCancel.Invoke());
        window.SetTitle(title);
        window.SetDescription(desc);
    }

    public void QuitPrompt()
    {
        DisplayPrompt("Wyjść do Menu?", "Czy chcesz wrócić do menu głównego? Twój postęp zostanie zapisany.",()=> onQuit.Invoke());
    }

}

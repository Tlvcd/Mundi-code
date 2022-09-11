using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ModalWindow : MonoBehaviour
{
    [SerializeField] private GameObject innerWindow;

    [SerializeField]
    InteractableButton buttonPrefab;
    List<InteractableButton> activeButtons = new List<InteractableButton>();

    [SerializeField]
    Transform buttonContainer;
    [SerializeField]
    CanvasGroup group;

    [SerializeField]
    TMP_Text titleText, descText;

    private void Awake()
    {
        innerWindow.transform.localPosition = new Vector3(0,-20,0);
        LeanTween.alphaCanvas(group, 1, 0.125f).setEaseInCubic().setIgnoreTimeScale(true);
        LeanTween.moveLocalY(innerWindow, 0, 0.125f).setEaseOutCubic().setIgnoreTimeScale(true);

    }

    /// <summary>
    /// ile ma byc stworzonych przyciskow w oknie, od lewej do prawej.
    /// </summary>
    /// <param name="buttonNames"></param>
    public void PopulateButtons(params string[] buttonNames)
    {
        if (activeButtons.Count > 0) return;

        if (buttonNames.Length == 0)
        {
            CreateNewButton("Ok");
            return;
        }

        foreach (var item in buttonNames)
        {
            CreateNewButton(item);
        }
    }

    private void CreateNewButton(string name)
    {
        var button = Instantiate(buttonPrefab, buttonContainer);
        button.SetButtonLabel(name);
        activeButtons.Add(button);
        button.OnInteractFunction = CloseWindow;
    }

    public void ChangeButtonColors(params Color[] colors)
    {
        for (int i = 0; i < activeButtons.Count; i++)
        {
            activeButtons[i].SetButtonLabelColor(colors[i]);
        }
    }

    public void PopulateButtonActions(params Action[] actions)
    {
        for (int i = 0; i < activeButtons.Count; i++)
        {
            if (actions.Length <= i) break;
            activeButtons[i].OnInteractFunction += actions[i];
        }
    }

    public void SetTitle(string title) => titleText.text = title;

    public void SetDescription(string desc) => descText.text = desc;

    public void CloseWindow()
    {
        LeanTween.moveLocalY(innerWindow, -10, 0.14f).setEaseOutCubic().setIgnoreTimeScale(true);
        LeanTween.alphaCanvas(group, 0, 0.125f).setEaseInCubic().setIgnoreTimeScale(true)
            .setOnComplete(() => Destroy(this.gameObject));
    }

}

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class InteractableButton : MonoBehaviour
{
    [SerializeField] Button mainButton;
    [SerializeField] TextMeshProUGUI buttonLabel;



    public Action OnInteractFunction;

    private void Awake()
    {
        mainButton.onClick.AddListener(
            TriggerEvent
        );
    }

    public void TriggerEvent() => OnInteractFunction?.Invoke();

    public void SetButtonLabel(string label)
    {
        buttonLabel.text = label;
    }

    public void SetButtonLabelColor(Color col)
    {
        buttonLabel.color = col;
    }

}

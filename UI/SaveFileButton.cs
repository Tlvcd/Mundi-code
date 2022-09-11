using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveFileButton : MonoBehaviour
{
    [SerializeField] Button mainButton, deleteButton;
    [SerializeField] TextMeshProUGUI buttonLabel;



    public Action OnInteractFunction;
    public Action OnDeleteInteraction;

    private void Start()
    {
        mainButton.onClick.AddListener(
            () => OnInteractFunction?.Invoke()
        );
        deleteButton.onClick.AddListener(
            () => OnDeleteInteraction?.Invoke()
        );
    }

    public void SetButtonLabel(string label)
    {
        buttonLabel.text = label;
    }



}

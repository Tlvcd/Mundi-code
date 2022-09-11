using Axis.Items;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AmberDisplay : MonoBehaviour
{
    [SerializeField]
    InventoryAsset inv;
    [SerializeField]
    TMP_Text textAmount;

    private void OnEnable()
    {
        UpdateCount();
        inv.OnInventoryChange += UpdateCount;
    }
    private void OnDisable()
    {
        inv.OnInventoryChange -= UpdateCount;
    }

    private void UpdateCount()
    {
        textAmount.text = inv.Amber.ToString();
    }
}

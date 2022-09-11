using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Axis.Items;
using System;
using UnityEngine.UI;

public class InventoryDescriptor : MonoBehaviour
{

    [SerializeField]
    TMP_Text itemDescription, itemName, itemAmount;

    [SerializeField] private Image itemSprite, colBg1, colBg2;

    [SerializeField]
    InventoryInterface bridge;

    private CanvasGroup groupObj;

    private StatsInspector statInspector;

    private void Awake()
    {
        statInspector = GetComponent<StatsInspector>();
        groupObj = GetComponent<CanvasGroup>();
        bridge.OnSelectedItemChange += DisplayItem;

        gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        bridge.OnSelectedItemChange -= DisplayItem;
    }
    private void OnDisable()
    {
        groupObj.alpha = 0;
        gameObject.SetActive(false);
    }

    private void DisplayItem()
    {
        

        var obj = bridge.SelectedItemSort;
        var item = obj.ItemObject;
        
        if (!gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
            LeanTween.alphaCanvas(groupObj, 1, 0.5f).setIgnoreTimeScale(true);
        }

        if (item==null)
        {
            LeanTween.alphaCanvas(groupObj, 0, 0.5f).setEaseInCirc().setIgnoreTimeScale(true).setOnComplete(() => { gameObject.SetActive(false); });
            return;
        }



        statInspector.DisableModules();

        

        itemDescription.text = item.Description;
        itemName.text = item.ItemName;
        itemAmount.text = "x"+obj.Amount.ToString();

        itemSprite.sprite = item.Sprite;


        var color = item.GetColorByRarity();
        colBg1.color = color;
        colBg2.color = color;


        var equipable = item as Equipable;
        if (equipable && equipable.statTable!=null)
        {
            statInspector.DisplayStatistics(equipable.GetItemStats());
        }

    }
}

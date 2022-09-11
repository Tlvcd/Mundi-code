using System;
using System.Collections;
using System.Collections.Generic;
using Axis.Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemPickupPopup : MonoBehaviour
{
    [SerializeField] private InventoryAsset inventory;
    [SerializeField] private Transform leanPoint, defaultPos;

    [SerializeField]
    private TMP_Text title, desc;

    [SerializeField]
    private Image img;

    private Queue<Item> collectionQueue= new Queue<Item>();

    private Item currentPopup;

    

    private void OnEnable()
    {
        inventory.NewItem += DisplayPopup;
    }

    private void OnDisable()
    {
        inventory.NewItem -= DisplayPopup;
    }

    private void DisplayPopup(Item obj)
    {
        if (!obj)
        {
            Recurrence();
            return;
        }

        if (currentPopup != null)
        {
            if (currentPopup== obj || collectionQueue.Contains(obj)) return;

            collectionQueue.Enqueue(obj);
            return;
        }

        currentPopup = obj;

        title.text = obj.ItemName;
        desc.text = obj.Description;
        img.sprite = obj.Sprite;

        LeanTween.move(gameObject, leanPoint, 0.5f).setEaseInOutCirc().setOnComplete(() =>
        {
            LeanTween.move(gameObject, defaultPos, 0.5f)
                .setEaseInOutCirc()
                .setDelay(2f)
                .setOnComplete(Recurrence);
        });





    }

    private void Recurrence()
    {
        currentPopup = null;
        if (collectionQueue.Count > 0)
        {
            DisplayPopup(collectionQueue.Dequeue());
        }

    }
}

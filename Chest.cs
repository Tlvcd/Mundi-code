
using Axis.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Chest : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer rend, openChest;

    [SerializeField] private List<Item> objects;
    [SerializeField] private int money;

    [SerializeField] private InventoryAsset pInv; 

    private void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    public void DropItems()
    {
        rend.enabled = false;
        openChest.gameObject.SetActive(true);

        pInv.Amber += money;

        foreach (var obj in objects)
        {
            if (!obj) continue;

            pInv.AddItem(obj);
        }

        StartCoroutine(FadeChest());

        IEnumerator FadeChest()
        {
            var timer = 5f;
            while (timer > 0f)
            {
                openChest.color = new Color(1, 1, 1, timer / 5);
                timer -= Time.deltaTime;
                yield return null;
            }

            gameObject.SetActive(false);
        }
    }


}

using Axis.Items;
using System.Collections.Generic;
using UnityEngine;

public class TalentManager : MonoBehaviour
{

    [SerializeField]
    InventoryAsset inventory;

    [SerializeField]
    Statistics basePlayer;

    [SerializeField]
    Item currency;

    List<Talent> activeTalents = new List<Talent>();

    public bool TryUnlockTalent(Talent obj, int price)
    {
        if (!inventory.RemoveItem(currency, price))
        {
            Debug.Log("Not enough currency");
            return false;
        }



        if (obj.Stat) { basePlayer.AddStatistics(obj.Stat); }

        if (obj.Reward) { inventory.AddItem(obj.Reward); }

        activeTalents.Add(obj);

        return true;
    }

    public void LoadTalents()
    {
        foreach (var item in activeTalents)
        {
            basePlayer.AddStatistics(item.Stat);
        }
    }

}

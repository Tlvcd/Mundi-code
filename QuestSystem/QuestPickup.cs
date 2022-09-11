using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class QuestPickup : QuestHandle
{

    [SerializeField]
    private List<ItemContainer> collectibles = new List<ItemContainer>();

    private int amount;


    protected override void HandleAwake()
    {
        collectibles.ForEach(x => x.OnPickup+=CompleteObjective);
    }

    protected override void Init()
    {
        base.Init();
        amount = 0;

    }

    private void CompleteObjective()
    {
        if (!this.enabled && !Retroactive) return;
        amount++;
        if (amount >= collectibles.Count) SendCompletion();
    }


    

}

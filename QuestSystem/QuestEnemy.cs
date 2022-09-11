using Axis.Abstractions;
using System.Collections.Generic;
using UnityEngine;

public class QuestEnemy : QuestHandle
{

    [SerializeField]
    private List<BaseEnemy> enemies = new List<BaseEnemy>();

    private int amount;


    protected override void HandleAwake()
    {
        enemies.ForEach(x => x.OnDeath += delegate { CompleteObjective(); });
        
    }

    protected override void Init()
    {
        base.Init();
        amount = 0;

    }

    private void CompleteObjective()
    {
        if (!this.enabled &&!Retroactive) return;
        amount++;

        if (amount >= enemies.Count) SendCompletion();


    }

}

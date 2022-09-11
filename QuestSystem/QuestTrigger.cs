using UnityEngine;

public class QuestTrigger : QuestHandle
{
    [SerializeField]
    private int amount=1;

    private int currAmount = 0;

    protected override void Init()
    {
        base.Init();
        currAmount = 0;
    }

    public void TriggerComplete()
    {
        if (!this.enabled && !Retroactive) return;

        currAmount++;

        if (currAmount >= amount) SendCompletion();
    }

}

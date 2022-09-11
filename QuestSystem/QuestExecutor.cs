using UnityEngine;
using UnityEngine.Events;

public class QuestExecutor : QuestHandle
{
    [SerializeField]
    private UnityEvent OnEnable;

    [SerializeField]
    private UnityEvent OnDisable;

    protected override void Init()
    {
        base.Init();
        OnEnable.Invoke();
    }

    public override void DeInit()
    {
        base.DeInit();
        OnDisable.Invoke();
    }

}

using System.Collections;
using System.Collections.Generic;
using Axis.Abstractions;
using UnityEngine;

public class BerserkStunned : StateClass<BerserkEnemy>
{
    public override void OnStateEnter(BerserkEnemy obj)
    {
        obj.PlayAnimation(AnimClip.Idle);
        Debug.Log("stunned");
        obj.InvokeRoutine(delegate { obj.ChangeCurrentState(obj.States.Avoid); }, 2f);
    }

    public override void OnStateExit()
    {

    }

    public override void OnStateUpdate()
    {
        return;
    }

}

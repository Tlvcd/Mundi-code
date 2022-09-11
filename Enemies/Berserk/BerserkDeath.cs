using System.Collections;
using System.Collections.Generic;
using Axis.Abstractions;
using UnityEngine;

public class BerserkDeath : StateClass<BerserkEnemy>
{
    public override void OnStateEnter(BerserkEnemy obj)
    {
        obj.Path.maxSpeed = 0;
        obj.Path.enabled = false;

        obj.PlayAnimation(AnimClip.Death);
        obj.InvokeRoutine(obj.DestroySelf, 4f);

    }

    public override void OnStateExit()
    {

    }

    public override void OnStateUpdate()
    {
        return;
    }

}

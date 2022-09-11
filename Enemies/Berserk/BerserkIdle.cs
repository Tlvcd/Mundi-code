using System.Collections;
using System.Collections.Generic;
using Axis.Abstractions;
using UnityEngine;

public class BerserkIdle : StateClass<BerserkEnemy>
{
    public override void OnStateEnter(BerserkEnemy obj)
    {
        obj.PlayAnimation(AnimClip.Idle);
    }

    public override void OnStateExit()
    {

    }

    public override void OnStateUpdate()
    {
        return;
    }

}

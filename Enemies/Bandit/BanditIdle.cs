using System.Collections;
using System.Collections.Generic;
using Axis.Abstractions;
using UnityEngine;

public class BanditIdle : StateClass<BanditEnemy>
{
    private BanditEnemy manager;

    public override void OnStateEnter(BanditEnemy obj)
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

using System.Collections;
using System.Collections.Generic;
using Axis.Abstractions;
using UnityEngine;

public class UtopiecIdle : StateClass<UtopiecEnemy>
{
    private UtopiecEnemy manager;

    public override void OnStateEnter(UtopiecEnemy obj)
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

using System.Collections;
using System.Collections.Generic;
using Axis.Abstractions;
using UnityEngine;

public class DeathState : StateClass<PlayerMovementStateManager>
{
    
    public override void OnStateEnter(PlayerMovementStateManager obj)
    {
        obj.PlayerAnims.PlayAnimation(AnimClip.Death);
        obj.PlayerStateObject.IsPerformingAction = true;
        obj.Inputs.Disable();
    }

    public override void OnStateExit()
    {
        
    }

    public override void OnStateUpdate()
    {
        
    }
}

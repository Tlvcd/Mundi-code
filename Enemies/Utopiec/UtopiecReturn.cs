using System.Collections;
using System.Collections.Generic;
using Axis.Abstractions;
using UnityEngine;

public class UtopiecReturn : StateClass<UtopiecEnemy>
{
    private UtopiecEnemy manager;

    public override void OnStateEnter(UtopiecEnemy obj)
    {
        manager = obj;
        manager.Path.destination = manager.SpawnPos;
    }

    public override void OnStateExit()
    {
        return;
    }

    public override void OnStateUpdate()
    {

        manager.PlayAnimation(AnimClip.Walk);
        if (manager.Path.reachedDestination)
        {
            manager.RestoreHealth();
            manager.ChangeCurrentState(manager.States.Idle);
        }
    }
    
}

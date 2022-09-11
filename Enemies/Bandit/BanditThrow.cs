using System.Collections;
using System.Collections.Generic;
using Axis.Abstractions;
using UnityEngine;

public class BanditThrow : StateClass<BanditEnemy>
{

    public override void OnStateEnter(BanditEnemy obj)
    {
        obj.StopMovement();

        obj.AttackParticles.Play();
        obj.InvokeRoutine(delegate
        {
            obj.FacePlayer();
            obj.PlayAnimation(AnimClip.Attack);
            obj.ThrowKnife();
            obj.ThrowCooldown();
        }, .5f);
        

        obj.InvokeRoutine(delegate { obj.ChangeCurrentState(obj.States.Avoid);  }, 3f);

    }

    public override void OnStateExit()
    {

    }

    public override void OnStateUpdate()
    {
        return;
    }

}

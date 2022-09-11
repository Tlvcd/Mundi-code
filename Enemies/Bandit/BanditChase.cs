using System.Collections;
using System.Collections.Generic;
using Axis.Abstractions;
using UnityEngine;

public class BanditChase : StateClass<BanditEnemy>
{
    private BanditEnemy manager;

    private Transform playerTrans;

    public override void OnStateEnter(BanditEnemy obj)
    {
        manager = obj;
        playerTrans = obj.Range.PlayerTransform;


    }

    public override void OnStateExit()
    {

    }

    public override void OnStateUpdate()
    {
        float radius = 0.7f;
        var position = playerTrans.position;
        var selfPos = manager.transform.position;

        Vector3 direction = (selfPos - position).normalized;

        Vector3 finalPosition = position + direction * radius;

        manager.Path.destination = finalPosition;

        var distanceToPlayer = Vector3.Distance(selfPos, position);

        if (distanceToPlayer <= 1.5f)
        {
            manager.ChangeCurrentState(manager.States.Melee);
        }

        if (distanceToPlayer >= 10f)
        {
            manager.ChangeCurrentState(manager.States.Throw);
        }

        manager.PlayAnimation(AnimClip.Walk);
    }

}

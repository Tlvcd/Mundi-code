using System.Collections;
using System.Collections.Generic;
using Axis.Abstractions;
using UnityEngine;


// ReSharper disable once CheckNamespace
// ReSharper disable once IdentifierTypo
public class UtopiecChase : StateClass<UtopiecEnemy>
{
    private UtopiecEnemy manager;
    private Transform playerPos;
    public override void OnStateEnter(UtopiecEnemy obj)
    {
        manager = obj;

        playerPos = manager.Range.PlayerObj.transform;


    }

    public override void OnStateExit()
    {
        
    }

    public override void OnStateUpdate()
    {
        float radius = 0.7f;
        var position = playerPos.position;
        var selfPos = manager.transform.position;
        Vector3 direction = (selfPos- position).normalized;

        Vector3 finalPosition = position + direction * radius;

        manager.Path.destination = finalPosition;

        var distanceToPlayer = Vector3.Distance(selfPos, position);

        if (distanceToPlayer<=1.5f)
        {
            manager.ChangeCurrentState(manager.States.Attack);
        }

        if (distanceToPlayer >= 5f)
        {
            manager.ChangeCurrentState(manager.States.Avoid);
        }

        manager.PlayAnimation(AnimClip.Walk);

    }

}

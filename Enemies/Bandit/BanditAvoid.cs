using System.Collections;
using System.Collections.Generic;
using Axis.Abstractions;
using UnityEngine;

public class BanditAvoid : StateClass<BanditEnemy>
{
    private BanditEnemy manager;
    private Transform playerPos;

    int attemptCounter=0;
    public override void OnStateEnter(BanditEnemy obj)
    {
        manager = obj;
        playerPos = manager.Range.PlayerObj.transform;
        attemptCounter++;
        if (attemptCounter >= 3)
        {
            manager.ChangeCurrentState(manager.States.Chase);
            attemptCounter = 0;
        }

        manager.InvokeRoutine(
            delegate
            {
                manager.ChangeCurrentState(manager.States.Throw);
            },
            Random.Range(5, 10));
    }

    public override void OnStateExit()
    {

    }

    public override void OnStateUpdate()
    {

        
        
        var position = playerPos.position;
        Vector3 direction = (manager.transform.position - position).normalized;

        var strafeDir = Quaternion.Euler(0, 0, 45) * direction;

        Vector3 finalPosition = position + direction * 6 + (strafeDir * 0.35f);

        manager.Path.destination = finalPosition;


        var distance = Vector3.Distance(manager.transform.position, playerPos.position);
        if (!manager.AttackDebounce && distance<2f)
        {
            manager.ChangeCurrentState(manager.States.Melee);
        }

        if(!manager.ThrowDebounce && distance > 10f)
        {
            manager.ChangeCurrentState(manager.States.Throw);
        }

        manager.PlayAnimation(AnimClip.Walk);
    }

}

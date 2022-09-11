using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Axis.Abstractions;
using UnityEngine;

public class UtopiecAvoid : StateClass<UtopiecEnemy>
{
    private UtopiecEnemy manager;
    private Transform playerPos;
    public override void OnStateEnter(UtopiecEnemy obj)
    {
        manager = obj;

        playerPos = manager.Range.PlayerObj.transform;

        manager.InvokeRoutine(
            delegate
            {
                manager.ChangeCurrentState(manager.States.Chase);
            },
            Random.Range(5,10));
    }

    public override void OnStateExit()
    {
        return;
    }

    public override void OnStateUpdate()
    {
        var position = playerPos.position;
        Vector3 direction = (manager.transform.position - position).normalized;

        var strafeDir = Quaternion.Euler(0, 0, 45) * direction;

        Vector3 finalPosition = position+ direction* 3.5f + (strafeDir*0.4f);

        manager.Path.destination = finalPosition;

        if (!manager.AttackDebounce && Vector3.Distance(manager.transform.position, playerPos.position) <= 2f)
        {
            manager.ChangeCurrentState(manager.States.Attack);
        }

        manager.PlayAnimation(AnimClip.Walk);
    }
}

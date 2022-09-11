using System.Collections;
using System.Collections.Generic;
using Axis.Abstractions;
using UnityEngine;

public class BerserkAvoid : StateClass<BerserkEnemy>
{
    private BerserkEnemy manager;
    private Transform playerPos;
    public override void OnStateEnter(BerserkEnemy obj)
    {
        manager = obj;
        playerPos = manager.Range.PlayerObj.transform;
        manager.Deflect = true;

        ScheduleAttack();

        manager.OnTakeDamage += FastAttack;

    }

    public override void OnStateExit()
    {
        manager.OnTakeDamage -= FastAttack;
        manager.Deflect = false;
    }


    private void ScheduleAttack()
    {
        manager.InvokeRoutine(
            delegate
            {
                
                manager.ChangeCurrentState(manager.States.Attack);
            },
            Random.Range(1, 3));
    }
    public override void OnStateUpdate()
    {
        var position = playerPos.position;
        Vector3 direction = (manager.transform.position - position).normalized;

        var strafeDir = Quaternion.Euler(0, 0, 45) * direction;

        Vector3 finalPosition = position + direction * 1.5f + (strafeDir * 0.6f);

        manager.Path.destination = finalPosition;

        if(! manager.ThrowDebounce &&Vector3.Distance(manager.transform.position, playerPos.position) > 8f)
        {
            manager.AttackParticles.Play();
            manager.ThrowKnife();
            manager.ThrowCooldown();
        }

        manager.PlayAnimation(AnimClip.Walk);
    }

    private void FastAttack(float health)
    {
        manager.ChangeCurrentState(manager.States.FastAttack);
    }

}

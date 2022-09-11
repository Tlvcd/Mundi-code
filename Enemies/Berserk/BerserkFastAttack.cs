using System.Collections;
using System.Collections.Generic;
using Axis.Abstractions;
using UnityEngine;

public class BerserkFastAttack : StateClass<BerserkEnemy>
{

    public override void OnStateEnter(BerserkEnemy obj)
    {
		if (Vector3.Distance(obj.transform.position, obj.Range.PlayerTransform.position) > 3f)
		{
			obj.ChangeCurrentState(obj.States.Avoid);
			return;

		}

		obj.StopMovement();

		obj.PlayAnimation(AnimClip.Idle);
		obj.AttackParticles.Play();

		obj.AttackNearby(obj.DType);
		obj.FacePlayer();
		obj.PlayAnimation(AnimClip.Attack);

		obj.Invincible = true;

		obj.InvokeRoutine(
			delegate
			{
				obj.Invincible = false;
				obj.ChangeCurrentState(obj.States.Avoid);
			}, 1f);
	}

    public override void OnStateExit()
    {

    }

    public override void OnStateUpdate()
    {
        return;
    }

}

using System.Collections;
using System.Collections.Generic;
using Axis.Abstractions;
using UnityEngine;

public class BanditMelee : StateClass<BanditEnemy>
{

    public override void OnStateEnter(BanditEnemy obj)
    {
		obj.StopMovement();


		if (obj.AttackDebounce)
		{
			obj.ChangeCurrentState(obj.States.Avoid);
			return;
		}

		obj.PlayAnimation(AnimClip.Idle);
		obj.AttackParticles.Play();

		obj.InvokeRoutine(delegate
		{
			obj.AttackNearby(obj.DType);
			obj.FacePlayer();
			obj.PlayAnimation(AnimClip.Attack);
			obj.AttackCooldown();
		}, .5f);



		obj.InvokeRoutine(
			delegate
			{
				obj.ChangeCurrentState(obj.States.Avoid);
			}, 1.7f);
	}

    public override void OnStateExit()
    {

    }

    public override void OnStateUpdate()
    {
        return;
    }

}

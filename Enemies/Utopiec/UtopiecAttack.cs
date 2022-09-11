using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Axis.Abstractions;
using UnityEngine;


// ReSharper disable once CheckNamespace
// ReSharper disable once IdentifierTypo
public class UtopiecAttack : StateClass<UtopiecEnemy>
{
	private UtopiecEnemy manager;

	public override void OnStateEnter(UtopiecEnemy obj)
	{
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

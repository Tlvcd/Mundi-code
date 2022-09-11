using System.Collections;
using System.Collections.Generic;
using Axis.Abstractions;
using UnityEngine;

public class BerserkAttack : StateClass<BerserkEnemy>
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
		obj.SpawnAttackNowVFX();

		obj.OnTakeDamage += Stun;
		obj.InvokeRoutine(delegate
		{
			obj.AttackNearby(obj.DType);
			obj.FacePlayer();
			obj.PlayAnimation(AnimClip.Attack);
			obj.AttackCooldown();

			obj.OnTakeDamage -= Stun;
			obj.Invincible = true;
		}, .4f);



		obj.InvokeRoutine(
			delegate
			{
				obj.Invincible = false;
				obj.ChangeCurrentState(obj.States.Avoid);
			}, 1f);



		void Stun(float health)
		{
			obj.OnTakeDamage -= Stun;
			obj.ChangeCurrentState(obj.States.Stunned);
			
			
		}
	}

	

    public override void OnStateExit()
    {

    }

    public override void OnStateUpdate()
    {
        return;
    }

}

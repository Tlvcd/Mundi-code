using System.Collections;
using System.Collections.Generic;
using Axis.Abstractions;
using UnityEngine;

public class UtopiecDeath : StateClass<UtopiecEnemy>
{
	
	public override void OnStateEnter(UtopiecEnemy obj)
    {
        obj.Path.maxSpeed = 0;
        obj.Path.enabled = false;

		obj.PlayAnimation(AnimClip.Death);
		obj.InvokeRoutine(obj.DestroySelf, 2f);

	}
	public override void OnStateExit()
	{
	
	}
	
	public override void OnStateUpdate()
	{
		return;
	}
}

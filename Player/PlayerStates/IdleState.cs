using Axis.Abstractions;
using System;
using UnityEngine;

public class IdleState : StateClass<PlayerMovementStateManager>
{
    private PlayerMovementStateManager manager;

    public override void OnStateEnter(PlayerMovementStateManager obj)
    {
        manager = obj;
        manager.PlayerStateObject.OnPlayerMeleeAttack += OnAttack;
        manager.PlayerStateObject.OnPlayerSpellAttack += OnSpell;
        
        

        obj.PlayerAnims.PlayAnimation(AnimClip.Idle);
    }

    

    public override void OnStateExit()
    {
        manager.PlayerStateObject.OnPlayerMeleeAttack -= OnAttack;
        manager.PlayerStateObject.OnPlayerSpellAttack -= OnSpell;
    }

    public override void OnStateUpdate()
    {

        if (manager.Inputs.BasePlayer.Movement.ReadValue<Vector2>()!= Vector2.zero)
        {
            manager.ChangeState(manager.states.MoveState);
        }
    }

    private void OnAttack()
    {
        manager.ChangeState(manager.states.AttackState);
    }

    private void OnSpell()
    {
        manager.ChangeState(manager.states.SpellState);
    }
}

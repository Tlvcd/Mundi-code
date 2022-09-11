using Axis.Abstractions;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class MovementState : StateClass<PlayerMovementStateManager>
{
    private PlayerMovementStateManager manager;

    private Vector2 currInput, refVector;
    private Vector3 result;

    private float boost=1;

    public override void OnStateEnter(PlayerMovementStateManager obj)
    {
        manager = obj;
  
        manager.PlayerStateObject.OnPlayerMeleeAttack += OnAttack;
        manager.PlayerStateObject.OnPlayerSpellAttack += OnSpell;
        manager.Inputs.BasePlayer.Dodge.started += OnDodge;

        if (manager.Inputs.BasePlayer.Dodge.IsPressed())
        {
            ToggleSprint(true);
        }
    }




    public override void OnStateExit()
    {
        ToggleSprint(false);


        manager.PlayerStateObject.OnPlayerMeleeAttack -= OnAttack;
        manager.PlayerStateObject.OnPlayerSpellAttack -= OnSpell;

        manager.Inputs.BasePlayer.Dodge.started -= OnDodge;
    }

    public override void OnStateUpdate()
    {
        manager.SendDirectionToStateObject();

        manager.PlayerAnims.PlayAnimation(AnimClip.Walk);
        if (!manager.Inputs.BasePlayer.Dodge.IsPressed())
        {
            ToggleSprint(false);
        }


        CalculateMovement();
    }

    private void CalculateMovement()
    {
        var move = manager.Inputs.BasePlayer.Movement.ReadValue<Vector2>();
        if (move == Vector2.zero) ToIdle();

        manager.Direction = move;
        currInput = Vector2.SmoothDamp(currInput, move, ref refVector, manager.DampSpeed); // pobiera input gracza i wygladza przejscia

        //gdy nie wykryto inputu, ustawia zmienna wygladzania na 0. Jest po to zeby mozna bylo szybko zmienic kierunek po postoju
        currInput = (move.magnitude == 0) ? Vector2.zero : currInput;

        result = Vector3.Lerp(result,
            currInput * (manager.PlayerStats.moveSpeed*boost),
            manager.Accel * Time.deltaTime);

        manager.Rb.velocity = result;
    }

    private void ToIdle() => manager.ChangeState(manager.states.IdleState);

    private void OnAttack()
    {
        manager.ChangeState(manager.states.AttackState);
    }

    private void OnSpell()
    {
        manager.ChangeState(manager.states.SpellState);

    }

    private void ToggleSprint(bool state)
    {
        manager.EnableDebrisVFX(state);
        if (state)
        {
            boost = manager.SprintBonus + 1;
            manager.PlayerAnims.AnimationSpeed(1.5f);
            return;
        }
        boost = 1;
        manager.PlayerAnims.AnimationSpeed(1);
    }

    private void OnDodge(InputAction.CallbackContext obj)
    {
        ToggleSprint(false);
        
        if (!manager.PlayerStateObject.DodgeCd)
        {
            manager.ChangeState(manager.states.DodgeState);
        
        }

    }
}

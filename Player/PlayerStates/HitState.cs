using Axis.Abstractions;
using System.Threading.Tasks;
using UnityEngine;

public class HitState : StateClass<PlayerMovementStateManager>
{
    private PlayerMovementStateManager manager;
    private Vector2 currInput,result;
    private Vector2 refVector;

    public override void OnStateEnter(PlayerMovementStateManager obj)
    {
        
        manager = obj;

        obj.PlayBloodVFX();
        if (obj.PlayerStateObject.CurrentHealth<= 0)
        {
            obj.ChangeState(obj.states.DeathState);
        }
        
        obj.PlayerAnims.PlayAnimation(AnimClip.Hit);
        obj.Rb.velocity = Vector2.zero;


        obj.ExecuteDelay(0.25f, AwaitEnd);
    }

    public override void OnStateExit()
    {
        return;
    }

    public override void OnStateUpdate()
    {
        CalculateMovement();
    }

    private void AwaitEnd()
    {
        manager.PlayerStateObject.IsPerformingAction = false;
        manager.ChangeState(manager.states.IdleState);
    }

    private void CalculateMovement()
    {
        var move = manager.Inputs.BasePlayer.Movement.ReadValue<Vector2>()*0.5f;

        manager.Direction = move;
        currInput = Vector2.SmoothDamp(currInput, move, ref refVector, manager.DampSpeed); // pobiera input gracza i wygladza przejscia

        //gdy nie wykryto inputu, ustawia zmienna wygladzania na 0. Jest po to zeby mozna bylo szybko zmienic kierunek po postoju
        currInput = (move.magnitude == 0) ? Vector2.zero : currInput;

        result = Vector3.Lerp(result,
            currInput * manager.PlayerStats.moveSpeed,
            manager.Accel * Time.deltaTime);

        manager.Rb.velocity = result;
    }
}

using Axis.Abstractions;
using System.Threading.Tasks;
using UnityEngine;

public class AttackState : StateClass<PlayerMovementStateManager>
{
    public override void OnStateEnter(PlayerMovementStateManager obj)
    {
        manager = obj;

        obj.Rb.velocity = Vector2.zero;

        obj.FacePointerDirection();

        obj.PlayerAnims.PlayAnimation(AnimClip.Attack);
        obj.PlayerStateObject.IsPerformingAction = true;


        obj.ExecuteDelay(0.4f, AwaitEnd);
    }

    public override void OnStateExit()
    {
        
    }

    private PlayerMovementStateManager manager;
    private Vector2 currInput, result;
    private Vector2 refVector;
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
        var move = manager.Inputs.BasePlayer.Movement.ReadValue<Vector2>() * 0.3f;

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

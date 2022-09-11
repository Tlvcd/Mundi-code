using Axis.Abstractions;
using UnityEngine;
using System.Threading.Tasks;

public class DodgeState : StateClass<PlayerMovementStateManager>
{
    private PlayerMovementStateManager manager;

    private Vector2 initDir;

    public override void OnStateEnter(PlayerMovementStateManager obj)
    {

        Vector3 heading = obj.Inputs.BasePlayer.Movement.ReadValue<Vector2>();
        initDir = heading;
        heading *= 1000f;
        obj.SendDirectionToStateObject();

        obj.PlayJumpVFX();
        obj.PlayerAnims.PlayAnimation(AnimClip.Jump);

        obj.PlayerStateObject.PlayerIFrames(true);

        heading.Normalize();

        obj.Rb.AddForce(heading*10,ForceMode2D.Impulse);
        obj.PlayerStateObject.IsPerformingAction = true;


        obj.PlayerStateObject.StartDodgeCoolDown();
        
        manager = obj;


        obj.ExecuteDelay(0.4f, AwaitEnd);
    }

    public override void OnStateExit()
    {
        manager.PlayerAnims.AnimationSpeed(1);
        return;
    }

    public override void OnStateUpdate()
    {
        var dir =
            (initDir
            - manager.PlayerStateObject.MovementDirection);
        dir.x =Mathf.Clamp(dir.x, -1, 1);
        dir.y = Mathf.Clamp(dir.y, -1, 1);
        dir *= 3;

        manager.Rb.AddForce(dir, ForceMode2D.Force);
    }

    private void AwaitEnd()
    {
        

        manager.PlayerStateObject.IsPerformingAction = false;


        manager.PlayerStateObject.PlayerIFrames(false);

        manager.Rb.velocity /= 2;
        manager.ChangeState(manager.states.MoveState);
    }
}

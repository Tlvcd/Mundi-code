using Axis.Abstractions;
using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovementStateManager : MonoBehaviour
{
    [field: SerializeField]
    public float DampSpeed { get; private set; }
    [field: SerializeField]
    public float Accel { get; private set; }

    [field: SerializeField]
    public float SprintBonus { get; private set; }

    [field: SerializeField] public RandomClipPlayback ClipPlayer { get; private set; }

    [SerializeField] ParticleSystem DebrisEffect, JumpEffect, BloodVFX;

    #region state_specific_vars
    StateClass<PlayerMovementStateManager> currState;
    public MovementStates states { get; private set; } = new MovementStates();
    #endregion

    #region state_accessible_vars
    public PlayerInputs Inputs { get; private set; }
    public Statistics PlayerStats;

    public Rigidbody2D Rb { get; private set; }

    public Vector2 Direction;

    [field: SerializeField]
    public PlayerState PlayerStateObject { get; private set; }

    [field: SerializeField]
    public PlayerAnimationAssets PlayerAnims { get; private set; }

    #endregion

    public void FacePointerDirection()
    {
        var pInput = Inputs.BasePlayer;
        Vector2 padAim = pInput.AimGamepad.ReadValue<Vector2>();

        if ( padAim != Vector2.zero)
        {
            Direction = padAim;
            SendDirectionToStateObject();
            return;
        }

        Direction = pInput.Aim.ReadValue<Vector2>();
        Direction.x -= Screen.width/2;
        Direction.y -= Screen.height / 2;
        Direction.Normalize();//tutaj musze obliczac myszke bo ekran przesuwa wektor, przez co zawsze byl na plusie
        SendDirectionToStateObject();
    }

    private void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        Inputs = PlayerInputManagerClass.GetInputClass();
        PlayerStateObject.PlayerIFrames(false);
        PlayerStateObject.IsPerformingAction = false;
    }

    private void OnEnable()
    {
        
        currState = states.IdleState;
        currState.OnStateEnter(this);

        PlayerStateObject.OnPlayerDeath += ExecuteDeath;
        PlayerStateObject.OnPlayerHit += ExecuteHit;

    }

    

    private void OnDisable()
    {
        currState.OnStateExit();
        PlayerStateObject.OnPlayerDeath -= ExecuteDeath;
        PlayerStateObject.OnPlayerHit -= ExecuteHit;
    }

    private void ExecuteDeath()
    {
        ChangeState(states.DeathState);
        StopAllCoroutines();
    }

    private void ExecuteHit()
    {
        StopAllCoroutines();
        ChangeState(states.HitState);
    }

    public void ChangeState(StateClass<PlayerMovementStateManager> state)
    {
        currState?.OnStateExit();

        currState = state;

        currState.OnStateEnter(this);
    }

    public void EnableDebrisVFX(bool state)
    {

        if (state)
        {
            DebrisEffect.Play(true);
            return;
        }

        DebrisEffect.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }

    public void PlayJumpVFX()
    {
        JumpEffect.Play();
    }

    public void PlayBloodVFX()
    {
        BloodVFX.Play();
    }

    void Update()
    {
        currState.OnStateUpdate();
        
    }

    public void SendDirectionToStateObject()
    {
        PlayerStateObject.MovementDirection = Direction;

        if (Direction == Vector2.zero) return;
        PlayerStateObject.LastDirection = Direction;
    }

    private void FixedUpdate()
    {
        currState.OnStateFixedUpdate();

    }

    public void ExecuteDelay(float t, Action act)
    {
        StopAllCoroutines();
        StartCoroutine(CO_ExecuteAfterDelay(t,act));
    }

    private IEnumerator CO_ExecuteAfterDelay(float t, Action act)
    {
        float time = 0;

        while (time<t)
        {
            time += Time.deltaTime;

            yield return null;
        }

        act.Invoke();
        yield break;
    }
}

public class MovementStates
{
    public IdleState IdleState { get; private set; } = new IdleState();
    public MovementState MoveState { get; private set; } = new MovementState();
    public DodgeState DodgeState { get; private set; } = new DodgeState();
    public HitState HitState { get; private set; } = new HitState();
    public AttackState AttackState { get; private set; } = new AttackState();
    public SpellState SpellState { get; private set; } = new SpellState();
    public DeathState DeathState { get; private set; } = new DeathState();
}

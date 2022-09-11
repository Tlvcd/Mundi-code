using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Axis.Abstractions;
using Pathfinding;
using UnityEngine;

public class UtopiecEnemy : BaseEnemy
{
	[field: SerializeField] public AIPath Path { get; private set; }
	[field: SerializeField] public EnemyRange Range { get; private set; }
	[field: SerializeField] public PlayerAnimationAssets AnimAsset { get; private set; }

	[SerializeField] private Animator animPlayer;

	[field: SerializeField] public DamageType DType { get; private set; }

	[SerializeField] public ParticleSystem AttackParticles, BloodVFX;

	public int Direction { get; private set; }

	private StateClass<UtopiecEnemy> currState;

	public UtopiecBehaviourStates States { get; } = new UtopiecBehaviourStates();

	public Vector3 SpawnPos { get; private set; }
	public bool AttackDebounce { get; private set; }
	public async void AttackCooldown()
	{
		AttackDebounce = true;
		await Task.Delay(5000);
		AttackDebounce = false;
	}

	void Awake()
	{
		RestoreHealth();
		currState = States.Idle;
		SpawnPos = transform.position;
	}


	private void OnEnable()
	{
		Range.OnPlayerInRange += ChasePlayer;
		Range.OnPlayerLeftRange += ReturnToSpawn;

		currState.OnStateEnter(this);
	}

	private void OnDisable()
	{
		Range.OnPlayerInRange -= ChasePlayer;
		Range.OnPlayerLeftRange -= ReturnToSpawn;

		currState.OnStateExit();
	}

	private void Update()
	{
		var pos = transform.position;

		Direction = CalcVector2Dir((Path.steeringTarget - pos).normalized);
		currState.OnStateUpdate();

		if (!dead && Vector3.Distance(pos, SpawnPos) >= 20f)
		{
			ChangeCurrentState(States.ReturnToSpawn);
			Range.LostPlayer();
		}
	}


	protected override void OnEntityDeath()
	{
		base.OnEntityDeath();
		ChangeCurrentState(States.Death);
	}

	public void SpawnAttackVFX()
	{
		AttackParticles.Play();
	}

    public override void TakeDamage(float damage, DamageType dType)
    {
        base.TakeDamage(damage, dType);
		BloodVFX.Play();

		if (health > 0)
		{
			PlayAnimation(AnimClip.Hit);
		}
	}
    public void DestroySelf() => Destroy(this.gameObject);

    private readonly Vector2 defaultDir = new Vector2(0, 1);
	private int CalcVector2Dir(Vector2 vec)
	{
		var dir = Vector2.SignedAngle(vec, defaultDir);
		// ReSharper disable once PossibleLossOfFraction
		dir = (int)dir / 43;
		return _ = dir < 0 ? (int)dir + 8 : (int)dir;
	}

    public void FacePlayer()
    {
        var player = Range.PlayerObj;

        if (!player) return;
        Direction = CalcVector2Dir((player.transform.position - transform.position).normalized);
    }


	private void FixedUpdate()
	{
		currState.OnStateFixedUpdate();
	}

	public void ChangeCurrentState(StateClass<UtopiecEnemy> state)
	{
		currState.OnStateExit();
        StopAllCoroutines();
		currState = state;

		currState.OnStateEnter(this);
	}

	private void ChasePlayer()
	{
		ChangeCurrentState(States.Avoid);
	}

	private void ReturnToSpawn()
	{
		ChangeCurrentState(States.ReturnToSpawn);
	}

	public void InvokeRoutine(Action method, float time)
	{
		
		StartCoroutine(CO_routineInvoker());

		IEnumerator CO_routineInvoker()
		{
			yield return new WaitForSeconds(time);

			method.Invoke();
		}
	}

    public void PlayAnimation(AnimClip clip)
    {
		animPlayer.Play(AnimAsset.GetAnimation(clip, Direction));
    }

}

// ReSharper disable once IdentifierTypo
public class UtopiecBehaviourStates
{
	public UtopiecIdle Idle { get; } = new UtopiecIdle();
	public UtopiecAvoid Avoid { get; } = new UtopiecAvoid();

	public UtopiecChase Chase { get; } = new UtopiecChase();
	public UtopiecReturn ReturnToSpawn { get; } = new UtopiecReturn();

	public UtopiecAttack Attack { get; } = new UtopiecAttack();

    public UtopiecDeath Death { get; } = new UtopiecDeath();
}

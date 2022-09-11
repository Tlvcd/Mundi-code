using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Axis.Abstractions;
using Pathfinding;
using UnityEngine;

public class BanditEnemy : BaseEnemy
{
	[field: SerializeField] public AIPath Path { get; private set; }
	[field: SerializeField] public EnemyRange Range { get; private set; }
	[field: SerializeField] public PlayerAnimationAssets AnimAsset { get; private set; }

	[SerializeField] private Animator animPlayer;

	[field: SerializeField] public DamageType DType { get; private set; }
	[SerializeField] private Spell knifeSpell;

	[SerializeField] public ParticleSystem AttackParticles, BloodVFX;


	public int Direction { get; private set; }

	private StateClass<BanditEnemy> currState;

	public BanditBehaviourStates States { get; } = new BanditBehaviourStates();

	public Vector3 SpawnPos { get; private set; }

	[SerializeField] float attackCD, throwCD;
	public bool AttackDebounce { get; private set; }
	public bool ThrowDebounce { get; private set; }
	public async void AttackCooldown()
	{
		AttackDebounce = true;
		await Task.Delay((int)(attackCD*1000));
		AttackDebounce = false;
	}

	public async void ThrowCooldown()
    {
		ThrowDebounce = true;
		await Task.Delay((int)(throwCD * 1000));
		ThrowDebounce = false;
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
			ChangeCurrentState(States.Return);
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

		if (!Range.PlayerInRange) return;
		Direction = CalcVector2Dir((Range.PlayerTransform.position - transform.position).normalized);
	}

	private readonly Vector2 throwDefaultDir = new Vector2(1,0);
	public void ThrowKnife()
    {
		var idk = -Vector2.SignedAngle(Range.PlayerTransform.position- transform.position, throwDefaultDir);
		var rot = Quaternion.Euler(0, 0, idk-90);
		knifeSpell.SpawnProjectile(Stats, transform.position, rot);
	}

	private void FixedUpdate()
	{
		currState.OnStateFixedUpdate();
	}

	public void ChangeCurrentState(StateClass<BanditEnemy> state)
	{
		currState.OnStateExit();
		StopAllCoroutines();
		currState = state;

		currState.OnStateEnter(this);
	}

	public void StopMovement()
    {
		Path.destination = transform.position;
    }

	private void ChasePlayer()
	{
		ChangeCurrentState(States.Avoid);
	}

	private void ReturnToSpawn()
	{
		ChangeCurrentState(States.Return);
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
public class BanditBehaviourStates
{
	public BanditIdle Idle { get; } = new BanditIdle();

	public BanditAvoid Avoid { get; } = new BanditAvoid();

	public BanditThrow Throw { get; } = new BanditThrow();

	public BanditMelee Melee { get; } = new BanditMelee();

	public BanditDeath Death { get; } = new BanditDeath();

	public BanditChase Chase { get; } = new BanditChase();

	public BanditReturn Return { get; } = new BanditReturn();
	
}


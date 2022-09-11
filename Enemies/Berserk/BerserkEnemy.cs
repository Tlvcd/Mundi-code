using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Axis.Abstractions;
using Pathfinding;
using UnityEngine;

public class BerserkEnemy : BaseEnemy
{
	[field: SerializeField] public AIPath Path { get; private set; }
	[field: SerializeField] public EnemyRange Range { get; private set; }
	[field: SerializeField] public PlayerAnimationAssets AnimAsset { get; private set; }

	[SerializeField] private Animator animPlayer;

	[field: SerializeField] public DamageType DType { get; private set; }
	[SerializeField] private Spell knifeSpell;

	[SerializeField] public ParticleSystem AttackParticles, BloodVFX, AttackNowParticle;


	public int Direction { get; private set; }

	private StateClass<BerserkEnemy> currState;

	public BerserkBehaviourStates States { get; } = new BerserkBehaviourStates();

	public Vector3 SpawnPos { get; private set; }

	[SerializeField] float attackCD, throwCD;
	public bool AttackDebounce { get; private set; }
	public bool ThrowDebounce { get; private set; }

	[HideInInspector]
	public bool Deflect, Invincible;
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

		if (!dead&&Vector3.Distance(pos, SpawnPos) >= 20f)
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

	public void SpawnAttackNowVFX()
	{
		AttackNowParticle.Play();
	}

	public override void TakeDamage(float damage, DamageType dType)
	{
		if (dead || Invincible) return;

        if (Deflect)
        {
			ChangeCurrentState(States.FastAttack);
			return;
        }
		float calculatedDamage = Mathf.Round(damage * ((100 / (100 + Stats.GetDefenseFromType(dType))) * 100)) / 100;

		health -= calculatedDamage;
		TookDamage();
		PlayAnimation(AnimClip.Hit);
		BloodVFX.Play();

		DamageDisplay.Create(transform.position, calculatedDamage, dType);

		if (health <= 0)
		{
			OnEntityDeath();
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

	public void ChangeCurrentState(StateClass<BerserkEnemy> state)
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
public class BerserkBehaviourStates
{
	public BerserkIdle Idle { get; } = new BerserkIdle();

	public BerserkAvoid Avoid { get; } = new BerserkAvoid();

	public BerserkStunned Stunned { get; } = new BerserkStunned();

	public BerserkAttack Attack { get; } = new BerserkAttack();

	public BerserkFastAttack FastAttack { get; } = new BerserkFastAttack();

	public BerserkDeath Death { get; } = new BerserkDeath();

	public BerserkReturn Return { get; } = new BerserkReturn();
	
}


using System;
using System.Threading.Tasks;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerStateObject", menuName ="Axis Mundi/Internal/PlayerState")]
public class PlayerState : ScriptableObject
{
    [SerializeField]
    private float dodgeCdLength;

    private Vector2 moveDir= new Vector2(),  lastDir = new Vector2();
    private static readonly Vector2 DefaultDir = new Vector2(0, 1);

    public Vector2 MovementDirection { 
        get => moveDir;
        set { 
            OnPlayerChangeDirection?.Invoke(); 
            moveDir = value; 
        } 
    }
    public Vector2 LastDirection
    {
        get => lastDir;
        set
        {
            OnPlayerChangeDirection?.Invoke();
            lastDir = value;
        }
    }

    public Action<float> heal;
    public void Heal(float amount) => heal?.Invoke(amount);
    public float CurrentHealth { get; private set; }
    public void UpdateHP(float hp)
    {
        CurrentHealth = hp;
    }

    private float maxHP;
    public void SetMaxHP(float hp) => maxHP = hp;
    public float HealthPercentage => CurrentHealth/maxHP;


    public bool IsDead { get; private set; }
    public void SetDeath(bool state) => IsDead = state;

    public bool DodgeCd { get; private set; }
    public async void StartDodgeCoolDown()
    {
        DodgeCd = true;
        await Task.Delay((int)(dodgeCdLength * 1000));
        DodgeCd = false;
    }


    public bool Invincible { get; private set; }

    public bool IsPerformingAction;


    public Spell SelectedSpell { get; private set; }
    public event Action<Spell> OnSelectedSpellChange;
    public void ChangeSelectedSpell(Spell obj)
    {
        SelectedSpell = obj;
        OnSelectedSpellChange?.Invoke(obj);
    }


    public void PlayerIFrames(bool state) => Invincible = state;

    public event Action OnPlayerHit, OnPlayerHeal, OnPlayerMeleeAttack, OnPlayerSpellAttack, OnPlayerChangeDirection, OnPlayerDeath;

    public void PlayerHealed() => OnPlayerHeal?.Invoke();
    public void PlayerHit() => OnPlayerHit?.Invoke();
    public void PlayerMeleeAttack() => OnPlayerMeleeAttack?.Invoke();
    public void PlayerSpellAttack() => OnPlayerSpellAttack?.Invoke();

    public void PlayerDeath()
    {
        SetDeath(true);
        OnPlayerDeath?.Invoke();
    }

    public int GetPlayerDirection()
    {
        lastDir = moveDir;
        return CalcVector2Dir(MovementDirection == Vector2.zero ? lastDir : moveDir);
    }

    public int CalcVector2Dir(Vector2 vec)
    {
        var dir = Vector2.SignedAngle(vec, DefaultDir);
        // ReSharper disable once PossibleLossOfFraction
        dir = (int)dir / 43;
        return _ = dir < 0 ? (int)dir + 8 : (int)dir;
    }

    public Quaternion Rotation { get {
            var idk =-Vector2.SignedAngle(LastDirection, DefaultDir);
            return Quaternion.Euler(0,0,idk); 
        } 
    }

    

}

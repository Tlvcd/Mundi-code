using Axis.Items;
using System.Collections;
using UnityEngine;
using System;


[CreateAssetMenu(fileName ="spell", menuName ="Axis Mundi/Spell")]
public class Spell : Item
{
    [SerializeField]
    SpellBase projectile;

    [SerializeField]
    DamageType type;

    [SerializeField,Min(0.01f)]
    float multiplier=1f;

    [field: SerializeField, Min(0.1f)]
    private float cooldownTime= 1f;
    public float Timer { get; private set; } = 0;
    public float CoolDownProgress() => !Cooldown ? 1 : Timer / cooldownTime;

    public bool Cooldown { get; private set; } = false;

    public event Action OnCast;
    public event Action OnCooldown;

    public void ResetCooldown()
    {
        Cooldown = false;
        Timer = 0;
    }

    public void SpawnProjectile(Statistics stats,Vector2 position, Quaternion direction)
    {
        var obj = Instantiate(projectile,position,direction);
        obj.SetStats(stats,type,multiplier);
        OnCast?.Invoke();
    }

    public IEnumerator CountCooldown()
    {
        OnCooldown?.Invoke();
        Cooldown = true;

        Timer = 0;
        while (Timer < cooldownTime)
        {
            Timer += Time.deltaTime;
            yield return null;
        }

        Cooldown = false;
    }
}

public abstract class SpellBase : MonoBehaviour
{
    protected Statistics stats;
    protected float multiplier;
    protected DamageType damageType;

    public void SetStats(Statistics stat, DamageType dT, float multi)
    {
        stats = stat;
        damageType = dT;
        multiplier = multi;
    }

}

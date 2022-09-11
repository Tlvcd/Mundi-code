using UnityEngine;
using System;
using System.Collections.Generic;


[CreateAssetMenu(menuName = "Axis Mundi/Create new stats table")]
public class Statistics : ScriptableObject
{
    [SerializeField]
    private float health;
    public float Health => health;

    [SerializeField]
    private float movementSpeed;
    public float moveSpeed => movementSpeed;


    // public Dictionary<DamageType, float> defense { get; private set; } = new Dictionary<DamageType, float>();
    // public Dictionary<DamageType, float> attackStrength { get; private set; } = new Dictionary<DamageType, float>();

    [field: SerializeField] 
    public List<DamageTypeValue> AttackStats { get; private set; } = new List<DamageTypeValue>();

    [field: SerializeField]
    public List<DamageTypeValue> DefenseStats { get; private set; } = new List<DamageTypeValue>();



    public float GetAttackFromType(DamageType dType)
    {

        foreach (var item in AttackStats)
        {
            if (item.Damage == dType) return item.Value;
        }

        return 1f;
    }

    public float GetDefenseFromType(DamageType dType)
    {
        foreach (var item in DefenseStats)
        {
            if (item.Damage == dType) return item.Value;
        }

        
        return 0f;
    }

    public void ClearStats()
    {
        health = 0;
        movementSpeed = 0;
        DefenseStats.Clear();
        AttackStats.Clear();
        onStatsChange?.Invoke();
    }

    public event Action onStatsChange;
    

    #region StatOPs
    public void AddStatistics(Statistics stats)
    {
        if (stats == null) return;


        this.health += stats.Health;
        this.movementSpeed += stats.moveSpeed;
        AttackStats = AddValues(AttackStats, stats.AttackStats);
        DefenseStats = AddValues(DefenseStats, stats.DefenseStats);

        onStatsChange?.Invoke();

    }
    public void SubtractStatistics(Statistics stats)
    {
        this.health -= stats.Health;
        this.movementSpeed -= stats.moveSpeed;
        SubtractValues(AttackStats, stats.AttackStats);
        SubtractValues(DefenseStats, stats.DefenseStats);

        onStatsChange?.Invoke();
    }

    public List<DamageTypeValue> SubtractValues(List<DamageTypeValue> values, List<DamageTypeValue> values2)
    {
        var list = new List<DamageTypeValue>(values);

        foreach (var item in values2)
        {
            var obj = values.FindIndex(x => x.Damage == item.Damage);
            if (obj == -1)
            {
                list.Add(new DamageTypeValue(item.Damage,item.Value));
                continue;
            }


            list[obj].Value -= item.Value;
        }

        return list;
    }

    public List<DamageTypeValue> AddValues(List<DamageTypeValue> values, List<DamageTypeValue> values2)
    {
        var list = new List<DamageTypeValue>(values);

        foreach (var item in values2)
        {
            var obj = values.FindIndex(x => x.Damage == item.Damage);
            if (obj == -1)
            {
                list.Add(new DamageTypeValue(item.Damage, item.Value));
                continue;
            }


            list[obj].Value += item.Value;
        }

        return list;
    }

    #endregion


    private static void LogStatTable(string name)
    {
        var stat = Resources.FindObjectsOfTypeAll<Statistics>();
        foreach (var item in stat)
        {
            if(item.name == name)
            {
                item.LogStats();
                return;
            }
        }

        Debug.Log("Resource not found");
    }

    public void LogStats()
    {
        var sb = new System.Text.StringBuilder();
        sb.Append($"Name: {this.name}\nHealth: {health}\nSpeed: {movementSpeed}\nAttacks:\n");
        foreach (var item in AttackStats)
        {
            sb.Append($"-{item.Damage}: {item.Value}\n");
        }

        sb.Append("Defenses: \n");
        foreach (var item in DefenseStats)
        {
            sb.Append($"-{item.Damage}: {item.Value}\n");
        }

        Debug.Log(sb.ToString());
    }
}


[Serializable]
public class DamageTypeValue
{
    [SerializeField]
    DamageType type;
    public DamageType Damage => type;

    [field: SerializeField]
    public float Value { get; set; }
    

    public DamageTypeValue(DamageType dType, float value)
    {
        this.type = dType;
        this.Value = value;
    }

}

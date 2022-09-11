using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Axis.Abstractions;
using System;

public class PlayerBase : MonoBehaviour, IDamagable, ISaveable
{
    [SerializeField] float playerHealth;
    [SerializeField]
    Statistics playerStats;

    [SerializeField]
    PlayerState state;


    private void Awake()
    {
        state.SetDeath(false);
        UpdateStats();
        state.UpdateHP(playerHealth);

        state.SetMaxHP(playerHealth);
    }

    private void OnEnable()
    {
        UpdateStats();
        playerStats.onStatsChange += UpdateStats;

        state.heal += HealTarget;
    }

    private void OnDisable()
    {
        playerStats.onStatsChange -= UpdateStats;

        state.heal -= HealTarget;
    }

    private void UpdateStats()
    {
        playerHealth = playerStats.Health;
        state.SetMaxHP(playerHealth);
    }


    private void Hurt(float damage)
    {
        playerHealth -= damage;
        state.UpdateHP(playerHealth);

        state.PlayerHit();
        if (playerHealth <= 0)
        {
            state.PlayerDeath();
            return;
        }

    }

    public void TakeDamage(float damage, DamageType dType)
    {
        if (state.Invincible || state.IsDead) return;

        
        float calculatedDamage = Mathf.Round(damage * ((100 / (100 + playerStats.GetDefenseFromType(dType))) * 100)) / 100;

        calculatedDamage = Mathf.Abs(calculatedDamage);
        
        playerHealth -= calculatedDamage;
        state.UpdateHP(playerHealth);

        DamageDisplay.Create(transform.position,calculatedDamage,dType);
        
        state.PlayerHit();
        if (playerHealth <= 0)
        {
            state.PlayerDeath();
            return;
        }

    }

    public void HealTarget(float health)
    {
        playerHealth += Mathf.Abs(health);
        playerHealth = Mathf.Clamp(playerHealth, 0, playerStats.Health);
        state.UpdateHP(playerHealth);
        state.PlayerHealed();
    }

    public object SaveState()
    {
        return playerHealth;
    }

    public void RestoreState(object obj)
    {
        playerHealth = (float)obj;
        state.UpdateHP(playerHealth);
        state.PlayerHit();
    }


}

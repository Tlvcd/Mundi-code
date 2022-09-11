using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{

    [SerializeField] private PlayerState state;


    [SerializeField] Slider healthBar;

    private void Awake()
    {
        healthBar ??= GetComponent<Slider>();
    }
    private void OnEnable()
    {
        state.OnPlayerHit += UpdateHealthBar;
        state.OnPlayerHeal += UpdateHealthBar;
    }
    private void OnDisable()
    {
        state.OnPlayerHit -= UpdateHealthBar;
        state.OnPlayerHeal -= UpdateHealthBar;
    }

    private void UpdateHealthBar()
    {
        healthBar.value = state.HealthPercentage;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Axis.Abstractions;
using UnityEngine.UI;

public class EnemyHealthBarHandler : MonoBehaviour
{
    #region cached_vars
    [SerializeField] private BaseEnemy _currEnemy;
    private float _currEnemyMaxHealth;

    #endregion

    #region reference_vars
    [SerializeField]private Slider healthBar;
    [SerializeField] private Slider damageBar;

    #endregion

    #region Inspector_vars
    [SerializeField] private float sliderSpeed;
    [SerializeField] private float damageBarHold;

    #endregion

    private bool isDepleting=false; //func for coroutine
    private float calculatedBarValue;

    private void Awake()
    {
        _currEnemy ??= GetComponentInParent<BaseEnemy>();
    }

    private void OnEnable()
    {
        if (!_currEnemy) return;

      

        _currEnemyMaxHealth = _currEnemy.MaxHealth;
        damageBar.value = healthBar.value = _currEnemy.CurrentHealth / _currEnemyMaxHealth;
        _currEnemy.OnTakeDamage += DepleteBar;
        _currEnemy.OnHeal += RefillBar;//sets up on call events

    }

    private void OnDisable()
    {
        if (!_currEnemy) return;

        _currEnemy.OnTakeDamage -= DepleteBar;
        _currEnemy.OnHeal -= RefillBar;//removes from events
    }


    private void DepleteBar(float health)
    {
        calculatedBarValue = health / _currEnemyMaxHealth;
        healthBar.value = calculatedBarValue; //sets healthbar value

        if (isDepleting) { return; } //skips if coroutine already running

        StartCoroutine(SmoothChangeSliderValue(damageBar));
    }

    private void RefillBar(float health)
    {
        damageBar.value = healthBar.value = health/_currEnemyMaxHealth; //sets all bars to same value
    }

    IEnumerator SmoothChangeSliderValue(Slider slider)
    {
        isDepleting = true;
        yield return new WaitForSeconds(damageBarHold);

        while (!Mathf.Approximately(slider.value,calculatedBarValue)) //if slider value is near target value
        {
            slider.value = Mathf.Lerp(slider.value, calculatedBarValue, Time.deltaTime*sliderSpeed);
            yield return null;
        }
        isDepleting = false;
    }


}

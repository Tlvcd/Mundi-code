using Axis.Abstractions;
using Axis.Items;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] LayerMask layerSearch;
    [SerializeField] Vector2 testingAttackRange; //tylko do testowania, pozniej sie zajme dokladnym sprawdzaniem.

    [SerializeField]
    private float attackDelay;
    private float internalDelay;

    [SerializeField]
    Collider2D MeleeCollider;

    [SerializeField] private Transform spellSpawnPoint;
    [SerializeField] private GameObject SlashFX;

    private PlayerInputs _inputs;
    private List<Collider2D> collidersNearby = new List<Collider2D>(10);

    [SerializeField] private RandomClipPlayback clipPlay;

    [SerializeField]
    private Statistics playerStats;

    [SerializeField]
    private PlayerState playerState;

    [SerializeField]
    DamageType damageType;

    [SerializeField]
    InventoryAsset inventory;


    private ContactFilter2D CF2D; // melee attack collider filter;

    private void Awake()
    {
        _inputs = PlayerInputManagerClass.GetInputClass();


            foreach (var spell in inventory.EquippedSpells)
            {
                spell?.ResetCooldown();
            }
        
            

        CF2D = new ContactFilter2D
        {
            layerMask = layerSearch,
            useLayerMask = true,
            useTriggers = true
        };
    }

    private void OnEnable()
    {
        _inputs.BasePlayer.Attack.performed += AttackNearby;

        _inputs.BasePlayer.Spell.performed += FireSpell;

        _inputs.BasePlayer.ManageSelection.performed += ChangeSelectedSpell;

        _inputs.BasePlayer.QuickSelect.performed += QuickChangeSpell;
        //playerStats.onStatsChange += UpdateStats;
    }


    private void OnDisable()
    {
        _inputs.BasePlayer.Attack.performed -= AttackNearby;
        _inputs.BasePlayer.Spell.performed -= FireSpell;
        _inputs.BasePlayer.ManageSelection.performed -= ChangeSelectedSpell;

        _inputs.BasePlayer.QuickSelect.performed -= QuickChangeSpell;
        //playerStats.onStatsChange -= UpdateStats;
        StopAllCoroutines();
    }

    private int selectedSpellIndex = 0;
    private void ChangeSelectedSpell(InputAction.CallbackContext obj)
    {
        selectedSpellIndex =(int) obj.ReadValue<float>()-1;

        if (selectedSpellIndex > inventory.EquippedSpells.Count - 1) return;


        var selected = inventory.EquippedSpells[selectedSpellIndex];
        if (!selected) return;

        playerState.ChangeSelectedSpell(selected);
    }

    private void QuickChangeSpell(InputAction.CallbackContext obj)
    {

        selectedSpellIndex++;
        selectedSpellIndex = selectedSpellIndex > 3 ? 0 : selectedSpellIndex;
        if (selectedSpellIndex > inventory.EquippedSpells.Count - 1)
        {
            selectedSpellIndex = Mathf.Clamp(inventory.EquippedSpells.Count-1, 0, 99);
        }

        if (selectedSpellIndex > inventory.EquippedSpells.Count - 1) return;

        var selected = inventory.EquippedSpells[selectedSpellIndex];

        if (!selected) return;

            playerState.ChangeSelectedSpell(selected);


    }


    private void FireSpell(InputAction.CallbackContext obj)
    {
        if (!playerState.SelectedSpell 
            || playerState.SelectedSpell.Cooldown 
            || playerState.IsPerformingAction) return;

        StartCoroutine(playerState.SelectedSpell.CountCooldown());

        playerState.PlayerSpellAttack();
        playerState
            .SelectedSpell
                .SpawnProjectile(
                    playerStats, 
                    spellSpawnPoint.position, 
                    spellSpawnPoint.rotation
                );
    }


    private IEnumerator DelayTimer()
    {

        while (internalDelay < attackDelay)
        {
            internalDelay += Time.deltaTime;
            yield return null;
        }

        internalDelay = 0;

    }

    private void AttackNearby(InputAction.CallbackContext obj)
    {
        if (internalDelay != 0 || playerState.IsPerformingAction) return;
        
        Physics2D.OverlapCollider(MeleeCollider,CF2D, collidersNearby);

        playerState.PlayerMeleeAttack();


        Instantiate(SlashFX, gameObject.transform.position, spellSpawnPoint.rotation);
        if (collidersNearby.Count == 0) return;

        clipPlay.PlayRandomClip(4); //play from region 4

        foreach(Collider2D col in collidersNearby)
        {

            var crit = Random.Range(1, 1.3f);
            col.GetComponent<IDamagable>()?
                .TakeDamage(
                playerStats.GetAttackFromType(damageType)*crit,
                this.damageType
                );
        }

        
        StartCoroutine(DelayTimer());
    }

}

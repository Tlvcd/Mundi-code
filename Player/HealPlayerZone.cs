using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Axis.Abstractions;


public class HealPlayerZone : MonoBehaviour
{
    [SerializeField, Min(1)] private float healAmount=1;
    [SerializeField] private bool fullHeal;

    private void OnEnable()
    {
        //Debug.Log("started heal zone");
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!enabled ||!col.CompareTag("Player")) return;

        col.GetComponent<IDamagable>()
            .HealTarget(
                fullHeal ? 
                    99999 : healAmount
                    );

    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (!enabled || !col.CompareTag("Player")) return;

        col.GetComponent<IDamagable>()
            .HealTarget(
                fullHeal ?
                    99999 : healAmount
                    );
    }
}

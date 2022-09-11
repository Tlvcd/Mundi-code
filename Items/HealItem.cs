using System.Collections;
using System.Collections.Generic;
using Axis.Items;
using UnityEngine;

[CreateAssetMenu(menuName = "Axis Mundi/Items/heal item")]
public class HealItem : Item
{
    [SerializeField] private PlayerState state;

    [SerializeField]
    private float healAmount;

    public void Heal() => state.Heal(healAmount);
}

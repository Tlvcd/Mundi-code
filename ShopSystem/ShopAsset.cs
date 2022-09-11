using Axis.Items;
using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="ShopAsset", menuName ="Axis Mundi/Shop Asset")]
public class ShopAsset : ScriptableObject
{
    [field: SerializeField]
    public List<ShopEntry> Entries { get; set; }

}

[Serializable]
public class ShopEntry
{
    [field: SerializeField]
    public Item ItemObject { get; private set; }

    [field: SerializeField, Min(1)]
    public int Amount { get; private set; }

    [field: SerializeField]
    public int Price { get; private set; }

    [field: SerializeField,Min(1)]
    public int InStorage { get; private set; }

    /// <summary>
    /// Usuwa amount od storage, uruchom po kupnie
    /// </summary>
    /// <returns>jezeli nic nie zostalo, true.</returns>
    public bool CalculateStorage()
    {
        InStorage -= Amount;
        return InStorage <= 0;
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Axis.Items;
using UnityEngine;


public class PlayerStatsManager : MonoBehaviour
{

    [SerializeField] private InventoryAsset pInv;

    /// <summary>
    /// Bierze bazowe staty gracza, nie uwzgledniajac innych itemow.
    /// </summary>
    public Statistics BasePlayerStats;

    public Statistics AdditionalPlayerStats;

    /// <summary>
    /// Staty uwzgledniajace wszystkie czynniki (bron, skille itp.)
    /// </summary>
    public Statistics FinalPlayerStats;

    private void Awake()
    {
        AdditionalPlayerStats.ClearStats();
        foreach (var stat in pInv.EquippedItems)
        {
            AdditionalPlayerStats.AddStatistics(stat.GetItemStats());
        }
    }

    private void OnEnable()
    {
        RecalcFinal();
        BasePlayerStats.onStatsChange += RecalcFinal;
        AdditionalPlayerStats.onStatsChange += RecalcFinal;
        
    }

    private void OnDisable()
    {
        BasePlayerStats.onStatsChange -= RecalcFinal;
        AdditionalPlayerStats.onStatsChange -= RecalcFinal;
    }


    public void RecalcFinal()
    {
        var watch = new Stopwatch();
        watch.Start();
        FinalPlayerStats.ClearStats();
        FinalPlayerStats.AddStatistics(AdditionalPlayerStats);
        FinalPlayerStats.AddStatistics(BasePlayerStats);
        watch.Stop();

        var end = watch.ElapsedTicks;
        //UnityEngine.Debug.Log($"Took: {end*100} ns");
    }

      
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayPlayerStats : MonoBehaviour
{
    [SerializeField]
    private Statistics playerFinalStats;

    StatsInspector inspector;

    private void Awake()
    {
        inspector = GetComponent<StatsInspector>();
        PassStatsToInspector();
    }

    private void OnEnable()
    {
        playerFinalStats.onStatsChange += PassStatsToInspector;
        
    }

    private void OnDisable()
    {
        playerFinalStats.onStatsChange -= PassStatsToInspector;
    }

    private void PassStatsToInspector()
    {
        inspector.DisableModules();
        inspector.DisplayStatistics(playerFinalStats);
    }
}

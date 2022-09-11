using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestPlayerHealth : QuestHandle
{
    [SerializeField]
    PlayerState player;

    [SerializeField]
    float healthPercentage;

    private void OnEnable()
    {
        player.OnPlayerHit += CheckHealth;
    }

    private void OnDisable()
    {
        player.OnPlayerHit -= CheckHealth;
    }


    private void CheckHealth()
    {
        if (player.HealthPercentage > healthPercentage/100) return;
        Debug.Log("player health point reach");
        SendCompletion();
    }
}

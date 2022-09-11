using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsInspector : MonoBehaviour
{
    [SerializeField] private Transform parent;

    [SerializeField] private StatInspectorModule module;

    private Queue<StatInspectorModule> activeModules = new Queue<StatInspectorModule>()
        , modulesPool = new Queue<StatInspectorModule>();

    public void DisplayStatistics(Statistics stats)
    {
        if (!stats) return;

        int countIndex = 0;

        foreach (var def in stats.DefenseStats)
        {
            CreateModule(def.Damage.TypeName+ " defensywa", def.Value, countIndex);
            countIndex++;
        }

        foreach (var atk in stats.AttackStats)
        {
            CreateModule(atk.Damage.TypeName+" atak", atk.Value, countIndex);
            countIndex++;
        }

        if (stats.moveSpeed > 0) CreateModule("Zwinność", stats.moveSpeed,countIndex, 20);

        countIndex++;
        if (stats.Health > 0) CreateModule("Zdrowie", stats.Health,countIndex, 500);        
    }

    private void CreateModule(string displayName, float value,int index ,float maxValue = 200)
    {
        StatInspectorModule obj;
        if (modulesPool.Count > 0)
        {
            obj =modulesPool.Dequeue();
            obj.gameObject.SetActive(true);
        }
        else
        {
            obj = Instantiate(module, parent);
        }
        activeModules.Enqueue(obj);
        obj.transform.SetSiblingIndex(index);
        obj.DisplayValue(displayName, value, maxValue);
    }

    public void DisableModules()
    {
        foreach (var statInspectorModule in activeModules)
        {
            statInspectorModule.gameObject.SetActive(false);
            modulesPool.Enqueue(statInspectorModule);
        }

        activeModules.Clear();
    }
}

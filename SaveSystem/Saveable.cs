using System;
using UnityEditor;
using UnityEngine;

using Axis.Abstractions;
using System.Collections.Generic;


[DisallowMultipleComponent]
public class Saveable : MonoBehaviour
{
    [field: SerializeField]
    public string GUID { get; private set; }


    private void CheckForGuid()
    {
        Debug.Log(GUID);
    }
    
    [Button]
    private void GenerateGuid()
    {
        GUID = Guid.NewGuid().ToString(); 
    }

    private void OnValidate()
    {
        if (String.IsNullOrEmpty(GUID))
        {
            GenerateGuid();
        }
    }

    public object SaveState()
    {
        var saveables = GetComponents<ISaveable>();
        var states = new Dictionary<string, object>();
        foreach (var item in saveables)
        {
            states[item.GetType().ToString()] = item.SaveState();
        }

        return states;
    }

    public void LoadState(object obj)
    {
        var states = (Dictionary<string, object>)obj;
        var saveables = GetComponents<ISaveable>();
        foreach (var item in saveables)
        {
            string typeName = item.GetType().ToString();

            if(states.TryGetValue(typeName,out object value))
            {
                item.RestoreState(value);
            }
        }

    }

}

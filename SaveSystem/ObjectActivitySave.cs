using Axis.Abstractions;
using UnityEngine;
using System;

[RequireComponent(typeof(Saveable)), DisallowMultipleComponent]
public class ObjectActivitySave : MonoBehaviour, ISaveable
{
    public void RestoreState(object obj)
    {
        var data = (SaveData)obj;
        gameObject.SetActive(data.Active);
    }


    public object SaveState()
    {
        return new SaveData
        {
            Active = gameObject.activeInHierarchy
        };
    }
    
    [Serializable]
    private struct SaveData
    {
        public bool Active;
    }

}



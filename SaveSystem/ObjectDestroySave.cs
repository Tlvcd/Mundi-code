using UnityEngine;
using System;
using System.Collections.Generic;
using Axis.Abstractions;


[RequireComponent(typeof(Saveable)), DisallowMultipleComponent]
public class ObjectDestroySave : MonoBehaviour, ISaveable
{
    [SerializeField] private List<GameObject> ObjectPool;

    public void RestoreState(object obj)
    {
        var data = (SaveData)obj;
        for (int i = 0; i < data.ObjectStatus.Length; i++)
        {
            if(!data.ObjectStatus[i]) Destroy(ObjectPool[i]);
        }
    }

    public object SaveState()
    {

        var save = new SaveData();
        save.ObjectStatus = new bool[ObjectPool.Count];
        for (int i = 0; i < ObjectPool.Count; i++)
        {
            save.ObjectStatus[i] = ObjectPool[i] != null;
        }

        return save;
    }

    [Button]
    private void OnValidate()
    {
        List<GameObject> objects= new List<GameObject>();

        foreach (var obj in FindObjectsOfType<ObjectActivitySave>(true))
        {
            objects.Add(obj.gameObject);
        }

        ObjectPool = objects;
    }

    [Serializable]
    private class SaveData
    {
        //false jezeli nie istnieje
        public bool[] ObjectStatus;
    }

}

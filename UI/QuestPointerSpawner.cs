using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestPointerSpawner : MonoBehaviour
{
    [SerializeField] private QuestInterface qstInt;
    [SerializeField] private QuestPointer pointerObject;

    private List<QuestPointer> pointerPool = new List<QuestPointer>();

    private QuestObjective currObj;

    private void OnEnable()
    {
        qstInt.ObjectiveChange += Sarapakaribe;
    }

    private void OnDisable()
    {
        qstInt.ObjectiveChange -= Sarapakaribe;
    }

    private void Sarapakaribe()
    {
        qstInt.CurrentObjective.OnSubObjective += delegate
        {
            SpawnPointers(qstInt.CurrentObjective.ObjectivePositions);
        };

        SpawnPointers(qstInt.CurrentObjective.ObjectivePositions);
    }

    private void SpawnPointers(List<Vector3> posList)
    {
        var itemCount = posList.Count;
        foreach (var pointer in pointerPool)
        {
            pointer.gameObject.SetActive(false);
        }

        for (int i = 0; i < itemCount; i++)
        {
            var current = posList[i];
            if (pointerPool.Count < itemCount)
            {
                var obj = Instantiate(pointerObject, transform);
                obj.SetTarget(current);

                pointerPool.Add(obj);
                continue;
                
            }   
            
            pointerPool[i].SetTarget(current);
            pointerPool[i].gameObject.SetActive(true);
            
        }
    }
}

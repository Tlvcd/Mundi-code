using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnOnKill : MonoBehaviour
{
    [SerializeField] private List<GameObject> objects;
    [SerializeField] private float spawnRange=1f;
    private void OnDestroy()
    {
        var selfPos = transform.position;

        foreach (var obj in objects)
        {
            if (!obj) continue;

            var point = new Vector3(selfPos.x + RandGen(), selfPos.y + RandGen(), 0);
            Instantiate(obj, point, Quaternion.identity);


            float RandGen() => Random.Range(-spawnRange, spawnRange);
        }
    }
}

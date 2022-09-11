using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Axis Mundi/Internal/Level")]
public class LoadAsset : ScriptableObject
{
    public event Action<string> OnSceneLoader;

    public void LoadLevel(string scena){
        OnSceneLoader?.Invoke(scena);
    }
}

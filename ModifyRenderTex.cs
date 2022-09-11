using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Threading.Tasks;

public class ModifyRenderTex : MonoBehaviour
{
    [SerializeField]
    private Camera RenderTexCam;
    [SerializeField]
    private Material blurMaterial;

    [SerializeField]
    RenderTexture defTex; //tylko do edytora.

    private void Start()
    {
        ReCalculateRenderTex();

    }



    public void ReCalculateRenderTex()
    {
        RenderTexCam.targetTexture = null;
        if (defTex != null)
        {
            defTex.Release();
        }
        defTex.width = Screen.width / 2;
        defTex.height = Screen.height / 2;
        defTex.Create();
        RenderTexCam.targetTexture = defTex;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Axis Mundi/Internal/tooltip bridge")]
public class TooltipInterface : ScriptableObject
{

    public event Action<string> OnNewTooltip;

    public void SendNewTooltip(string message)
    {
        OnNewTooltip?.Invoke(message);
    }
}

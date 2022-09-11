using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    [SerializeField]
    TooltipInterface bridge;

    CanvasGroup group;


    TMP_Text textTMP;
    private void Awake()
    {
        textTMP = GetComponent<TMP_Text>();
        group = GetComponent<CanvasGroup>();

        textTMP.text = string.Empty;
    }

    private void OnEnable()
    {
        bridge.OnNewTooltip += QueueTooltip;
    }

    private void OnDisable()
    {
        bridge.OnNewTooltip -= QueueTooltip;
    }

    private Queue<string> tipQueue = new Queue<string>();
    private void QueueTooltip(string obj)
    {
        tipQueue.Enqueue(obj);

        if (!queueActive && tipQueue.Count == 1) DisplayTooltip();
    }

    private bool queueActive = false;
    private async void DisplayTooltip()
    {
        if (tipQueue.Count == 0) return;
        queueActive = true;
        group.alpha = 0;

        LeanTween.alphaCanvas(group, 1, 0.5f);
        textTMP.text = tipQueue.Dequeue();
        await Task.Delay(4000);

        LeanTween.alphaCanvas(group, 0, 0.3f).setOnComplete(() =>
        {
            if (tipQueue.Count > 0)
            {
                DisplayTooltip();
                return;
            }
            queueActive = false;
        });

    }
}

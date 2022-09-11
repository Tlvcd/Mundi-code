using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestStatusDisplay : MonoBehaviour
{
    [SerializeField]
    QuestInterface bridge;

    [SerializeField]
    TMP_Text title, desc;

    [SerializeField]
    TooltipInterface tooltip;

    private CanvasGroup group;
    private void Awake()
    {
        group = GetComponent<CanvasGroup>();
    }


    private void OnEnable()
    {
        bridge.ObjectiveChange += DisplayObjective;
        bridge.OnQuestEnd += FadeDisplay;
        bridge.OnQuestStart += TooltipQuest;
    }

    private void FadeDisplay()
    {
        LeanTween.alphaCanvas(group, 0, 0.3f).setEaseInCirc();
    }

    private void OnDisable()
    {
        bridge.OnQuestStart -= TooltipQuest;
        bridge.OnQuestEnd -= FadeDisplay;
        bridge.ObjectiveChange -= DisplayObjective;
    }

    private void TooltipQuest()
    {
        tooltip.SendNewTooltip(bridge.CurrentQuest.quest.QuestName);
    }

    private void DisplayObjective()
    {
        var obj = bridge.CurrentObjective;

        title.text = obj.ObjectiveName;
        desc.text = obj.ObjectiveDescription;

        LeanTween.alphaCanvas(group, 1, 0.3f).setEaseOutCirc();
    }
}

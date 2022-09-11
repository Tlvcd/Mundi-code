using UnityEngine;
using TMPro;
using System;

public class TemporaryQuestStatus : MonoBehaviour
{
    /*[SerializeField]
    QuestInterface reciever;

    [SerializeField]
    TMP_Text text;

    private void OnEnable()
    {
        reciever.ObjectiveChange += UpdateText;

        reciever.OnQuestEnd += EndQuestStatus;
    }

    private void EndQuestStatus(QuestBase obj)
    {
        text.text = $"Quest: {obj.QuestName} has been completed!";
    }

    private void OnDisable()
    {
        reciever.ObjectiveChange -= UpdateText;
        reciever.OnQuestEnd -= EndQuestStatus;
    }

    private void UpdateText(QuestObjective obj)
    {

        text.text = $"Objective Name:\n {obj.ObjectiveName}\nDescription:\n {obj.ObjectiveDescription}";
    }
    */
}

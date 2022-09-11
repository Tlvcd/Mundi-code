using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName ="Quest interface", menuName ="Axis Mundi/Internal/QuestInterface")]
public class QuestInterface : ScriptableObject
{
    public Action<QuestBase> NewQuest;
    public event Action OnQuestStart;

    public QuestManager.QuestProgress CurrentQuest { get; private set; }
    public QuestObjective CurrentObjective;


    public void SetNewQuest(QuestManager.QuestProgress qst)
    {
        CurrentQuest = qst;
    } 

    public event Action ObjectiveChange;
    public event  Action OnQuestEnd;

    public void OnObjectiveChange() => ObjectiveChange?.Invoke();
    public void EndQuest()
    {
        CurrentObjective = null;
        OnQuestEnd?.Invoke();
    }

    public void QuestStart()
    {
        OnQuestStart?.Invoke();
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Axis.Items;


[CreateAssetMenu(fileName ="New Quest", menuName ="Axis Mundi/ Quest asset"), Serializable]
public class QuestBase : ScriptableObject
{
    [SerializeField]
    private string questName;
    public string QuestName => questName;

    [field: SerializeField]
    public int QuestImportance { get; set; }

    [field: SerializeField]
    public bool AutoStart { get; set; }

    [SerializeField]
    private string questDescription;
    public string QuestDescription => questDescription;


    public List<QuestObjective> Objectives= new List<QuestObjective>();


    public void CompleteQuest()
    {
        OnQuestEnd?.Invoke();
    }

    public event Action OnQuestStart;
    public event Action OnQuestEnd;


}

[Serializable]
public class QuestObjective
{


    [SerializeField]
    private string objName;
    public string ObjectiveName => objName;

    [SerializeField]
    private string description;
    public string ObjectiveDescription => description;

    [SerializeField]
    private bool multipleObjectives;

    [SerializeField]
    private int amountRequired=1;

    private int currAmount;

    [SerializeField]
    public bool givesRewards;

    [SerializeField]
    public QuestRewards rewards;

    public List<Vector3> ObjectivePositions { get; private set; } = new List<Vector3>();


    public void PassObjectivePosition(Vector3 pos)
    {
        ObjectivePositions.Add(pos);
    }


    public void DeleteObjectivePosition(Vector3 pos)
    {
        ObjectivePositions.Remove(pos);
    }

    [HideInInspector]
    public bool Completed;

    [HideInInspector]
    public bool Pending;

    public bool RetroActivated { get; set; }

    /// <summary>
    /// Tylko do manager'a quest'ów.
    /// </summary>
    public Action ObjectiveProgress;

    /// <summary>
    /// Wylaczenie obiektow sluchajacych eventa
    /// </summary>
    public event Action ObjectDeinit;

    public event Action OnSubObjective;
    public Action OnStart;

    /// <summary>
    /// wlaczenie obiektów sluchających eventa
    /// </summary>
    public event Action ObjectInit;


    public void StartObjective()
    {
        ObjectivePositions.Clear();

        Pending = true;
        Completed = false; //gdyby quest mialby byc powtarzany
        amountRequired = multipleObjectives ? amountRequired : 1;
        currAmount = 0;
        OnStart?.Invoke();
        ObjectInit?.Invoke();

        
    }

    public void CompleteObjective(QuestHandle handle)
    {
        currAmount++;
        if(currAmount< amountRequired)
        {
            OnSubObjective?.Invoke();
            handle.DeInit();
            return;
        }


        ObjectDeinit?.Invoke();
        ObjectiveProgress?.Invoke();
        Completed = true;
        Pending = false;
        currAmount = 0;


        OnSubObjective = null;
    }

    /// <summary>
    /// Na sile deinicjuje kazdego nasluchywacza tego objective.
    /// </summary>
    public void ForceDeinit()
    {
        ObjectDeinit?.Invoke();
    }

}

[Serializable]
public struct QuestRewards
{
    public int Money;

    public List<Item> ItemRewards;
}


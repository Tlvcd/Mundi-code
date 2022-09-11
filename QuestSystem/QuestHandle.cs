using UnityEngine;


using System;

public abstract class QuestHandle : MonoBehaviour
{
    [SerializeField]
    private QuestBase quest;

    [SerializeField]
    private int objectiveID;


    [SerializeField]
    protected bool DestroyAfterCompletion;

    [SerializeField]
    protected bool DisableAfterCompletion;

    [SerializeField]
    protected bool Retroactive;

    [SerializeField] private bool markLocation = true;

    private bool completed;

    private Vector3 cachedPosition;

    protected void SendCompletion()
    {
        if (!this.enabled)
        {
            completed = true;

            return;
        }
        CompleteCurrentObjective();

    }

    private void CompleteCurrentObjective()
    {
        quest.Objectives[objectiveID].CompleteObjective(this);
    }

    protected virtual void Init()
    {
        this.enabled = true;

        if (markLocation)
        {
            quest.Objectives[objectiveID].PassObjectivePosition(cachedPosition);
        }


        gameObject.SetActive(true);
        if (completed) {CompleteCurrentObjective();  }
    }

    public virtual void DeInit()
    {
        if (markLocation)
        {
            quest.Objectives[objectiveID].DeleteObjectivePosition(cachedPosition);
        }

        completed = false;
        this.enabled = false;
        

        if (DisableAfterCompletion) this.gameObject.SetActive(false);
        if (DestroyAfterCompletion) Destroy(this.gameObject);
    }


    

#if UNITY_EDITOR

    private ValueDropdownList<int> GetObjectives()
    {
        if (quest == null) return null;

        var list = new ValueDropdownList<int>();

        for (int i = 0; i < quest.Objectives.Count; i++)
        {
            list.Add(quest.Objectives[i].ObjectiveName,i);
        }

        return list;
    }


#endif


    private void Awake()
    {
        cachedPosition = transform.position;
        var objective = quest.Objectives[objectiveID];
        objective.ObjectInit += Init;
        objective.ObjectDeinit += DeInit;

        this.enabled = false;
        HandleAwake();
    }

    /// <summary>
    /// Awake dla klas implementujacych QuestHandle, dzieki niemu poprawie wszystko dziala
    /// </summary>
    protected virtual void HandleAwake() { }


    /// <summary>
    /// Destroy dla klas implementujacych QuestHandle
    /// </summary>
    protected virtual void HandleDestroy() { }

    private void OnDestroy()
    {
        var objective = quest.Objectives[objectiveID];
        objective.ObjectInit -= Init;
        objective.ObjectDeinit -= DeInit;

        HandleDestroy();
    }




}




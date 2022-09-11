using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using Axis.Items;

public class QuestManager : MonoBehaviour
{



    [field: SerializeField]
    public QuestProgress currQst { get; private set; }

    [SerializeField]
    private QuestInterface questReciever;

    [SerializeField]
    InventoryAsset inv;

    
    public List<QuestProgress> pendingQuests = new List<QuestProgress>();


    private int questLength;

    private Action completion;


    private void OnEnable()
    {
        questReciever.NewQuest += AddNewQuest;
    }

    private void OnDisable()
    {
        questReciever.NewQuest -= AddNewQuest;
    }

    private void AddNewQuest(QuestBase obj)
    {
        if (!obj || pendingQuests.Any(x => x.quest == obj)) return;

        

        var quest = new QuestProgress(0,obj);

        pendingQuests.Add(quest);

        if (obj.AutoStart) { StartQuest(quest); }
    }

    public void ResumeQuest(QuestBase obj)
    {
        var quest = pendingQuests
            .FirstOrDefault(x => x.quest == obj);

        if (quest != null) { StartQuest(quest); }

    }

    public void StartQuest(QuestProgress questAsset)
    {

        currQst?.quest?
            .Objectives[IndexCorrection(currQst.objID)]
            .ForceDeinit(); 

        
         // wylacz obiekty z innego questa

        currQst = questAsset;
        questReciever.SetNewQuest(questAsset);
        questReciever.QuestStart();

        questLength = currQst.quest.Objectives.Count;

        PopulateDelegate(ProgressObjective);

        var index = IndexCorrection(currQst.objID);
        currQst.objID = index; // ponowne wczytywanie poprawnego zadania.
        
        ProgressObjective();

    }

    private void CompleteQuest()
    {
        // usuwa pozostaly event

        questReciever.EndQuest();

        questReciever.SetNewQuest(null);

        currQst.quest.CompleteQuest(); //metoda konczaca w srodku skryptu

        pendingQuests = pendingQuests.Where(x => x.quest != currQst.quest).ToList();
        currQst = null; // usuwa referencje obecnego
    }

    public void ProgressObjective()
        {
        if (questReciever.CurrentObjective!=null &&questReciever.CurrentObjective.givesRewards)
        {
            var reward = questReciever.CurrentObjective.rewards;
            foreach (var item in reward.ItemRewards)
            {
                inv.AddItem(item);
            }
            inv.Amber += reward.Money;
        }

        if (currQst.objID >= questLength) // jezeli index jest ostatnim, Konczy quest.
        {
            CompleteQuest();
            return;
        }

        var current = currQst.quest.Objectives[currQst.objID];
        questReciever.CurrentObjective = current;

        currQst.objID++;
        current.StartObjective(); // startuje i dodaje obecna metode do eventu po skonczeniu

        questReciever.OnObjectiveChange();


    }

    private void PopulateDelegate(Action act) //usuwa poprzednia metode z eventu by zapobiec bledom.
    {
        currQst
            .quest
            .Objectives.ForEach(x => x.ObjectiveProgress = act);
        
    }

    [Serializable]
    public class QuestProgress
    {
        public int objID;
        public QuestBase quest;

        public QuestProgress(int index, QuestBase qst)
        {
            this.objID = index;
            this.quest = qst;
        }
    }

    private void FindAndStart(string name)
    {
        var pepe = pendingQuests.FirstOrDefault(x => x.quest.QuestName == name);

        if (pepe != null) { StartQuest(pepe); }
    }

    private int IndexCorrection(int num) //odejmuje by quest zawsze wracal na zachowany objective.
    {
         return num > 0 ? num - 1 : 0;
    }

}

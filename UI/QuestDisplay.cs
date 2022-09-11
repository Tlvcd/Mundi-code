using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestDisplay : MonoBehaviour
{
    [SerializeField]
    QuestManager manager;

    [SerializeField]
    TMP_Text title, desc, progress;

    [SerializeField]
    QuestUIbutton prefab, currentQuest;

    private List<QuestUIbutton> buttons = new List<QuestUIbutton>();


    private void OnEnable()
    {
        foreach (var item in manager.pendingQuests)
        {
            var butt = Instantiate(prefab,transform);
            bool selected = item == manager.currQst;

            butt.SetContainer(item, selected);
            butt.OnClick = DescribeQuest;


            buttons.Add(butt);
        }
    }

    private void OnDisable()
    {
        ClearAllButtons(); 
    }

    private void ClearAllButtons()
    {
        foreach (var item in buttons)
        {
            Destroy(item.gameObject);
        }
        buttons.Clear();
    }

    QuestManager.QuestProgress selected;
    private void DescribeQuest(QuestManager.QuestProgress obj)
    {
        selected = obj;
        var qst = obj.quest;


        title.text = qst.QuestName;
        desc.text = qst.QuestDescription;
    }


    public void StartSelectedQuest()
    {
        if (selected == null || selected ==manager.currQst) return;

        manager.StartQuest(selected);
    }
}

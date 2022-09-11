using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestUIbutton : MonoBehaviour
{

    [SerializeField]
    TMP_Text title;

    [SerializeField]
    GameObject currentHighlight;

    public Action<QuestManager.QuestProgress> OnClick;
    public QuestManager.QuestProgress container { get; private set; }
    public void SetContainer(QuestManager.QuestProgress obj, bool current = false)
    {
        container = obj;
        title.text = obj.quest.QuestName;
        currentHighlight.SetActive(current);
    }
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(InvokeClicked);
    }

    private void InvokeClicked()
    {
        OnClick?.Invoke(container);
    }

}

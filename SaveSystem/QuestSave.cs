using Axis.Abstractions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestSave : MonoBehaviour, ISaveable
{
	[SerializeField]
	QuestManager manager;

	private void Awake()
	{
		//manager = GetComponent<QuestManager>();
	}

	public void RestoreState(object obj)
	{
		var data = (SaveData)obj;
		var list = JsonUtility.FromJson<ListWrapper>(data.questTable);

        Debug.Log(list.questList.Count);
		manager.pendingQuests = list.questList;


		var currQst = JsonUtility.FromJson<QuestManager.QuestProgress>(data.currQuest);
		if (currQst == null) return;
		manager.StartQuest(currQst);
		
	}


	public object SaveState()
	{
		Debug.Log(manager.pendingQuests.Count);

		var listWrap = new ListWrapper
		{
			questList = manager.pendingQuests
		};

		return new SaveData
		{
			questTable = JsonUtility.ToJson(listWrap),
			currQuest = JsonUtility.ToJson(manager.currQst)
		};
	}

	[Serializable]
	private struct SaveData
	{
		public string questTable;
		public string currQuest;
	}

	private struct ListWrapper
    {
		public List<QuestManager.QuestProgress> questList;
    }
}

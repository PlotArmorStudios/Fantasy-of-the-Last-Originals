using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SocialPlatforms.Impl;

public class QuestManager : MonoBehaviour
{
    static private QuestManager Instance;

    public QuestRecord[] QuestRecords;

    public Quest[] Quests;

    private Dictionary<Quest.QuestType, QuestRecord> QuestDictionary;

    private void Awake()
    {
        Instance = this;
        
        QuestDictionary = new Dictionary<Quest.QuestType, QuestRecord>();
      
        foreach (QuestRecord questRecord in QuestRecords)
        {
            QuestDictionary.Add(questRecord.type, questRecord);
        }
    }

    //static private QuestManager QuestManagerInstance => Instance;
        
    public void QuestProgress(Quest.QuestType quest, int num = 1)
    {
        QuestRecord questRecord = QuestDictionary[quest];

        if (questRecord == null) return;

        questRecord.Progress(num);

        foreach (Quest currentQuest in Instance.Quests)
        {
            if (!currentQuest.Complete)
            {
                if (currentQuest.CheckCompletion(quest, questRecord.Number))
                {
                    AnnounceQuestCompletion(currentQuest);
                }
            }
        }
    }

    private void AnnounceQuestCompletion(Quest currentQuest)
    {
        Debug.Log("Quest Complete!");
    }
}

[System.Serializable]
public class QuestRecord
{
    public Quest.QuestType type;
    public int _num;
    public bool _cumulative;

    public int Number => _num;

    public void Progress(int num)
    {
        if (_cumulative)
        {
            _num += num;
        }
        else
        {
            _num = num;
        }
    }
}

[System.Serializable]
public class Quest
{
    public enum QuestType
    {
        CollectTrollFruit,
    }

    public string _name;
    public string _description;
    public int _progressCount;
    public QuestType type;
    
    [SerializeField]
    private bool _complete;

    public bool Complete
    {
        get => _complete;
        set => _complete = value;
    }

    public bool CheckCompletion(QuestType quest, int num)
    {
        if (quest != type || Complete)
        {
            return false;
        }

        if (num >= _progressCount)
        {
            Complete = true;
            return true;
        }

        return false;
    }
}
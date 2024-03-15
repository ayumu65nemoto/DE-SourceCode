using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaFhase3 : NPCController, IConversation
{
    //　好感度
    [SerializeField]
    private int _favRate = 0;
    // クエストクリア時の会話
    [SerializeField]
    private Conversation _conversation2;

    public int FavRate { get => _favRate; set => _favRate = value; }

    public Conversation GetConversation()
    {
        if (_questManager.QuestList[2].IsClear == true)
        {
            if (_questManager.QuestList[2].RewardHint.Count != 0)
            {
                foreach (var num in _questManager.QuestList[2].RewardHint)
                {
                    _talkManager.HintLists[num].IsHint = true;
                    _questManager.HappendQuestList.Remove(_questManager.QuestList[2]);
                    _questManager.ShowQuest();
                }
            }
            return _conversation2;
        }

        _questManager.QuestList[2].IsHappen = true;
        _questManager.HappendQuestList.Add(_questManager.QuestList[2]);
        _questManager.ShowQuest();
        return _conversation1;
    }

    public Conversation GetConversation2()
    {
        throw new System.NotImplementedException();
    }

    public Conversation GetConversation3()
    {
        throw new System.NotImplementedException();
    }

    public void FinishTalking()
    {
        //_tipsController.ShowTips(1);
    }
}

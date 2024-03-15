using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaFhase1 : NPCController, IConversation
{
    //�@�D���x
    [SerializeField]
    private int _favRate = 0;
    // �N�G�X�g�N���A���̉�b
    [SerializeField]
    private Conversation _conversation2;
    // �N�G�X�g�N���A�A�C�e��
    [SerializeField]
    private GameObject _keyItem;

    public int FavRate { get => _favRate; set => _favRate = value; }

    public Conversation GetConversation()
    {
        if (_questManager.QuestList[0].IsClear == true)
        {
            if (_questManager.QuestList[0].RewardHint.Count != 0)
            {
                foreach (var num in _questManager.QuestList[0].RewardHint)
                {
                    _talkManager.HintLists[num].IsHint = true;
                    _questManager.HappendQuestList.Remove(_questManager.QuestList[0]);
                    _questManager.ShowQuest();
                }
            }
            return _conversation2;
        }

        _questManager.QuestList[0].IsHappen = true;
        _questManager.HappendQuestList.Add(_questManager.QuestList[0]);
        _questManager.ShowQuest();
        _keyItem.SetActive(true);
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

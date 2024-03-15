using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ameria : NPCController, IConversation
{
    //�@�D���x
    [SerializeField]
    private int _favRate = 0;
    [SerializeField]
    private Conversation _conversation0;
    [SerializeField]
    private Conversation _conversation1_2;
    // �N�G�X�g�N���A���̉�b
    [SerializeField]
    private Conversation _conversation2;
    // Tips���o�����ǂ���
    private bool _isTips = false;

    public int FavRate { get => _favRate; set => _favRate = value; }

    public Conversation GetConversation()
    {
        if (_questManager.QuestList[2].IsHappen == true)
        {
            _isTips = true;
            if (GameManager.gameManager.AmeriaCount > 0)
            {
                _questManager.QuestList[2].IsClear = true;
                _questManager.ShowQuest("�˗���ɕ񍐂��悤");
                return _conversation2;
            }
            else
            {
                return _conversation1;
            }
        }

        return _conversation0;
    }

    public Conversation GetConversation2()
    {
        _playerTalk.BgState = PlayerTalk.BG.Exploration;
        _playerTalk.CharPictureState = PlayerTalk.CharPicture.Normal;
        _playerTalk.IsCenterChar = true;
        return _conversation1_2;
    }

    public Conversation GetConversation3()
    {
        throw new System.NotImplementedException();
    }

    public void FinishTalking()
    {
        if (_isTips == true)
        {
            GameManager.gameManager.AmeriaCount += 1;
            //_tipsController.ShowTips(2);
        }
    }
}

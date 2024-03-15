using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordsObject : NPCController, IConversation
{
    //�@���C���V�i���I���X�g
    [SerializeField]
    private Conversation _chat;
    [SerializeField]
    private List<int> _translateNumList = new List<int>();

    public Conversation GetConversation()
    {
        foreach (var translateNum in _translateNumList)
        {
            _talkManager.Vocabulary[translateNum].SelectWord = _talkManager.Vocabulary[translateNum].Key;
        }

        //�@�w�i�E�����G�E�����G�ʒu��ݒ�
        _playerTalk.BgState = PlayerTalk.BG.Normal;
        _playerTalk.CharPictureState = PlayerTalk.CharPicture.Normal;
        _playerTalk.IsCenterChar = false;
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
        //throw new System.NotImplementedException();
    }
}

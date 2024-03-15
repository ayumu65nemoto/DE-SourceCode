using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : NPCController, IConversation
{
    //�@���C���V�i���I���X�g
    [SerializeField]
    private List<Conversation> _mainScenarioList = new List<Conversation>();

    public Conversation GetConversation()
    {
        //�@�ړI�@
        if (_gameManager.AllGameFlags["Chapter2_1_2"] == false && _gameManager.AllGameFlags["Chapter2_1_1"] == true)
        {
            //�@�w�i�E�����G�E�����G�ʒu��ݒ�
            _playerTalk.BgState = PlayerTalk.BG.Normal;
            _playerTalk.CharPictureState = PlayerTalk.CharPicture.Normal;
            _playerTalk.IsCenterChar = false;

            _gameManager.AllGameFlags["Chapter2_1_2"] = true;
            _playerTalk.NowChapter = "Chapter2_1_2";
            return _mainScenarioList[0];
        }

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

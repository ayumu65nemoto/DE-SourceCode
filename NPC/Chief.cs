using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chief : NPCController, IConversation
{
    //�@���C���V�i���I���X�g
    [SerializeField]
    private List<Conversation> _mainScenarioList = new List<Conversation>();

    public Conversation GetConversation()
    {
        //�@�ړI3-�C
        if (_gameManager.AllGameFlags["Chapter3_4_1"] == false && _gameManager.AllGameFlags["Chapter3_3_2"] == true)
        {
            //�@�w�i�E�����G�E�����G�ʒu��ݒ�
            _playerTalk.BgState = PlayerTalk.BG.Normal;
            _playerTalk.CharPictureState = PlayerTalk.CharPicture.Normal;
            _playerTalk.IsCenterChar = false;

            _gameManager.AllGameFlags["Chapter3_4_1"] = true;
            _playerTalk.NowChapter = "Chapter3_4_1";
            return _mainScenarioList[0];
        }
        //�@3-�G���f�B���O
        else if (_gameManager.AllGameFlags["Chapter3_ed_1"] == false 
            && _gameManager.AllGameFlags["Chapter3_4_3"] == true
            && _talkManager.Vocabulary[18].SelectWord != ""
            && _talkManager.Vocabulary[19].SelectWord != ""
            && _talkManager.Vocabulary[20].SelectWord != ""
            && _talkManager.Vocabulary[21].SelectWord != ""
            && _talkManager.Vocabulary[22].SelectWord != ""
            && _talkManager.Vocabulary[23].SelectWord != ""
            && _talkManager.Vocabulary[24].SelectWord != ""
            && _talkManager.Vocabulary[25].SelectWord != ""
            && _talkManager.Vocabulary[26].SelectWord != ""
            && _talkManager.Vocabulary[27].SelectWord != "")
        {
            //�@�w�i�E�����G�E�����G�ʒu��ݒ�
            _playerTalk.BgState = PlayerTalk.BG.Normal;
            _playerTalk.CharPictureState = PlayerTalk.CharPicture.Normal;
            _playerTalk.IsCenterChar = false;

            _gameManager.AllGameFlags["Chapter3_ed_1"] = true;
            _playerTalk.NowChapter = "Chapter3_ed_1";
            return _mainScenarioList[3];
        }

        return _conversation1;
    }

    public Conversation GetConversation2()
    {
        //�@�ړI3-�C
        if (_gameManager.AllGameFlags["Chapter3_4_2"] == false && _gameManager.AllGameFlags["Chapter3_4_1"] == true)
        {
            //�@�w�i�E�����G�E�����G�ʒu��ݒ�
            _playerTalk.BgState = PlayerTalk.BG.Normal;
            _playerTalk.CharPictureState = PlayerTalk.CharPicture.Sadness;
            _playerTalk.IsCenterChar = false;

            _gameManager.AllGameFlags["Chapter3_4_2"] = true;
            _playerTalk.NowChapter = "Chapter3_4_2";
            return _mainScenarioList[1];
        }

        return _conversation1;
    }

    public Conversation GetConversation3()
    {
        //�@�ړI3-�C
        if (_gameManager.AllGameFlags["Chapter3_4_3"] == false && _gameManager.AllGameFlags["Chapter3_4_2"] == true)
        {
            //�@�w�i�E�����G�E�����G�ʒu��ݒ�
            _playerTalk.BgState = PlayerTalk.BG.Normal;
            _playerTalk.CharPictureState = PlayerTalk.CharPicture.Joy;
            _playerTalk.IsCenterChar = false;

            _gameManager.AllGameFlags["Chapter3_4_3"] = true;
            _playerTalk.NowChapter = "Chapter3_4_3";
            return _mainScenarioList[2];
        }

        return _conversation1;
    }

    public void FinishTalking()
    {
        //throw new System.NotImplementedException();
    }
}

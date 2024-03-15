using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookShelf : NPCController, IConversation
{
    //�@���C���V�i���I���X�g
    [SerializeField]
    private List<Conversation> _mainScenarioList = new List<Conversation>();

    public Conversation GetConversation()
    {
        //�@�ړI�B
        if (_gameManager.AllGameFlags["Chapter1_3_2"] == false && _gameManager.AllGameFlags["Chapter1_3_1"] == true)
        {
            //�@�w�i�E�����G�E�����G�ʒu��ݒ�
            _playerTalk.BgState = PlayerTalk.BG.Normal;
            _playerTalk.CharPictureState = PlayerTalk.CharPicture.Normal;
            _playerTalk.IsCenterChar = false;

            _gameManager.AllGameFlags["Chapter1_3_2"] = true;
            _playerTalk.NowChapter = "Chapter1_3_2";
            _talkManager.Vocabulary[0].SelectWord = _talkManager.Vocabulary[0].Key;
            _talkManager.Vocabulary[3].SelectWord = _talkManager.Vocabulary[3].Key;
            _talkManager.Vocabulary[4].SelectWord = _talkManager.Vocabulary[4].Key;
            _talkManager.Vocabulary[5].SelectWord = _talkManager.Vocabulary[5].Key;
            _talkManager.Vocabulary[6].SelectWord = _talkManager.Vocabulary[6].Key;
            return _mainScenarioList[0];
        }
        //�@�ړI�C
        else if (_gameManager.AllGameFlags["Chapter1_4_1"] == false && _gameManager.AllGameFlags["Chapter1_3_2"] == true)
        {
            //�@�w�i�E�����G�E�����G�ʒu��ݒ�
            _playerTalk.BgState = PlayerTalk.BG.Normal;
            _playerTalk.CharPictureState = PlayerTalk.CharPicture.Normal;
            _playerTalk.IsCenterChar = false;

            _gameManager.AllGameFlags["Chapter1_4_1"] = true;
            _playerTalk.NowChapter = "Chapter1_4_1";
            _talkManager.Vocabulary[15].SelectWord = _talkManager.Vocabulary[15].Key;
            _talkManager.Vocabulary[16].SelectWord = _talkManager.Vocabulary[16].Key;
            _talkManager.Vocabulary[17].SelectWord = _talkManager.Vocabulary[17].Key;
            return _mainScenarioList[1];
        }
        //�@�ړI2-�A
        else if (_gameManager.AllGameFlags["Chapter2_2_1"] == false && _gameManager.AllGameFlags["Chapter2_1_3"] == true)
        {
            //�@�w�i�E�����G�E�����G�ʒu��ݒ�
            _playerTalk.BgState = PlayerTalk.BG.Normal;
            _playerTalk.CharPictureState = PlayerTalk.CharPicture.Normal;
            _playerTalk.IsCenterChar = false;

            _gameManager.AllGameFlags["Chapter2_2_1"] = true;
            _playerTalk.NowChapter = "Chapter2_2_1";
            _talkManager.Vocabulary[28].SelectWord = _talkManager.Vocabulary[28].Key;
            _talkManager.Vocabulary[29].SelectWord = _talkManager.Vocabulary[29].Key;
            _talkManager.Vocabulary[30].SelectWord = _talkManager.Vocabulary[30].Key;
            _talkManager.Vocabulary[25].SelectWord = _talkManager.Vocabulary[25].Key;
            _talkManager.Vocabulary[19].SelectWord = _talkManager.Vocabulary[19].Key;
            return _mainScenarioList[2];
        }
        //�@�ړI3-�B
        else if (_gameManager.AllGameFlags["Chapter3_3_1"] == false && _gameManager.AllGameFlags["Chapter3_2_1"] == true)
        {
            //�@�w�i�E�����G�E�����G�ʒu��ݒ�
            _playerTalk.BgState = PlayerTalk.BG.Normal;
            _playerTalk.CharPictureState = PlayerTalk.CharPicture.Normal;
            _playerTalk.IsCenterChar = false;

            _gameManager.AllGameFlags["Chapter3_3_1"] = true;
            _playerTalk.NowChapter = "Chapter3_3_1";
            return _mainScenarioList[3];
        }

        //�@�w�i�E�����G�E�����G�ʒu��ݒ�
        _playerTalk.BgState = PlayerTalk.BG.Normal;
        _playerTalk.CharPictureState = PlayerTalk.CharPicture.None;
        _playerTalk.IsCenterChar = false;
        return _conversation1;
    }

    public Conversation GetConversation2()
    {
        //�@�ړI3-�B
        if (_gameManager.AllGameFlags["Chapter3_3_2"] == false && _gameManager.AllGameFlags["Chapter3_3_1"] == true)
        {
            //�@�w�i�E�����G�E�����G�ʒu��ݒ�
            _playerTalk.BgState = PlayerTalk.BG.Normal;
            _playerTalk.CharPictureState = PlayerTalk.CharPicture.Joy;
            _playerTalk.IsCenterChar = false;

            _gameManager.AllGameFlags["Chapter3_3_2"] = true;
            _playerTalk.NowChapter = "Chapter3_3_2";
            return _mainScenarioList[4];
        }

        return _conversation1;
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

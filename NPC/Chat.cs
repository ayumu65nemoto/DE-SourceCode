using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Chat : NPCController, IConversation
{
    //�@���C���V�i���I���X�g
    [SerializeField]
    private List<Conversation> _chapter1ScenarioList = new List<Conversation>();
    [SerializeField]
    private List<Conversation> _chapter2ScenarioList = new List<Conversation>();
    [SerializeField]
    private List<Conversation> _chapter3ScenarioList = new List<Conversation>();
    //�@�G�k���X�g
    [SerializeField]
    private List<Conversation> _chatList1 = new List<Conversation>();
    [SerializeField]
    private List<Conversation> _chatList2 = new List<Conversation>();
    //�@�G�k�ԍ�
    private int _chatNum = 0;
    private int _chatNum2 = 0;
    //�@�����b
    private bool _isActiveChase = false;
    //�@Tips�P
    private bool _tips1 = false;

    public Conversation GetConversation()
    {
        //�@�擾�^�C�~���O������Ȃ��̂�
        if (_gameManager == null)
        {
            _gameManager = GameManager.gameManager;
        }
        if (_playerTalk == null)
        {
            _playerTalk = PlayerTalk.instance;
            _playerTalk.Initialize();
        }

        if (_gameManager.Favorability < 3)
        {
            //�@�Q�[���J�n��
            if (_gameManager.AllGameFlags["Chapter1_op_1"] == false)
            {
                //�@�w�i�E�����G�E�����G�ʒu��ݒ�
                _playerTalk.BgState = PlayerTalk.BG.Normal;
                _playerTalk.CharPictureState = PlayerTalk.CharPicture.Normal;
                _playerTalk.IsCenterChar = false;

                _gameManager.AllGameFlags["Chapter1_op_1"] = true;
                _playerTalk.NowChapter = "Chapter1_op_1";
                return _chapter1ScenarioList[0];
            }
            //�@�ړI�@�����ɘb��������
            else if (_gameManager.AllGameFlags["Chapter1_1_1"] == false && _gameManager.AllGameFlags["Chapter1_op_1"] == true)
            {
                //�@�w�i�E�����G�E�����G�ʒu��ݒ�
                _playerTalk.BgState = PlayerTalk.BG.Normal;
                _playerTalk.CharPictureState = PlayerTalk.CharPicture.Normal;
                _playerTalk.IsCenterChar = false;

                _gameManager.AllGameFlags["Chapter1_1_1"] = true;
                _playerTalk.NowChapter = "Chapter1_1_1";
                _talkManager.HintLists[1].IsHint = true;
                _tips1 = true;
                return _chapter1ScenarioList[1];
            }
            //�@�ړI�A
            else if (_gameManager.AllGameFlags["Chapter1_2_1"] == false && _gameManager.AllGameFlags["Chapter1_1_1"] == true && SceneManager.GetActiveScene().name == "Map1")
            {
                //�@�w�i�E�����G�E�����G�ʒu��ݒ�
                _playerTalk.BgState = PlayerTalk.BG.Normal;
                _playerTalk.CharPictureState = PlayerTalk.CharPicture.Normal;
                _playerTalk.IsCenterChar = false;

                _gameManager.AllGameFlags["Chapter1_2_1"] = true;
                _playerTalk.NowChapter = "Chapter1_2_1";
                _tips1 = true;
                return _chapter1ScenarioList[2];
            }
            //�@�����ɓ����Ă����ԑO��Ȃ̂ŁA����ȊO�Ȃ�G�k��Ԃ�����
            else if (_gameManager.AllGameFlags["Chapter1_2_2"] == false && _gameManager.AllGameFlags["Chapter1_2_1"] == true && SceneManager.GetActiveScene().name == "Room")
            {
                //�@�w�i�E�����G�E�����G�ʒu��ݒ�
                _playerTalk.BgState = PlayerTalk.BG.Normal;
                _playerTalk.CharPictureState = PlayerTalk.CharPicture.Normal;
                _playerTalk.IsCenterChar = false;

                _gameManager.AllGameFlags["Chapter1_2_2"] = true;
                _playerTalk.NowChapter = "Chapter1_2_2";
                _talkManager.HintLists[2].IsHint = true;
                _tips1 = true;
                return _chapter1ScenarioList[3];
            }
            //�@�ړI�B
            else if (_gameManager.AllGameFlags["Chapter1_3_1"] == false && _gameManager.AllGameFlags["Chapter1_2_2"] == true)
            {
                //�@�w�i�E�����G�E�����G�ʒu��ݒ�
                _playerTalk.BgState = PlayerTalk.BG.Normal;
                _playerTalk.CharPictureState = PlayerTalk.CharPicture.Normal;
                _playerTalk.IsCenterChar = false;

                _gameManager.AllGameFlags["Chapter1_3_1"] = true;
                _playerTalk.NowChapter = "Chapter1_3_1";
                return _chapter1ScenarioList[4];
            }
            //�@�G���f�B���O
            else if (_gameManager.AllGameFlags["Chapter1_ed_1"] == false 
                && _gameManager.AllGameFlags["Chapter1_3_2"] == true
                && _talkManager.Vocabulary[3].SelectWord != ""
                && _talkManager.Vocabulary[4].SelectWord != ""
                && _talkManager.Vocabulary[5].SelectWord != ""
                && _talkManager.Vocabulary[0].SelectWord != ""
                && _talkManager.Vocabulary[2].SelectWord != "")
            {
                //�@�w�i�E�����G�E�����G�ʒu��ݒ�
                _playerTalk.BgState = PlayerTalk.BG.Normal;
                _playerTalk.CharPictureState = PlayerTalk.CharPicture.Fear;
                _playerTalk.IsCenterChar = false;

                _gameManager.AllGameFlags["Chapter1_ed_1"] = true;
                _playerTalk.NowChapter = "Chapter1_ed_1";
                return _chapter1ScenarioList[7];
            }

            if (_gameManager.AllGameFlags["Chapter1_3_2"] == true)
            {
                //�@�G�k�\���
                if (_chatNum == 0)
                {
                    _playerTalk.CharPictureState = PlayerTalk.CharPicture.Disgust;
                }
                else if (_chatNum == 1)
                {
                    _playerTalk.CharPictureState = PlayerTalk.CharPicture.Anger;
                }
                else
                {
                    _playerTalk.CharPictureState = PlayerTalk.CharPicture.Normal;
                }
                _playerTalk.BgState = PlayerTalk.BG.Normal;
                _playerTalk.IsCenterChar = false;
                _playerTalk.NowChapter = "";
                return _chatList1[_chatNum];
            }
            else
            {
                return _conversation1;
            }
        }
        else if (_gameManager.Favorability < 5)
        {
            if (_gameManager.AllGameFlags["Chapter2_op_1"] == false)
            {
                //�@�w�i�E�����G�E�����G�ʒu��ݒ�
                _playerTalk.BgState = PlayerTalk.BG.Normal;
                _playerTalk.CharPictureState = PlayerTalk.CharPicture.Normal;
                _playerTalk.IsCenterChar = false;

                _gameManager.AllGameFlags["Chapter2_op_1"] = true;
                _playerTalk.NowChapter = "Chapter2_op_1";
                return _chapter2ScenarioList[0];
            }
            //�@�ړI�@
            else if (_gameManager.AllGameFlags["Chapter2_1_1"] == false && _gameManager.AllGameFlags["Chapter2_op_1"] == true)
            {
                //�@�w�i�E�����G�E�����G�ʒu��ݒ�
                _playerTalk.BgState = PlayerTalk.BG.Normal;
                _playerTalk.CharPictureState = PlayerTalk.CharPicture.Normal;
                _playerTalk.IsCenterChar = false;

                _gameManager.AllGameFlags["Chapter2_1_1"] = true;
                _playerTalk.NowChapter = "Chapter2_1_1";
                return _chapter2ScenarioList[1];
            }
            //�@�Ԃ�n������
            else if (_gameManager.AllGameFlags["Chapter2_1_3"] == false && _gameManager.AllGameFlags["Chapter2_1_2"] == true)
            {
                //�@�w�i�E�����G�E�����G�ʒu��ݒ�
                _playerTalk.BgState = PlayerTalk.BG.Normal;
                _playerTalk.CharPictureState = PlayerTalk.CharPicture.Normal;
                _playerTalk.IsCenterChar = false;

                _gameManager.AllGameFlags["Chapter2_1_3"] = true;
                _playerTalk.NowChapter = "Chapter2_1_3";
                _talkManager.HintLists[10].IsHint = true;
                return _chapter2ScenarioList[3];
            }
            //�@����̒P����l�����Ă����i�q���gor�|��ǂ����H�j�ꍇ�ɕ���
            else if (_gameManager.AllGameFlags["Chapter2_ed_1"] == false 
                && _gameManager.AllGameFlags["Chapter2_2_1"] == true 
                && SceneManager.GetActiveScene().name == "Map1"
                && _talkManager.Vocabulary[7].SelectWord != ""
                && _talkManager.Vocabulary[8].SelectWord != ""
                && _talkManager.Vocabulary[9].SelectWord != ""
                && _talkManager.Vocabulary[10].SelectWord != ""
                && _talkManager.Vocabulary[11].SelectWord != ""
                && _talkManager.Vocabulary[12].SelectWord != ""
                && _talkManager.Vocabulary[13].SelectWord != ""
                && _talkManager.Vocabulary[14].SelectWord != "")
            {
                //�@�w�i�E�����G�E�����G�ʒu��ݒ�
                _playerTalk.BgState = PlayerTalk.BG.Normal;
                _playerTalk.CharPictureState = PlayerTalk.CharPicture.Normal;
                _playerTalk.IsCenterChar = false;

                _gameManager.AllGameFlags["Chapter2_ed_1"] = true;
                _playerTalk.NowChapter = "Chapter2_ed_1";
                _gameManager.Favorability += 2;
                return _chapter2ScenarioList[5];
            }

            //�@�G�k�\���
            if (_chatNum2 == 0)
            {
                _playerTalk.CharPictureState = PlayerTalk.CharPicture.Sadness;
            }
            else if (_chatNum2 == 1)
            {
                _playerTalk.CharPictureState = PlayerTalk.CharPicture.Anger;
            }
            else if (_chatNum2 == 2)
            {
                _playerTalk.CharPictureState = PlayerTalk.CharPicture.Joy;
            }
            else
            {
                _playerTalk.CharPictureState = PlayerTalk.CharPicture.Normal;
            }
            _playerTalk.BgState = PlayerTalk.BG.Normal;
            _playerTalk.IsCenterChar = false;
            _playerTalk.NowChapter = "";
            return _chatList2[_chatNum2];
        }
        else
        {
            if (_gameManager.AllGameFlags["Chapter3_op_1"] == false)
            {
                //�@�w�i�E�����G�E�����G�ʒu��ݒ�
                _playerTalk.BgState = PlayerTalk.BG.Normal;
                _playerTalk.CharPictureState = PlayerTalk.CharPicture.Normal;
                _playerTalk.IsCenterChar = false;

                _gameManager.AllGameFlags["Chapter3_op_1"] = true;
                _playerTalk.NowChapter = "Chapter3_op_1";
                return _chapter3ScenarioList[0];
            }
            //�@�ړI�@�����ɘb��������
            else if (_gameManager.AllGameFlags["Chapter3_1_1"] == false && _gameManager.AllGameFlags["Chapter3_op_1"] == true)
            {
                //�@�w�i�E�����G�E�����G�ʒu��ݒ�
                _playerTalk.BgState = PlayerTalk.BG.Normal;
                _playerTalk.CharPictureState = PlayerTalk.CharPicture.Normal;
                _playerTalk.IsCenterChar = false;

                _gameManager.AllGameFlags["Chapter3_1_1"] = true;
                _playerTalk.NowChapter = "Chapter3_1_1";
                return _chapter3ScenarioList[1];
            }
            //�@��������������
            else if (_gameManager.AllGameFlags["Chapter3_1_2"] == false && _gameManager.AllGameFlags["Chapter3_1_1"] == true)
            {
                //�@�w�i�E�����G�E�����G�ʒu��ݒ�
                _playerTalk.BgState = PlayerTalk.BG.Normal;
                _playerTalk.CharPictureState = PlayerTalk.CharPicture.Sadness;
                _playerTalk.IsCenterChar = false;

                _gameManager.AllGameFlags["Chapter3_1_2"] = true;
                _playerTalk.NowChapter = "Chapter3_1_2";
                return _chapter3ScenarioList[2];
            }
            //�@�ړI�A
            else if (_gameManager.AllGameFlags["Chapter3_2_1"] == false && _gameManager.AllGameFlags["Chapter3_1_2"] == true)
            {
                //�@�w�i�E�����G�E�����G�ʒu��ݒ�
                _playerTalk.BgState = PlayerTalk.BG.Normal;
                _playerTalk.CharPictureState = PlayerTalk.CharPicture.Normal;
                _playerTalk.IsCenterChar = false;

                _gameManager.AllGameFlags["Chapter3_2_1"] = true;
                _playerTalk.NowChapter = "Chapter3_2_1";
                return _chapter3ScenarioList[3];
            }

            if (_chatNum2 == 0)
            {
                _playerTalk.CharPictureState = PlayerTalk.CharPicture.Sadness;
            }
            else if (_chatNum2 == 1)
            {
                _playerTalk.CharPictureState = PlayerTalk.CharPicture.Anger;
            }
            else if (_chatNum2 == 2)
            {
                _playerTalk.CharPictureState = PlayerTalk.CharPicture.Joy;
            }
            else
            {
                _playerTalk.CharPictureState = PlayerTalk.CharPicture.Normal;
            }
            _playerTalk.BgState = PlayerTalk.BG.Normal;
            _playerTalk.IsCenterChar = false;
            _playerTalk.NowChapter = "";
            return _chatList2[_chatNum2];
        }
    }

    public Conversation GetConversation2()
    {
        if (_gameManager.AllGameFlags["Chapter1_ed_2"] == false && _gameManager.AllGameFlags["Chapter1_ed_1"] == true)
        {
            //�@�w�i�E�����G�E�����G�ʒu��ݒ�
            _playerTalk.BgState = PlayerTalk.BG.Normal;
            _playerTalk.CharPictureState = PlayerTalk.CharPicture.Joy;
            _playerTalk.IsCenterChar = false;

            _gameManager.AllGameFlags["Chapter1_ed_2"] = true;
            _playerTalk.NowChapter = "Chapter1_ed_2";
            return _chapter1ScenarioList[8];
        }

        _playerTalk.BgState = PlayerTalk.BG.Normal;
        _playerTalk.CharPictureState = PlayerTalk.CharPicture.Normal;
        _playerTalk.IsCenterChar = false;
        _playerTalk.NowChapter = "";
        return _chatList1[_chatNum];
    }

    public Conversation GetConversation3()
    {
        throw new System.NotImplementedException();
    }

    public void FinishTalking()
    {
        if (_isActiveChase == false)
        {
            ChasePlayer chasePlayer = GetComponent<ChasePlayer>();
            if (chasePlayer != null)
            {
                chasePlayer.enabled = true;
            }
        }

        if (_tips1 == true)
        {
            _tipsController.ShowTips(0);
        }

        _chatNum = Random.Range(0, _chatList1.Count);
        _chatNum2 = Random.Range(0, _chatList2.Count);
    }
}

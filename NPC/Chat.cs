using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Chat : NPCController, IConversation
{
    //@ƒƒCƒ“ƒVƒiƒŠƒIƒŠƒXƒg
    [SerializeField]
    private List<Conversation> _chapter1ScenarioList = new List<Conversation>();
    [SerializeField]
    private List<Conversation> _chapter2ScenarioList = new List<Conversation>();
    [SerializeField]
    private List<Conversation> _chapter3ScenarioList = new List<Conversation>();
    //@G’kƒŠƒXƒg
    [SerializeField]
    private List<Conversation> _chatList1 = new List<Conversation>();
    [SerializeField]
    private List<Conversation> _chatList2 = new List<Conversation>();
    //@G’k”Ô†
    private int _chatNum = 0;
    private int _chatNum2 = 0;
    //@‰‰ñ‰ï˜b
    private bool _isActiveChase = false;
    //@Tips‚P
    private bool _tips1 = false;

    public Conversation GetConversation()
    {
        //@æ“¾ƒ^ƒCƒ~ƒ“ƒO‚ª‡‚í‚È‚¢‚Ì‚Å
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
            //@ƒQ[ƒ€ŠJn
            if (_gameManager.AllGameFlags["Chapter1_op_1"] == false)
            {
                //@”wŒiE—§‚¿ŠGE—§‚¿ŠGˆÊ’u‚ğİ’è
                _playerTalk.BgState = PlayerTalk.BG.Normal;
                _playerTalk.CharPictureState = PlayerTalk.CharPicture.Normal;
                _playerTalk.IsCenterChar = false;

                _gameManager.AllGameFlags["Chapter1_op_1"] = true;
                _playerTalk.NowChapter = "Chapter1_op_1";
                return _chapter1ScenarioList[0];
            }
            //@–Ú“I‡@­—‚É˜b‚µ‚©‚¯‚é
            else if (_gameManager.AllGameFlags["Chapter1_1_1"] == false && _gameManager.AllGameFlags["Chapter1_op_1"] == true)
            {
                //@”wŒiE—§‚¿ŠGE—§‚¿ŠGˆÊ’u‚ğİ’è
                _playerTalk.BgState = PlayerTalk.BG.Normal;
                _playerTalk.CharPictureState = PlayerTalk.CharPicture.Normal;
                _playerTalk.IsCenterChar = false;

                _gameManager.AllGameFlags["Chapter1_1_1"] = true;
                _playerTalk.NowChapter = "Chapter1_1_1";
                _talkManager.HintLists[1].IsHint = true;
                _tips1 = true;
                return _chapter1ScenarioList[1];
            }
            //@–Ú“I‡A
            else if (_gameManager.AllGameFlags["Chapter1_2_1"] == false && _gameManager.AllGameFlags["Chapter1_1_1"] == true && SceneManager.GetActiveScene().name == "Map1")
            {
                //@”wŒiE—§‚¿ŠGE—§‚¿ŠGˆÊ’u‚ğİ’è
                _playerTalk.BgState = PlayerTalk.BG.Normal;
                _playerTalk.CharPictureState = PlayerTalk.CharPicture.Normal;
                _playerTalk.IsCenterChar = false;

                _gameManager.AllGameFlags["Chapter1_2_1"] = true;
                _playerTalk.NowChapter = "Chapter1_2_1";
                _tips1 = true;
                return _chapter1ScenarioList[2];
            }
            //@•”‰®‚É“ü‚Á‚Ä‚¢‚éó‘Ô‘O’ñ‚È‚Ì‚ÅA‚»‚êˆÈŠO‚È‚çG’k‚ğ•Ô‚µ‚½‚¢
            else if (_gameManager.AllGameFlags["Chapter1_2_2"] == false && _gameManager.AllGameFlags["Chapter1_2_1"] == true && SceneManager.GetActiveScene().name == "Room")
            {
                //@”wŒiE—§‚¿ŠGE—§‚¿ŠGˆÊ’u‚ğİ’è
                _playerTalk.BgState = PlayerTalk.BG.Normal;
                _playerTalk.CharPictureState = PlayerTalk.CharPicture.Normal;
                _playerTalk.IsCenterChar = false;

                _gameManager.AllGameFlags["Chapter1_2_2"] = true;
                _playerTalk.NowChapter = "Chapter1_2_2";
                _talkManager.HintLists[2].IsHint = true;
                _tips1 = true;
                return _chapter1ScenarioList[3];
            }
            //@–Ú“I‡B
            else if (_gameManager.AllGameFlags["Chapter1_3_1"] == false && _gameManager.AllGameFlags["Chapter1_2_2"] == true)
            {
                //@”wŒiE—§‚¿ŠGE—§‚¿ŠGˆÊ’u‚ğİ’è
                _playerTalk.BgState = PlayerTalk.BG.Normal;
                _playerTalk.CharPictureState = PlayerTalk.CharPicture.Normal;
                _playerTalk.IsCenterChar = false;

                _gameManager.AllGameFlags["Chapter1_3_1"] = true;
                _playerTalk.NowChapter = "Chapter1_3_1";
                return _chapter1ScenarioList[4];
            }
            //@ƒGƒ“ƒfƒBƒ“ƒO
            else if (_gameManager.AllGameFlags["Chapter1_ed_1"] == false 
                && _gameManager.AllGameFlags["Chapter1_3_2"] == true
                && _talkManager.Vocabulary[3].SelectWord != ""
                && _talkManager.Vocabulary[4].SelectWord != ""
                && _talkManager.Vocabulary[5].SelectWord != ""
                && _talkManager.Vocabulary[0].SelectWord != ""
                && _talkManager.Vocabulary[2].SelectWord != "")
            {
                //@”wŒiE—§‚¿ŠGE—§‚¿ŠGˆÊ’u‚ğİ’è
                _playerTalk.BgState = PlayerTalk.BG.Normal;
                _playerTalk.CharPictureState = PlayerTalk.CharPicture.Fear;
                _playerTalk.IsCenterChar = false;

                _gameManager.AllGameFlags["Chapter1_ed_1"] = true;
                _playerTalk.NowChapter = "Chapter1_ed_1";
                return _chapter1ScenarioList[7];
            }

            if (_gameManager.AllGameFlags["Chapter1_3_2"] == true)
            {
                //@G’k•\î·•ª
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
                //@”wŒiE—§‚¿ŠGE—§‚¿ŠGˆÊ’u‚ğİ’è
                _playerTalk.BgState = PlayerTalk.BG.Normal;
                _playerTalk.CharPictureState = PlayerTalk.CharPicture.Normal;
                _playerTalk.IsCenterChar = false;

                _gameManager.AllGameFlags["Chapter2_op_1"] = true;
                _playerTalk.NowChapter = "Chapter2_op_1";
                return _chapter2ScenarioList[0];
            }
            //@–Ú“I‡@
            else if (_gameManager.AllGameFlags["Chapter2_1_1"] == false && _gameManager.AllGameFlags["Chapter2_op_1"] == true)
            {
                //@”wŒiE—§‚¿ŠGE—§‚¿ŠGˆÊ’u‚ğİ’è
                _playerTalk.BgState = PlayerTalk.BG.Normal;
                _playerTalk.CharPictureState = PlayerTalk.CharPicture.Normal;
                _playerTalk.IsCenterChar = false;

                _gameManager.AllGameFlags["Chapter2_1_1"] = true;
                _playerTalk.NowChapter = "Chapter2_1_1";
                return _chapter2ScenarioList[1];
            }
            //@‰Ô‚ğ“n‚µ‚½Œã
            else if (_gameManager.AllGameFlags["Chapter2_1_3"] == false && _gameManager.AllGameFlags["Chapter2_1_2"] == true)
            {
                //@”wŒiE—§‚¿ŠGE—§‚¿ŠGˆÊ’u‚ğİ’è
                _playerTalk.BgState = PlayerTalk.BG.Normal;
                _playerTalk.CharPictureState = PlayerTalk.CharPicture.Normal;
                _playerTalk.IsCenterChar = false;

                _gameManager.AllGameFlags["Chapter2_1_3"] = true;
                _playerTalk.NowChapter = "Chapter2_1_3";
                _talkManager.HintLists[10].IsHint = true;
                return _chapter2ScenarioList[3];
            }
            //@“Á’è‚Ì’PŒê‚ğŠl“¾‚µ‚Ä‚¢‚½iƒqƒ“ƒgor–|–ó‚Ç‚Á‚¿Hjê‡‚É•ªŠò
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
                //@”wŒiE—§‚¿ŠGE—§‚¿ŠGˆÊ’u‚ğİ’è
                _playerTalk.BgState = PlayerTalk.BG.Normal;
                _playerTalk.CharPictureState = PlayerTalk.CharPicture.Normal;
                _playerTalk.IsCenterChar = false;

                _gameManager.AllGameFlags["Chapter2_ed_1"] = true;
                _playerTalk.NowChapter = "Chapter2_ed_1";
                _gameManager.Favorability += 2;
                return _chapter2ScenarioList[5];
            }

            //@G’k•\î·•ª
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
                //@”wŒiE—§‚¿ŠGE—§‚¿ŠGˆÊ’u‚ğİ’è
                _playerTalk.BgState = PlayerTalk.BG.Normal;
                _playerTalk.CharPictureState = PlayerTalk.CharPicture.Normal;
                _playerTalk.IsCenterChar = false;

                _gameManager.AllGameFlags["Chapter3_op_1"] = true;
                _playerTalk.NowChapter = "Chapter3_op_1";
                return _chapter3ScenarioList[0];
            }
            //@–Ú“I‡@­—‚É˜b‚µ‚©‚¯‚é
            else if (_gameManager.AllGameFlags["Chapter3_1_1"] == false && _gameManager.AllGameFlags["Chapter3_op_1"] == true)
            {
                //@”wŒiE—§‚¿ŠGE—§‚¿ŠGˆÊ’u‚ğİ’è
                _playerTalk.BgState = PlayerTalk.BG.Normal;
                _playerTalk.CharPictureState = PlayerTalk.CharPicture.Normal;
                _playerTalk.IsCenterChar = false;

                _gameManager.AllGameFlags["Chapter3_1_1"] = true;
                _playerTalk.NowChapter = "Chapter3_1_1";
                return _chapter3ScenarioList[1];
            }
            //@­—‚ª—‚¿‚Ş
            else if (_gameManager.AllGameFlags["Chapter3_1_2"] == false && _gameManager.AllGameFlags["Chapter3_1_1"] == true)
            {
                //@”wŒiE—§‚¿ŠGE—§‚¿ŠGˆÊ’u‚ğİ’è
                _playerTalk.BgState = PlayerTalk.BG.Normal;
                _playerTalk.CharPictureState = PlayerTalk.CharPicture.Sadness;
                _playerTalk.IsCenterChar = false;

                _gameManager.AllGameFlags["Chapter3_1_2"] = true;
                _playerTalk.NowChapter = "Chapter3_1_2";
                return _chapter3ScenarioList[2];
            }
            //@–Ú“I‡A
            else if (_gameManager.AllGameFlags["Chapter3_2_1"] == false && _gameManager.AllGameFlags["Chapter3_1_2"] == true)
            {
                //@”wŒiE—§‚¿ŠGE—§‚¿ŠGˆÊ’u‚ğİ’è
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
            //@”wŒiE—§‚¿ŠGE—§‚¿ŠGˆÊ’u‚ğİ’è
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

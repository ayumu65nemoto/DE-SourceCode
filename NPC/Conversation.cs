using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


[CreateAssetMenu(fileName = "Conversation", menuName = "CreateConversation")]
public class Conversation : ScriptableObject
{
    //　会話内容
    [SerializeField]
    [Multiline(100)]
    //表示する文章
    private string _message;
    //言語の組み合わせ
    [SerializeField] private string[] _langComb;
    //選択肢を選んだあとの文章と組み合わせ
    private string _firstMessage;
    private string _secondMessage;
    private string _thirdMessage;
    private List<string> _firstMessages = new List<string>();
    private List<string> _secondMesseages = new List<string>();
    private List<string> _thirdMesseages = new List<string>();
    [SerializeField] private string[] _firstLangComb;
    [SerializeField] private string[] _secondLangComb;
    [SerializeField] private string[] _thirdLangComb;
    //選択肢のメッセージと組み合わせ
    private List<string> _firstChoiseMessages = new List<string>();
    private List<string> _secondChoiseMessages = new List<string>();
    private List<string> _thirdChoiseMessages = new List<string>();
    [SerializeField] private string[] _firstChoiseComb;
    [SerializeField] private string[] _secondChoiseComb;
    [SerializeField] private string[] _thirdChoiseComb;
    //TalkManager
    private TalkManager _talkManager;
    //PlayerTalk
    private PlayerTalk _playerTalk;
    //WordImagePairのリスト
    private Dictionary<string, WordDictionary> _wordDictionary;

    public Dictionary<string, WordDictionary> WordDictionary { get => _wordDictionary; set => _wordDictionary = value; }

    //　会話内容を返す
    public string GetConversationMessage()
    {
        _talkManager = TalkManager.instance;
        _playerTalk = PlayerTalk.instance;

        //初期化
        _firstMessages = new List<string>();
        _secondMesseages = new List<string>();
        _thirdMesseages = new List<string>();
        _firstChoiseMessages = new List<string>();
        _secondChoiseMessages = new List<string>();
        _thirdChoiseMessages = new List<string>();

        WordDictionary = _talkManager.WordDictionary;
        ReturnText();
        return _message;
    }

    //会話全文を渡す
    public void ReturnText()
    {
        //普通の会話文
        _message = "";
        _firstMessage = "";
        _secondMessage = "";
        _thirdMessage = "";
        for (int i = 0; i < _langComb.Length; i++)
        {
            if (WordDictionary.ContainsKey(_langComb[i]))
            {
                if (WordDictionary[_langComb[i]].SelectWord != "")
                {
                    string text = WordDictionary[_langComb[i]].SelectWord;
                    _message += text;
                }
                else
                {
                    string text = WordDictionary[_langComb[i]].OriginalWord;
                    _message += text;
                }
            }
            else
            {
                string text = _langComb[i];
                _message += text;
            }
        }

        //選択肢1を選んだ後の会話文
        for (int i = 0; i < _firstLangComb.Length; i++)
        {
            if (WordDictionary.ContainsKey(_firstLangComb[i]))
            {
                if (WordDictionary[_firstLangComb[i]].SelectWord != "")
                {
                    _firstMessages.Add(WordDictionary[_firstLangComb[i]].SelectWord);
                    string text = WordDictionary[_firstLangComb[i]].SelectWord;
                    _firstMessage += text;
                }
                else
                {
                    _firstMessages.Add(WordDictionary[_firstLangComb[i]].OriginalWord);
                    string text = WordDictionary[_firstLangComb[i]].OriginalWord;
                    _firstMessage += text;
                }
            }
            else
            {
                _firstMessages.Add(_firstLangComb[i]);
                string text = _firstLangComb[i];
                _firstMessage += text;
            }
        }

        //選択肢2を選んだ後の会話文
        for (int i = 0; i < _secondLangComb.Length; i++)
        {
            if (WordDictionary.ContainsKey(_secondLangComb[i]))
            {
                if (WordDictionary[_secondLangComb[i]].SelectWord != "")
                {
                    _secondMesseages.Add(WordDictionary[_secondLangComb[i]].SelectWord);
                    string text = WordDictionary[_secondLangComb[i]].SelectWord;
                    _secondMessage += text;
                }
                else
                {
                    _secondMesseages.Add(WordDictionary[_secondLangComb[i]].OriginalWord);
                    string text = WordDictionary[_secondLangComb[i]].OriginalWord;
                    _secondMessage += text;
                }
            }
            else
            {
                _secondMesseages.Add(_secondLangComb[i]);
                string text = _secondLangComb[i];
                _secondMessage += text;
            }
        }

        //選択肢3を選んだ後の会話文
        for (int i = 0; i < _thirdLangComb.Length; i++)
        {
            if (WordDictionary.ContainsKey(_thirdLangComb[i]))
            {
                if (WordDictionary[_thirdLangComb[i]].SelectWord != "")
                {
                    _thirdMesseages.Add(WordDictionary[_thirdLangComb[i]].SelectWord);
                    string text = WordDictionary[_thirdLangComb[i]].SelectWord;
                    _thirdMessage += text;
                }
                else
                {
                    _thirdMesseages.Add(WordDictionary[_thirdLangComb[i]].OriginalWord);
                    string text = WordDictionary[_thirdLangComb[i]].OriginalWord;
                    _thirdMessage += text;
                }
            }
            else
            {
                _thirdMesseages.Add(_thirdLangComb[i]);
                string text = _thirdLangComb[i];
                _thirdMessage += text;
            }
        }

        //選択肢1のテキスト
        for (int i = 0; i < _firstChoiseComb.Length; i++)
        {
            if (WordDictionary.ContainsKey(_firstChoiseComb[i]))
            {
                if (WordDictionary[_firstChoiseComb[i]].SelectWord != "")
                {
                    _firstChoiseMessages.Add(WordDictionary[_firstChoiseComb[i]].SelectWord);
                }
                else
                {
                    _firstChoiseMessages.Add(WordDictionary[_firstChoiseComb[i]].OriginalWord);
                }
            }
            else
            {
                _firstChoiseMessages.Add(_firstChoiseComb[i]);
            }
        }

        //選択肢2のテキスト
        for (int i = 0; i < _secondChoiseComb.Length; i++)
        {
            if (WordDictionary.ContainsKey(_secondChoiseComb[i]))
            {
                if (WordDictionary[_secondChoiseComb[i]].SelectWord != "")
                {
                    _secondChoiseMessages.Add(WordDictionary[_secondChoiseComb[i]].SelectWord);
                }
                else
                {
                    _secondChoiseMessages.Add(WordDictionary[_secondChoiseComb[i]].OriginalWord);
                }
            }
            else
            {
                _secondChoiseMessages.Add(_secondChoiseComb[i]);
            }
        }

        //選択肢3のテキスト
        for (int i = 0; i < _thirdChoiseComb.Length; i++)
        {
            if (WordDictionary.ContainsKey(_thirdChoiseComb[i]))
            {
                if (WordDictionary[_thirdChoiseComb[i]].SelectWord != "")
                {
                    _thirdChoiseMessages.Add(WordDictionary[_thirdChoiseComb[i]].SelectWord);
                }
                else
                {
                    _thirdChoiseMessages.Add(WordDictionary[_thirdChoiseComb[i]].OriginalWord);
                }
            }
            else
            {
                _thirdChoiseMessages.Add(_thirdChoiseComb[i]);
            }
        }

        _playerTalk.SetTheTitleTextOfTheChoice(_firstMessages, _secondMesseages, _thirdMesseages, _firstChoiseMessages, _secondChoiseMessages, _thirdChoiseMessages);
    }

    //　１の会話を返す
    public string ReturnOne()
    {
        return _firstMessage;
    }

    //　２の会話を返す
    public string ReturnTwo()
    {
        return _secondMessage;
    }

    //　３の会話を返す
    public string ReturnThree()
    {
        return _thirdMessage;
    }
}

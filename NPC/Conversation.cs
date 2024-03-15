using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


[CreateAssetMenu(fileName = "Conversation", menuName = "CreateConversation")]
public class Conversation : ScriptableObject
{
    //�@��b���e
    [SerializeField]
    [Multiline(100)]
    //�\�����镶��
    private string _message;
    //����̑g�ݍ��킹
    [SerializeField] private string[] _langComb;
    //�I������I�񂾂��Ƃ̕��͂Ƒg�ݍ��킹
    private string _firstMessage;
    private string _secondMessage;
    private string _thirdMessage;
    private List<string> _firstMessages = new List<string>();
    private List<string> _secondMesseages = new List<string>();
    private List<string> _thirdMesseages = new List<string>();
    [SerializeField] private string[] _firstLangComb;
    [SerializeField] private string[] _secondLangComb;
    [SerializeField] private string[] _thirdLangComb;
    //�I�����̃��b�Z�[�W�Ƒg�ݍ��킹
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
    //WordImagePair�̃��X�g
    private Dictionary<string, WordDictionary> _wordDictionary;

    public Dictionary<string, WordDictionary> WordDictionary { get => _wordDictionary; set => _wordDictionary = value; }

    //�@��b���e��Ԃ�
    public string GetConversationMessage()
    {
        _talkManager = TalkManager.instance;
        _playerTalk = PlayerTalk.instance;

        //������
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

    //��b�S����n��
    public void ReturnText()
    {
        //���ʂ̉�b��
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

        //�I����1��I�񂾌�̉�b��
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

        //�I����2��I�񂾌�̉�b��
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

        //�I����3��I�񂾌�̉�b��
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

        //�I����1�̃e�L�X�g
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

        //�I����2�̃e�L�X�g
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

        //�I����3�̃e�L�X�g
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

    //�@�P�̉�b��Ԃ�
    public string ReturnOne()
    {
        return _firstMessage;
    }

    //�@�Q�̉�b��Ԃ�
    public string ReturnTwo()
    {
        return _secondMessage;
    }

    //�@�R�̉�b��Ԃ�
    public string ReturnThree()
    {
        return _thirdMessage;
    }
}

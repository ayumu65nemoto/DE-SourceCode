using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class WordDictionary
{
    [SerializeField]
    private string _key;
    [SerializeField]
    private string _originalWord;
    [SerializeField]
    private string _selectWord;

    public string OriginalWord { get => _originalWord; set => _originalWord = value; }
    public string SelectWord { get => _selectWord; set => _selectWord = value; }
    public string Key { get => _key; set => _key = value; }
}

[System.Serializable]
public class WordChoise
{
    [SerializeField]
    private string _word1;
    [SerializeField]
    private string _word2;
    [SerializeField]
    private string _word3;
    [SerializeField]
    private string _word4;
    [SerializeField]
    private string _word5;

    public string Word1 { get => _word1; set => _word1 = value; }
    public string Word2 { get => _word2; set => _word2 = value; }
    public string Word3 { get => _word3; set => _word3 = value; }
    public string Word4 { get => _word4; set => _word4 = value; }
    public string Word5 { get => _word5; set => _word5 = value; }
}

[System.Serializable]
public class HintList
{
    [SerializeField]
    private bool _isHint;
    [SerializeField]
    private Sprite _hint1;

    public bool IsHint { get => _isHint; set => _isHint = value; }
    public Sprite Hint1 { get => _hint1; set => _hint1 = value; }
}

public class TalkManager : MonoBehaviour
{
    //インスタンス化
    public static TalkManager instance;
    //単語の辞書
    [SerializeField]
    private Dictionary<string, WordDictionary> _wordDictionary = new Dictionary<string, WordDictionary>();
    //単語集
    [SerializeField]
    private List<WordDictionary> _vocabulary;
    //翻訳選択肢
    [SerializeField]
    private List<WordChoise> _wordChoises;
    //ヒントのリスト
    [SerializeField]
    private List<HintList> _hintLists;

    public List<WordChoise> WordChoises { get => _wordChoises; set => _wordChoises = value; }
    public List<HintList> HintLists { get => _hintLists; set => _hintLists = value; }
    public Dictionary<string, WordDictionary> WordDictionary { get => _wordDictionary; set => _wordDictionary = value; }
    public List<WordDictionary> Vocabulary { get => _vocabulary; set => _vocabulary = value; }

    private void Awake()
    {
        //instance = this;
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        for (int i = 0; i < _vocabulary.Count; i++)
        {
            _wordDictionary.Add(_vocabulary[i].Key, _vocabulary[i]);
        }
    }
}

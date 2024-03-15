using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;
using System.Linq;

public class DictionaryMode : MonoBehaviour
{
    public static DictionaryMode dictionaryMode;

    public enum DictionaryState
    {
        None,
        Dictionary,
        Translation,
        Hint
    }

    private DictionaryState _currentDictionaryState;
    private Menu _menu;

    //　辞書表示パネル
    [SerializeField]
    private GameObject _dictionaryPanel;
    //　翻訳選択肢パネル
    [SerializeField]
    private GameObject _dictionaryChoisePanel;
    //　翻訳記述パネル
    [SerializeField]
    private GameObject _dictionaryWritePanel;
    //　ヒントパネル
    [SerializeField]
    private GameObject _hintPanel;

    //　辞書のCanvasGroup
    private CanvasGroup _dictionaryCanvasGroup;
    //　翻訳のCanvasGroup
    private CanvasGroup _transCanvasGroup;
    //　翻訳のCanvasGroup
    private CanvasGroup _writeCanvasGroup;

    //　辞書に表示する言語
    [SerializeField]
    private GameObject _originReal;
    //　ヒントボタン
    [SerializeField]
    private GameObject _hintButton;
    //　辞書の表示する部分
    [SerializeField]
    private GameObject _dictionaryParent;
    //　翻訳選択肢テキスト１
    [SerializeField]
    private GameObject _transChoise1;
    [SerializeField]
    private TextMeshProUGUI _transChoiseText1;
    //　翻訳選択肢テキスト２
    [SerializeField]
    private GameObject _transChoise2;
    [SerializeField]
    private TextMeshProUGUI _transChoiseText2;
    //　翻訳選択肢テキスト３
    [SerializeField]
    private GameObject _transChoise3;
    [SerializeField]
    private TextMeshProUGUI _transChoiseText3;
    //　翻訳選択肢テキスト４
    [SerializeField]
    private GameObject _transChoise4;
    [SerializeField]
    private TextMeshProUGUI _transChoiseText4;
    //　翻訳選択肢テキスト５
    [SerializeField]
    private GameObject _transChoise5;
    [SerializeField]
    private TextMeshProUGUI _transChoiseText5;
    //　翻訳記入欄
    [SerializeField]
    private GameObject _transInput;
    //　ヒント
    [SerializeField]
    private Image _hintImage;
    //　言語インデックス
    private TalkManager _talkManager = null;
    //PlayerTalk
    [SerializeField]
    private PlayerTalk _playerTalk;

    //　生成したOriginRealを格納しておく(１０個単位で表示するため)
    private List<GameObject> _originReals = new List<GameObject>();
    //　ヒントも一緒に格納
    private List<GameObject> _hints = new List<GameObject>();

    //　辞書の最大表示数
    [SerializeField]
    private int _maxWordsNum = 8;
    //　単語の間隔
    [SerializeField]
    private int _intervalWords = 60;
    //　辞書の今見ているページ
    private int _currentPage = 1;
    //　辞書の最終ページ
    private int _maxPage = 0;

    //　CameraController
    [SerializeField]
    private CameraController _cameraController;
    [SerializeField]
    private PlayerController _playerController;

    //　最後に選択していたゲームオブジェクトをスタック
    private Stack<GameObject> _selectedGameObjectStack = new Stack<GameObject>();

    private SoundManager _soundManager;
    //　SE
    //　辞書を開く
    [SerializeField]
    private AudioClip _openDictionarySE;
    //　ページをめくる
    [SerializeField]
    private AudioClip _turnPageSE;
    //　辞書を閉じる
    [SerializeField]
    private AudioClip _closeDictionarySE;

    public DictionaryState CurrentDictionaryState { get => _currentDictionaryState; set => _currentDictionaryState = value; }

    private void Awake()
    {
        dictionaryMode = this;

        _menu = GameObject.FindWithTag("Player").GetComponent<Menu>();
        CurrentDictionaryState = DictionaryState.None;
        _dictionaryCanvasGroup = _dictionaryParent.GetComponent<CanvasGroup>();
        _transCanvasGroup = _dictionaryChoisePanel.GetComponent<CanvasGroup>();
        _writeCanvasGroup = _dictionaryWritePanel.GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        //　キャンセルボタンを押した時の処理
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
        {
            if (CurrentDictionaryState == DictionaryState.Dictionary)
            {
                _menu.ExitCommand();
                gameObject.SetActive(false);
                if (_playerController.PlayerState1 != PlayerController.PlayerState.Talk)
                {
                    _cameraController.IsActive = true;
                }

                _dictionaryPanel.SetActive(false);
                CurrentDictionaryState = DictionaryState.None;
            }
            else if (CurrentDictionaryState == DictionaryState.Translation)
            {
                if (_transInput.GetComponent<TMP_InputField>().text != null)
                {
                    return;
                }
                //_dictionaryChoisePanel.SetActive(false);
                //_dictionaryCanvasGroup.interactable = true;
                _dictionaryWritePanel.SetActive(false);
                _dictionaryCanvasGroup.interactable = true;
                CurrentDictionaryState = DictionaryState.Dictionary;
                //EventSystem.current.SetSelectedGameObject(_selectedGameObjectStack.Pop());
            }
            else if (CurrentDictionaryState == DictionaryState.Hint)
            {
                _hintPanel.SetActive(false);
                _dictionaryCanvasGroup.interactable = true;
                CurrentDictionaryState = DictionaryState.Dictionary;
                //EventSystem.current.SetSelectedGameObject(_selectedGameObjectStack.Pop());
            }
        }

        //　辞書を開いている時の操作
        if (CurrentDictionaryState == DictionaryState.Dictionary)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                NextPage();
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                PreviousPage();
            }
        }

        //　翻訳を開いている時の操作
        //if (CurrentDictionaryState == DictionaryState.Translation)
        //{
        //    if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
        //    {
        //        _transInput.GetComponent<LangTranslation>().InputTranslation();
        //    }
        //}

        //　選択解除された時（マウスでUI外をクリックした）は現在のモードによって無理やり選択させる
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            if (CurrentDictionaryState == DictionaryState.Dictionary)
            {
                EventSystem.current.SetSelectedGameObject(_dictionaryParent.transform.GetChild(0 + ((_currentPage - 1) * 8)).gameObject);
            }
        }
    }

    private void OnEnable()
    {
        //　現在のコマンドの初期化
        CurrentDictionaryState = DictionaryState.Dictionary;
        _cameraController.IsActive = false;
        _dictionaryChoisePanel.SetActive(false);
        _hintPanel.SetActive(false);
        _selectedGameObjectStack.Clear();
        _dictionaryCanvasGroup.interactable = true;
        _soundManager = SoundManager.soundManager;

        //　TalkManager取得
        if (_talkManager == null)
        {
            _talkManager = TalkManager.instance;
        }

        ShowDictionary(_talkManager);
    }

    //　選択したコマンドで処理分け
    public void SelectCommand(string command)
    {
        if (command == "Translation" /*&& _talkManager.HintLists[EventSystem.current.currentSelectedGameObject.GetComponent<CommandButton>().SelectLangNum].IsHint == true*/)
        {
            CurrentDictionaryState = DictionaryState.Translation;
            _cameraController.IsActive = false;
            _dictionaryCanvasGroup.interactable = false;
            _selectedGameObjectStack.Push(EventSystem.current.currentSelectedGameObject);

            ShowTranslation(EventSystem.current.currentSelectedGameObject.GetComponent<CommandButton>().SelectLangNum, EventSystem.current.currentSelectedGameObject.GetComponent<CommandButton>().SelectLang);
        }
        if (command == "Hint")
        {
            CurrentDictionaryState = DictionaryState.Hint;
            _cameraController.IsActive = false;
            _dictionaryCanvasGroup.interactable = false;

            ShowHint(EventSystem.current.currentSelectedGameObject.GetComponent<CommandButton>().SelectLangNum);
        }
    }

    //　辞書表示
    public void ShowDictionary(TalkManager talkManager)
    {
        CurrentDictionaryState = DictionaryState.Dictionary;
        _dictionaryPanel.SetActive(true);
        _soundManager.PlaySE(_openDictionarySE);

        int startY = 0;
        int i = 0;
        int j = 0;

        foreach (var word in talkManager.WordDictionary)
        {
            if (j == _maxWordsNum)
            {
                startY += _maxWordsNum * _intervalWords;
                j = 0;
            }
            j++;

            //言語を生成する位置
            Vector2 pos = new Vector2(150, startY - _intervalWords * ((i - 1) + 1));
            //言語を生成
            GameObject originReal = Instantiate(_originReal, new Vector3(0, 0, 0), Quaternion.identity, _dictionaryParent.transform);
            originReal.transform.localPosition = pos;
            _originReals.Add(originReal);
            //生成した言語をボタンとして取得、OnClickを設定
            Button originRealButton = originReal.GetComponent<Button>();
            originRealButton.onClick.AddListener(() => SelectCommand("Translation"));
            //選択した言語のインデックス番号を渡す
            CommandButton commandButton = originReal.GetComponent<CommandButton>();
            commandButton.SelectLangNum = i;
            commandButton.SelectLang = word.Key;
            GameObject parent = originReal.transform.GetChild(0).gameObject;
            TextMeshProUGUI origin = parent.transform.Find("Origin").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI real = parent.transform.Find("Real").GetComponent<TextMeshProUGUI>();
            //言語を表示
            if (_playerTalk == null)
            {
                _playerTalk = PlayerTalk.instance;
            }
            origin.text = _playerTalk.ConvFoolCoolFont(word.Value.OriginalWord);
            if (word.Value.SelectWord != null)
            {
                real.text = word.Value.SelectWord;
            }
            else
            {
                real.text = "";
            }
            //ヒントボタン生成
            if (talkManager.HintLists[i].IsHint == true)
            {
                GameObject hintButtonObject = Instantiate(_hintButton, new Vector3(0, 0, 0), Quaternion.identity, _dictionaryParent.transform);
                hintButtonObject.transform.localPosition = pos + new Vector2(320, 0);
                _hints.Add(hintButtonObject);
                Button hintButton = hintButtonObject.GetComponent<Button>();
                hintButton.onClick.AddListener(() => SelectCommand("Hint"));
                CommandButton commandButton1 = hintButtonObject.GetComponent<CommandButton>();
                commandButton1.SelectLangNum = i;
            }
            else
            {
                _hints.Add(null);
            }

            i++;
        }

        //一定数を表示（それ以外は非表示）
        for (int k = 0; k < _originReals.Count; k++)
        {
            if (k >= _currentPage * _maxWordsNum || (_currentPage - 1) * _maxWordsNum > k)
            {
                if (_originReals[k] != null)
                {
                    _originReals[k].SetActive(false);
                }
                if (_hints[k] != null)
                {
                    _hints[k].SetActive(false);
                }
            }
        }

        EventSystem.current.SetSelectedGameObject(_originReals[(_currentPage - 1) * _maxWordsNum]);
    }

    //　翻訳選択
    public void ShowTranslation(int num, string word)
    {
        CurrentDictionaryState = DictionaryState.Translation;
        //_dictionaryChoisePanel.SetActive(true);
        _dictionaryWritePanel.SetActive(true);
        //EventSystem.current.SetSelectedGameObject(_transChoise1);

        //受け取ったインデックス番号に応じたリアル言語を表示
        //string[] wordChoises = new string[5];
        //wordChoises[0] = _talkManager.WordChoises[num].Word1;
        //wordChoises[1] = _talkManager.WordChoises[num].Word2;
        //wordChoises[2] = _talkManager.WordChoises[num].Word3;
        //wordChoises[3] = _talkManager.WordChoises[num].Word4;
        //wordChoises[4] = _talkManager.WordChoises[num].Word5;
        //wordChoises = wordChoises.OrderBy(a => Guid.NewGuid()).ToArray();

        //_transChoiseText1.text = wordChoises[0];
        //_transChoiseText2.text = wordChoises[1];
        //_transChoiseText3.text = wordChoises[2];
        //_transChoiseText4.text = wordChoises[3];
        //_transChoiseText5.text = wordChoises[4];

        ////受け取ったインデックス番号を翻訳選択のCommandButtonに渡す
        //_transChoise1.GetComponent<CommandButton>().SelectLangNum = num;
        //_transChoise2.GetComponent<CommandButton>().SelectLangNum = num;
        //_transChoise3.GetComponent<CommandButton>().SelectLangNum = num;
        //_transChoise4.GetComponent<CommandButton>().SelectLangNum = num;
        //_transChoise5.GetComponent<CommandButton>().SelectLangNum = num;

        ////受け取った文字列を翻訳選択のCommandButtonに渡す
        //_transChoise1.GetComponent<CommandButton>().SelectLang = word;
        //_transChoise2.GetComponent<CommandButton>().SelectLang = word;
        //_transChoise3.GetComponent<CommandButton>().SelectLang = word;
        //_transChoise4.GetComponent<CommandButton>().SelectLang = word;
        //_transChoise5.GetComponent<CommandButton>().SelectLang = word;

        _transInput.GetComponent<TMP_InputField>().text = "";
        _transInput.GetComponent<LangTranslation>().SelectLang = word;
        EventSystem.current.SetSelectedGameObject(_transInput);
    }

    //　翻訳選択後、Canvasを更新
    public void UpdateDictionary()
    {
        _dictionaryCanvasGroup.interactable = true;
        CurrentDictionaryState = DictionaryState.Dictionary;
        //_dictionaryChoisePanel.SetActive(false);
        _dictionaryWritePanel.SetActive(false);
        _dictionaryPanel.SetActive(false);
        DestroyOriginReal();
        ShowDictionary(_talkManager);
    }

    //　OriginRealを破棄
    public void DestroyOriginReal()
    {
        int childCount = _dictionaryParent.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Destroy(_dictionaryParent.transform.GetChild(i).gameObject);
        }
        _originReals.Clear();
        _hints.Clear();
    }

    //　ヒント閲覧
    public void ShowHint(int num)
    {
        CurrentDictionaryState = DictionaryState.Hint;
        _hintPanel.SetActive(true);

        //　ヒントを表示
        _hintImage.sprite = _talkManager.HintLists[num].Hint1;
    }

    //　辞書のページ送り
    public void NextPage()
    {
        if (CurrentDictionaryState != DictionaryState.Dictionary)
        {
            return;
        }

        _currentPage++;
        _maxPage = _originReals.Count / _maxWordsNum + 1;
        bool isSelect = true;
        _soundManager.PlaySE(_turnPageSE);

        if (_currentPage < _maxPage)
        {
            for (int i = (_currentPage - 1) * _maxWordsNum; i < (_currentPage - 1) * _maxWordsNum + _maxWordsNum; i++)
            {
                if (_originReals[i - _maxWordsNum] != null)
                {
                    _originReals[i - _maxWordsNum].SetActive(false);
                }
                if (_originReals[i] != null)
                {
                    _originReals[i].SetActive(true);
                    if (isSelect == true)
                    {
                        EventSystem.current.SetSelectedGameObject(_originReals[i]);
                        isSelect = false;
                    }
                }

                if (_hints[i - _maxWordsNum] != null)
                {
                    _hints[i - _maxWordsNum].SetActive(false);
                }
                if (_hints[i] != null)
                {
                    _hints[i].SetActive(true);
                }
            }
        }
        else if (_currentPage == _maxPage)
        {
            for (int i = 0; i < _originReals.Count; i++)
            {
                if (i < (_currentPage - 1) * _maxWordsNum)
                {
                    if (_originReals[i] != null)
                    {
                        _originReals[i].SetActive(false);
                    }
                    if (_hints[i] != null)
                    {
                        _hints[i].SetActive(false);
                    }
                }
                else
                {
                    if (_originReals[i] != null)
                    {
                        _originReals[i].SetActive(true);
                        if (isSelect == true)
                        {
                            EventSystem.current.SetSelectedGameObject(_originReals[i]);
                            isSelect = false;
                        }
                    }
                    if (_hints[i] != null)
                    {
                        _hints[i].SetActive(true);
                    }
                }
            }
        }
        else
        {
            _currentPage = 1;
            for (int i = 0; i < _originReals.Count; i++)
            {
                if (i >= _maxWordsNum)
                {
                    if (_originReals[i] != null)
                    {
                        _originReals[i].SetActive(false);
                    }
                    if (_hints[i] != null)
                    {
                        _hints[i].SetActive(false);
                    }
                }
                else
                {
                    if (_originReals[i] != null)
                    {
                        _originReals[i].SetActive(true);
                        if (isSelect == true)
                        {
                            EventSystem.current.SetSelectedGameObject(_originReals[i]);
                            isSelect = false;
                        }
                    }
                    if (_hints[i] != null)
                    {
                        _hints[i].SetActive(true);
                    }
                }
            }
        }
    }

    //　辞書のページ戻し
    public void PreviousPage()
    {
        if (CurrentDictionaryState != DictionaryState.Dictionary)
        {
            return;
        }

        _currentPage--;
        _maxPage = _originReals.Count / _maxWordsNum + 1;
        bool isSelect = true;
        _soundManager.PlaySE(_turnPageSE);

        if (_currentPage == _maxPage - 1)
        {
            for (int i = (_maxPage - 1) * _maxWordsNum; i < _originReals.Count; i++)
            {
                if (_originReals[i] != null)
                {
                    _originReals[i].SetActive(false);
                }
                if (_hints[i] != null)
                {
                    _hints[i].SetActive(false);
                }
            }
            for (int i = _currentPage * _maxWordsNum; i < _currentPage * _maxWordsNum + _maxWordsNum; i++)
            {
                if (_originReals[i - _maxWordsNum] != null)
                {
                    _originReals[i - _maxWordsNum].SetActive(true);
                    if (isSelect == true)
                    {
                        EventSystem.current.SetSelectedGameObject(_originReals[i - _maxWordsNum]);
                        isSelect = false;
                    }
                }
                if (_hints[i - _maxWordsNum] != null)
                {
                    _hints[i - _maxWordsNum].SetActive(true);
                }
            }
        }
        else if (_currentPage == 0)
        {
            _currentPage = _maxPage;
            for (int i = 0; i < _originReals.Count; i++)
            {
                if (i < (_currentPage - 1) * _maxWordsNum)
                {
                    if (_originReals[i] != null)
                    {
                        _originReals[i].SetActive(false);
                    }
                    if (_hints[i] != null)
                    {
                        _hints[i].SetActive(false);
                    }
                }
                else
                {
                    if (_originReals[i] != null)
                    {
                        _originReals[i].SetActive(true);
                        if (isSelect == true)
                        {
                            EventSystem.current.SetSelectedGameObject(_originReals[i]);
                            isSelect = false;
                        }
                    }
                    if (_hints[i] != null)
                    {
                        _hints[i].SetActive(true);
                    }
                }
            }
        }
        else
        {
            for (int i = _currentPage * _maxWordsNum; i < _currentPage * _maxWordsNum + _maxWordsNum; i++)
            {
                if (_originReals[i] != null)
                {
                    _originReals[i].SetActive(false);
                }
                if (_hints[i] != null)
                {
                    _hints[i].SetActive(false);
                }

                if (_originReals[i - _maxWordsNum] != null)
                {
                    _originReals[i - _maxWordsNum].SetActive(true);
                    if (isSelect == true)
                    {
                        EventSystem.current.SetSelectedGameObject(_originReals[i - _maxWordsNum]);
                        isSelect = false;
                    }
                }
                if (_hints[i - _maxWordsNum] != null)
                {
                    _hints[i - _maxWordsNum].SetActive(true);
                }
            }
        }
    }

    //　ExitCommandの時にパネルも無効化しておく
    public void ExitCommandPanel()
    {
        _dictionaryPanel.SetActive(false);
        _dictionaryChoisePanel.SetActive(false);
        _hintPanel.SetActive(false);
        DestroyOriginReal();
        _soundManager.PlaySE(_closeDictionarySE);
    }
}

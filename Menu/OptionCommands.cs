using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OptionCommands : MonoBehaviour
{
    public static OptionCommands optionCommands;

    public enum OptionMode
    {
        None,
        Command,
        OpeInst,
        TalkLog,
        Settings,
        Save
    }
    [SerializeField]
    private OptionMode _currentOption;

    private Menu _menu;

    //　最初に選択するButtonのTransform
    [SerializeField]
    private GameObject _firstSelectButton;

    //　コマンドパネル
    [SerializeField]
    private GameObject _commandPanel;
    //　操作説明ボタン
    [SerializeField]
    private GameObject _opeInstButton;
    //　会話履歴ボタン
    [SerializeField]
    private GameObject _talkLogButton;
    //　設定ボタン
    [SerializeField]
    private GameObject _settingsButton;
    //　セーブボタン
    [SerializeField]
    private GameObject _saveButton;
    //　操作説明表示パネル
    [SerializeField]
    private GameObject _opeInstPanel;
    //　会話履歴表示パネル
    [SerializeField]
    private GameObject _talkLogPanel;
    //　会話履歴表示Content
    [SerializeField]
    private RectTransform _content;
    //　設定表示パネル
    [SerializeField]
    private GameObject _settingsPanel;
    //　セーブ表示パネル
    [SerializeField]
    private GameObject _savePanel;

    //　コマンドパネルのCanvasGroup
    private CanvasGroup _optionCanvasGroup;
    private CanvasGroup _commandPanelCanvasGroup;
    private CanvasGroup _talkLogPanelCanvasGroup;

    //　CameraController
    [SerializeField]
    private CameraController _cameraController;
    [SerializeField]
    private PlayerController _playerController;

    //　最後に選択していたゲームオブジェクトをスタック
    private Stack<GameObject> _selectedGameObjectStack = new Stack<GameObject>();

    public OptionMode CurrentOption { get => _currentOption; set => _currentOption = value; }

    private void Awake()
    {
        // 常に一つだけにする
        if (optionCommands == null)
        {
            optionCommands = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        CurrentOption = OptionMode.None;
        _optionCanvasGroup = GetComponent<CanvasGroup>();
        _commandPanelCanvasGroup = _commandPanel.GetComponent<CanvasGroup>();
        _talkLogPanelCanvasGroup = _talkLogPanel.GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        //　キャンセルボタンを押した時の処理
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
        {
            if (CurrentOption == OptionMode.Command)
            {
                _menu = GameObject.FindWithTag("Player").GetComponent<Menu>();
                _menu.ExitOptionCommand();
                _optionCanvasGroup.alpha = 0;
                if (_playerController.PlayerState1 != PlayerController.PlayerState.Talk)
                {
                    if (_cameraController != null)
                    {
                        _cameraController.IsActive = true;
                    }
                }

                CurrentOption = OptionMode.None;
            }
            else if (CurrentOption == OptionMode.OpeInst)
            {
                _opeInstPanel.SetActive(false);
                _commandPanelCanvasGroup.interactable = true;
                CurrentOption = OptionMode.Command;
                EventSystem.current.SetSelectedGameObject(_selectedGameObjectStack.Pop());
            }
            else if (CurrentOption == OptionMode.TalkLog)
            {
                _talkLogPanelCanvasGroup.alpha = 0;
                _commandPanelCanvasGroup.interactable = true;
                CurrentOption = OptionMode.Command;
                EventSystem.current.SetSelectedGameObject(_selectedGameObjectStack.Pop());
            }
            else if (CurrentOption == OptionMode.Settings)
            {
                _settingsPanel.SetActive(false);
                _commandPanelCanvasGroup.interactable = true;
                CurrentOption = OptionMode.Command;
                EventSystem.current.SetSelectedGameObject(_selectedGameObjectStack.Pop());
            }
            else if (CurrentOption == OptionMode.Save)
            {
                _savePanel.SetActive(false);
                _commandPanelCanvasGroup.interactable = true;
                CurrentOption = OptionMode.Command;
                EventSystem.current.SetSelectedGameObject(_selectedGameObjectStack.Pop());
            }
        }

        //　選択解除された時（マウスでUI外をクリックした）は現在のモードによって無理やり選択させる
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            if (CurrentOption == OptionMode.Command)
            {
                EventSystem.current.SetSelectedGameObject(_firstSelectButton);
            }
        }
    }

    public void Initialize()
    {
        //　現在のコマンドの初期化
        CurrentOption = OptionMode.Command;
        if (_cameraController != null)
        {
            _cameraController.IsActive = false;
        }
        _opeInstPanel.SetActive(false);
        _talkLogPanelCanvasGroup.alpha = 0;
        //_settingsPanel.SetActive(false);
        //_savePanel.SetActive(false);
        _selectedGameObjectStack.Clear();
        _optionCanvasGroup.interactable = true;
        _commandPanelCanvasGroup.interactable = true;
    }

    //　選択したコマンドで処理分け
    public void SelectOptionCommand(string command)
    {
        if (command == "OpeInst")
        {
            _currentOption = OptionMode.OpeInst;
            _commandPanelCanvasGroup.interactable = false;
            _selectedGameObjectStack.Push(EventSystem.current.currentSelectedGameObject);

            _opeInstPanel.SetActive(true);
        }
        else if (command == "TalkLog")
        {
            _currentOption = OptionMode.TalkLog;
            _commandPanelCanvasGroup.interactable = false;
            _selectedGameObjectStack.Push(EventSystem.current.currentSelectedGameObject);

            _talkLogPanelCanvasGroup.alpha = 1;
            LayoutRebuilder.ForceRebuildLayoutImmediate(_content);
        }
        else if (command == "Settings")
        {
            _currentOption = OptionMode.Settings;
            _commandPanelCanvasGroup.interactable = false;
            _selectedGameObjectStack.Push(EventSystem.current.currentSelectedGameObject);

            _settingsPanel.SetActive(true);
        }
        else if (command == "Save")
        {
            _currentOption = OptionMode.Save;
            _commandPanelCanvasGroup.interactable = false;
            _selectedGameObjectStack.Push(EventSystem.current.currentSelectedGameObject);

            _savePanel.SetActive(true);
        }
    }

    //　ExitCommandの時にパネルも無効化しておく
    public void ExitOptionCommandPanel()
    {
        _optionCanvasGroup.interactable = false;
        _opeInstPanel.SetActive(false);
        _talkLogPanelCanvasGroup.alpha = 0;
        //_settingsPanel.SetActive(false);
        //_savePanel.SetActive(false);
    }
}

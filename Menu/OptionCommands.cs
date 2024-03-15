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

    //�@�ŏ��ɑI������Button��Transform
    [SerializeField]
    private GameObject _firstSelectButton;

    //�@�R�}���h�p�l��
    [SerializeField]
    private GameObject _commandPanel;
    //�@��������{�^��
    [SerializeField]
    private GameObject _opeInstButton;
    //�@��b�����{�^��
    [SerializeField]
    private GameObject _talkLogButton;
    //�@�ݒ�{�^��
    [SerializeField]
    private GameObject _settingsButton;
    //�@�Z�[�u�{�^��
    [SerializeField]
    private GameObject _saveButton;
    //�@��������\���p�l��
    [SerializeField]
    private GameObject _opeInstPanel;
    //�@��b����\���p�l��
    [SerializeField]
    private GameObject _talkLogPanel;
    //�@��b����\��Content
    [SerializeField]
    private RectTransform _content;
    //�@�ݒ�\���p�l��
    [SerializeField]
    private GameObject _settingsPanel;
    //�@�Z�[�u�\���p�l��
    [SerializeField]
    private GameObject _savePanel;

    //�@�R�}���h�p�l����CanvasGroup
    private CanvasGroup _optionCanvasGroup;
    private CanvasGroup _commandPanelCanvasGroup;
    private CanvasGroup _talkLogPanelCanvasGroup;

    //�@CameraController
    [SerializeField]
    private CameraController _cameraController;
    [SerializeField]
    private PlayerController _playerController;

    //�@�Ō�ɑI�����Ă����Q�[���I�u�W�F�N�g���X�^�b�N
    private Stack<GameObject> _selectedGameObjectStack = new Stack<GameObject>();

    public OptionMode CurrentOption { get => _currentOption; set => _currentOption = value; }

    private void Awake()
    {
        // ��Ɉ�����ɂ���
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
        //�@�L�����Z���{�^�������������̏���
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

        //�@�I���������ꂽ���i�}�E�X��UI�O���N���b�N�����j�͌��݂̃��[�h�ɂ���Ė������I��������
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
        //�@���݂̃R�}���h�̏�����
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

    //�@�I�������R�}���h�ŏ�������
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

    //�@ExitCommand�̎��Ƀp�l�������������Ă���
    public void ExitOptionCommandPanel()
    {
        _optionCanvasGroup.interactable = false;
        _opeInstPanel.SetActive(false);
        _talkLogPanelCanvasGroup.alpha = 0;
        //_settingsPanel.SetActive(false);
        //_savePanel.SetActive(false);
    }
}

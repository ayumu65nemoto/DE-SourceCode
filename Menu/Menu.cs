using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    private LoadSceneManager _loadSceneManager;
    //�@����UI
    [SerializeField]
    private GameObject _dictionaryCanvas;
    //�@�I�v�V����UI
    [SerializeField]
    private GameObject _optionCanvas;
    [SerializeField]
    private PlayerController _playerController;
    //�@Command
    private DictionaryMode _dictionaryMode;
    private OptionCommands _optionCommands;
    //  PlayerTalk
    private PlayerTalk _playerTalk;

    // Start is called before the first frame update
    void Start()
    {
        _loadSceneManager = LoadSceneManager.loadSceneManager;
        _playerController = GetComponent<PlayerController>();
        _playerTalk = GetComponent<PlayerTalk>();
        _dictionaryMode = _dictionaryCanvas.GetComponent<DictionaryMode>();
        _optionCommands = OptionCommands.optionCommands;
        _optionCanvas = FindAnyObjectByType<OptionCommands>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (_loadSceneManager.IsTransition == true)
        {
            return;
        }

        if (_optionCommands == null)
        {
            _optionCommands = OptionCommands.optionCommands;
        }

        //�@����UI�̕\���E��\���̐؂�ւ�
        if (Input.GetKeyDown(KeyCode.R) 
            && _dictionaryMode.CurrentDictionaryState != DictionaryMode.DictionaryState.Translation 
            && _optionCommands.CurrentOption == OptionCommands.OptionMode.None
            )
        {
            //�@�R�}���h
            if (!_dictionaryCanvas.activeSelf)
            {
                //�@���j�e�B�������R�}���h��Ԃɂ���
                if (_playerController.PlayerState1 != PlayerController.PlayerState.Talk)
                {
                    _playerController.SetPlayerState(PlayerController.PlayerState.Command);
                }
            }
            else
            {
                ExitCommand();
            }
            //�@�R�}���hUI�̃I���E�I�t
            _dictionaryCanvas.SetActive(!_dictionaryCanvas.activeSelf);
        }

        //�@�I�v�V����UI�̕\���E��\���̐؂�ւ�
        if (Input.GetKeyDown(KeyCode.Tab)
            && _dictionaryMode.CurrentDictionaryState == DictionaryMode.DictionaryState.None)
        {
            //�@�R�}���h
            if (_optionCanvas.GetComponent<CanvasGroup>().alpha == 0)
            {
                //�@���j�e�B�������R�}���h��Ԃɂ���
                if (_playerController.PlayerState1 != PlayerController.PlayerState.Talk)
                {
                    _playerController.SetPlayerState(PlayerController.PlayerState.Command);
                }
                _optionCommands.Initialize();
                _optionCanvas.GetComponent<CanvasGroup>().alpha = 1;
            }
            else
            {
                ExitOptionCommand();
                _optionCanvas.GetComponent<CanvasGroup>().alpha = 0;
            }
        }
    }

    public void ExitCommand()
    {
        EventSystem.current.SetSelectedGameObject(null);
        if (_playerController.PlayerState1 != PlayerController.PlayerState.Talk)
        {
            _playerController.SetPlayerState(PlayerController.PlayerState.Normal);
        }
        else
        {
            _playerTalk.ReStartTalking();
        }
        _dictionaryMode = DictionaryMode.dictionaryMode;
        _dictionaryMode.ExitCommandPanel();
        _dictionaryMode.CurrentDictionaryState = DictionaryMode.DictionaryState.None;
    }

    public void ExitOptionCommand()
    {
        EventSystem.current.SetSelectedGameObject(null);
        if (_playerController.PlayerState1 != PlayerController.PlayerState.Talk)
        {
            _playerController.SetPlayerState(PlayerController.PlayerState.Normal);
        }
        else
        {
            _playerTalk.ReStartTalking();
        }
        _optionCommands = OptionCommands.optionCommands;
        _optionCommands.ExitOptionCommandPanel();
        _optionCommands.CurrentOption = OptionCommands.OptionMode.None;
    }
}

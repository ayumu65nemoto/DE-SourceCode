using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuCommand : MonoBehaviour
{
    private LoadSceneManager _loadSceneManager;
    //　メニューUI
    [SerializeField]
    private GameObject _menuUI;
    private PlayerController _playerController;
    //　Command
    private Command _command;
    //  PlayerTalk
    private PlayerTalk _playerTalk;

    // Start is called before the first frame update
    void Start()
    {
        _loadSceneManager = LoadSceneManager.loadSceneManager;
        _playerController = GetComponent<PlayerController>();
        _playerTalk = GetComponent<PlayerTalk>();
        _command = _menuUI.GetComponent<Command>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_loadSceneManager.IsTransition == true)
        {
            return;
        }
        //　コマンドUIの表示・非表示の切り替え
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //　コマンド
            if (!_menuUI.activeSelf)
            {
                //　ユニティちゃんをコマンド状態にする
                if (_playerController.PlayerState1 != PlayerController.PlayerState.Talk)
                {
                    _playerController.SetPlayerState(PlayerController.PlayerState.Command);
                }
            }
            else
            {
                ExitCommand();
            }
            //　コマンドUIのオン・オフ
            _menuUI.SetActive(!_menuUI.activeSelf);
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
        _command = Command.instance;
        _command.ExitCommandPanel();
        _command.CurrentCommand = Command.CommandMode.None;
    }
}

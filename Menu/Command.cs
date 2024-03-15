using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;
using System.Linq;

public class Command : MonoBehaviour
{
    public static Command instance;

    public enum CommandMode
    {
        None,
        Command,
        Status,
        Item,
        UseItem,
        UseItemToItem,
        UseItemToUseItem,
        NoItemPassed,
        Dictionary,
        Translation,
        Hint
    }

    [SerializeField]
    private CommandMode _currentCommand;
    //�@���j�e�B�����R�}���h�X�N���v�g
    private MenuCommand _menuCommand;

    //�@�ŏ��ɑI������Button��Transform
    [SerializeField]
    private GameObject _firstSelectButton;

    //�@�R�}���h�p�l��
    [SerializeField]
    private GameObject _commandPanel;
    //�@�X�e�[�^�X�{�^��
    [SerializeField]
    private GameObject _statusButton;
    //�@�����{�^��
    [SerializeField]
    private GameObject _dictionaryButton;
    //�@�X�e�[�^�X�\���p�l��
    [SerializeField]
    private GameObject _statusPanel;
    //�@�����\���p�l��
    [SerializeField]
    private GameObject _dictionaryPanel;
    //�@�|��I�����p�l��
    [SerializeField]
    private GameObject _dictionaryChoisePanel;
    //�@�q���g�p�l��
    [SerializeField]
    private GameObject _hintPanel;
    //�@�y�[�W����{�^��
    //[SerializeField]
    //private GameObject _nextButton;
    //[SerializeField]
    //private GameObject _previousButton;

    //�@�R�}���h�p�l����CanvasGroup
    private CanvasGroup _commandPanelCanvasGroup;
    //�@�����R�}���h��CanvasGroup
    private CanvasGroup _dictionaryCanvasGroup;
    //�@�����R�}���h��CanvasGroup
    private CanvasGroup _transCanvasGroup;

    //�@�L�����N�^�[��
    [SerializeField]
    private TextMeshProUGUI _characterNameText;
    //�@�X�e�[�^�X�^�C�g���e�L�X�g
    [SerializeField]
    private TextMeshProUGUI _statusTitleText;
    //�@�X�e�[�^�X�p�����[�^�e�L�X�g1
    [SerializeField]
    private TextMeshProUGUI _statusParamText;
    //�@�v���C���[�X�e�[�^�X
    [SerializeField]
    private PlayerStatus _playerStatus = null;
    //�@�����ɕ\�����錾��
    [SerializeField]
    private GameObject _originReal;
    //�@�q���g�{�^��
    [SerializeField]
    private GameObject _hintButton;
    //�@�����̕\�����镔��
    [SerializeField]
    private GameObject _dictionaryParent;
    //�@�I���W�i������
    private TextMeshProUGUI _originLangText;
    //�@�����̌���
    private TextMeshProUGUI _realLangText;
    //�@�|��I�����e�L�X�g�P
    [SerializeField]
    private GameObject _transChoise1;
    [SerializeField]
    private TextMeshProUGUI _transChoiseText1;
    //�@�|��I�����e�L�X�g�Q
    [SerializeField]
    private GameObject _transChoise2;
    [SerializeField]
    private TextMeshProUGUI _transChoiseText2;
    //�@�|��I�����e�L�X�g�R
    [SerializeField]
    private GameObject _transChoise3;
    [SerializeField]
    private TextMeshProUGUI _transChoiseText3;
    //�@�q���g�P
    [SerializeField]
    private Image _hint1;
    //�@����C���f�b�N�X
    [SerializeField]
    private TalkManager _talkManager = null;
    //PlayerTalk
    private PlayerTalk _playerTalk;

    //�@��������OriginReal���i�[���Ă���(�P�O�P�ʂŕ\�����邽��)
    private List<GameObject> _originReals = new List<GameObject>();
    //�@�q���g���ꏏ�Ɋi�[
    private List<GameObject> _hints = new List<GameObject>();

    //�@�����̍ő�\����
    [SerializeField]
    private int _maxWordsNum = 9;
    //�@�����̍����Ă���y�[�W
    private int _currentPage = 1;
    //�@�����̍ŏI�y�[�W
    private int _maxPage = 0;

    //�@CameraController
    [SerializeField]
    private CameraController _cameraController;

    //�@�Ō�ɑI�����Ă����Q�[���I�u�W�F�N�g���X�^�b�N
    private Stack<GameObject> _selectedGameObjectStack = new Stack<GameObject>();

    //�@�A�C�e���\���p�l��
    [SerializeField]
    private GameObject _itemPanel;
    //�@�A�C�e���p�l���{�^����\������ꏊ
    [SerializeField]
    private GameObject _content;
    //�@�A�C�e�����g���I���p�l��
    [SerializeField]
    private GameObject _useItemPanel;
    //�@���\���p�l��
    [SerializeField]
    private GameObject _itemInfoPanel;
    //�@�A�C�e���g�p��̏��\���p�l��
    [SerializeField]
    private GameObject _useItemInfoPanel;

    //�@�A�C�e���p�l����Canvas Group
    private CanvasGroup _itemPanelCanvasGroup;
    //�@�A�C�e�����g���I���p�l����CanvasGroup
    private CanvasGroup _useItemPanelCanvasGroup;
   
    //�@���\���^�C�g���e�L�X�g
    [SerializeField]
    private TextMeshProUGUI _infoTitleText;
    //�@���\���e�L�X�g
    [SerializeField]
    private TextMeshProUGUI _infoText;

    //�@�L�����N�^�[�A�C�e���̃{�^���̃v���n�u
    [SerializeField]
    private GameObject _itemPanelButtonPrefab = null;
    //�@�A�C�e���g�p���̃{�^���̃v���n�u
    [SerializeField]
    private GameObject _useItemPanelButtonPrefab = null;

    //�@�A�C�e���{�^���ꗗ
    private List<GameObject> _itemPanelButtonList = new List<GameObject>();

    public CommandMode CurrentCommand { get => _currentCommand; set => _currentCommand = value; }

    private void Awake()
    {
        instance = this;

        //�@�R�}���h��ʂ��J�����������Ă���UnityChanCommandScript���擾
        _menuCommand = GameObject.FindWithTag("Player").GetComponent<MenuCommand>();
        //�@���݂̃R�}���h��������
        CurrentCommand = CommandMode.None;
        //�@CanvasGroup
        _commandPanelCanvasGroup = _commandPanel.GetComponent<CanvasGroup>();
        _dictionaryCanvasGroup = _dictionaryParent.GetComponent<CanvasGroup>();
        _transCanvasGroup = _dictionaryChoisePanel.GetComponent<CanvasGroup>();
        
        //�@�A�C�e���Ɏg��CanvasGroup
        _itemPanelCanvasGroup = _itemPanel.GetComponent<CanvasGroup>();
        _useItemPanelCanvasGroup = _useItemPanel.GetComponent<CanvasGroup>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _playerTalk = PlayerTalk.instance;
    }

    // Update is called once per frame
    void Update()
    {
        //�@�L�����Z���{�^�������������̏���
        if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
        {
            //�@�R�}���h�I����ʎ�
            if (CurrentCommand == CommandMode.Command)
            {
                _menuCommand.ExitCommand();
                gameObject.SetActive(false);
                _cameraController.IsActive = true;
            }
            else if (CurrentCommand == CommandMode.Status)
            {
                _statusPanel.SetActive(false);
                //�@�O�̃p�l���őI�����Ă����Q�[���I�u�W�F�N�g��I��
                EventSystem.current.SetSelectedGameObject(_selectedGameObjectStack.Pop());
                _commandPanelCanvasGroup.interactable = true;
                CurrentCommand = CommandMode.Command;
            }
            else if (CurrentCommand == CommandMode.Dictionary)
            {
                _dictionaryPanel.SetActive(false);
                //_nextButton.SetActive(false);
                //_previousButton.SetActive(false);
                EventSystem.current.SetSelectedGameObject(_dictionaryButton);
                _commandPanelCanvasGroup.interactable = true;
                CurrentCommand = CommandMode.Command;
            }
            else if (CurrentCommand == CommandMode.Translation)
            {
                _dictionaryChoisePanel.SetActive(false);
                _dictionaryCanvasGroup.interactable = true;
                CurrentCommand = CommandMode.Dictionary;
                EventSystem.current.SetSelectedGameObject(_selectedGameObjectStack.Pop());
            }
            else if (CurrentCommand == CommandMode.Hint)
            {
                _hintPanel.SetActive(false);
                _dictionaryCanvasGroup.interactable = true;
                CurrentCommand = CommandMode.Dictionary;
            }
            else if (CurrentCommand == CommandMode.Item)
            {
                _itemPanelCanvasGroup.interactable = false;
                _itemPanel.SetActive(false);
                _itemInfoPanel.SetActive(false);
                //�@���X�g���N���A
                _itemPanelButtonList.Clear();
                //�@ItemPanel��Cancel����������content�ȉ��̃A�C�e���p�l���{�^����S�폜
                for (int i = _content.transform.childCount - 1; i >= 0; i--)
                {
                    Destroy(_content.transform.GetChild(i).gameObject);
                }

                EventSystem.current.SetSelectedGameObject(_selectedGameObjectStack.Pop());
                _commandPanelCanvasGroup.interactable = true;
                CurrentCommand = CommandMode.Command;
            }
            else if (CurrentCommand == CommandMode.UseItem)
            {
                _useItemPanelCanvasGroup.interactable = false;
                _useItemPanel.SetActive(false);
                //�@UseItemPanel��Cancel�{�^������������UseItemPanel�̎q�v�f�̃{�^���̑S�폜
                for (int i = _useItemPanel.transform.childCount - 1; i >= 0; i--)
                {
                    Destroy(_useItemPanel.transform.GetChild(i).gameObject);
                }

                EventSystem.current.SetSelectedGameObject(_selectedGameObjectStack.Pop());
                _itemPanelCanvasGroup.interactable = true;
                CurrentCommand = CommandMode.Item;
            }
        }

        //�@�A�C�e���𑕔��A�������O�����\����̏���
        if (CurrentCommand == CommandMode.UseItemToItem)
        {
            if (Input.anyKeyDown
                || !Mathf.Approximately(Input.GetAxis("Horizontal"), 0f)
                || !Mathf.Approximately(Input.GetAxis("Vertical"), 0f)
                )
            {
                CurrentCommand = CommandMode.Item;
                _useItemInfoPanel.SetActive(false);
                _itemPanel.transform.SetAsLastSibling();
                _itemPanelCanvasGroup.interactable = true;

                EventSystem.current.SetSelectedGameObject(_selectedGameObjectStack.Pop());

            }
        }
        //�@�A�C�e�����̂Ă��I��������̏��
        else if (CurrentCommand == CommandMode.UseItemToUseItem)
        {
            if (Input.anyKeyDown
                || !Mathf.Approximately(Input.GetAxis("Horizontal"), 0f)
                || !Mathf.Approximately(Input.GetAxis("Vertical"), 0f)
                )
            {
                CurrentCommand = CommandMode.UseItem;
                _useItemInfoPanel.SetActive(false);
                _useItemPanel.transform.SetAsLastSibling();
                _useItemPanelCanvasGroup.interactable = true;
            }
        }
        //�@�A�C�e�����g�p�A�n���A�̂Ă��I��������ɂ��̃A�C�e���̐���0�ɂȂ�����
        else if (CurrentCommand == CommandMode.NoItemPassed)
        {
            if (Input.anyKeyDown
                || !Mathf.Approximately(Input.GetAxis("Horizontal"), 0f)
                || !Mathf.Approximately(Input.GetAxis("Vertical"), 0f)
                )
            {
                CurrentCommand = CommandMode.Item;
                _useItemInfoPanel.SetActive(false);
                _useItemPanel.SetActive(false);
                _itemPanel.transform.SetAsLastSibling();
                _itemPanelCanvasGroup.interactable = true;

                //�@�A�C�e���p�l���{�^��������΍ŏ��̃A�C�e���p�l���{�^����I��
                if (_content.transform.childCount != 0)
                {
                    EventSystem.current.SetSelectedGameObject(_content.transform.GetChild(0).gameObject);
                }
                else
                {
                    //�@�A�C�e���p�l���{�^�����Ȃ���΁i�A�C�e���������Ă��Ȃ��jItemSelectPanel�ɖ߂�
                    CurrentCommand = CommandMode.Item;
                    _itemPanelCanvasGroup.interactable = false;
                    _itemPanel.SetActive(false);
                    EventSystem.current.SetSelectedGameObject(_selectedGameObjectStack.Pop());
                }
            }
        }

        //�@�������J���Ă��鎞�̑���
        if (CurrentCommand == CommandMode.Dictionary)
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

        //�@�I���������ꂽ���i�}�E�X��UI�O���N���b�N�����j�͌��݂̃��[�h�ɂ���Ė������I��������
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            if (CurrentCommand == CommandMode.Command)
            {
                EventSystem.current.SetSelectedGameObject(_commandPanel.transform.GetChild(0).gameObject);
            }
            else if (CurrentCommand == CommandMode.Item)
            {
                EventSystem.current.SetSelectedGameObject(_content.transform.GetChild(0).gameObject);
            }
            else if (CurrentCommand == CommandMode.UseItem)
            {
                EventSystem.current.SetSelectedGameObject(_useItemPanel.transform.GetChild(0).gameObject);
            }
        }
    }

    private void OnEnable()
    {
        //�@���݂̃R�}���h�̏�����
        CurrentCommand = CommandMode.Command;
        //�@�R�}���h���j���[�\�����ɑ��̃p�l���͔�\���ɂ���
        _statusPanel.SetActive(false);

        _selectedGameObjectStack.Clear();

        _commandPanelCanvasGroup.interactable = true;
        EventSystem.current.SetSelectedGameObject(_firstSelectButton);

        //�@TalkManager�擾
        if (_talkManager == null)
        {
            _talkManager = TalkManager.instance;
        }

        _itemPanel.SetActive(false);
        _useItemPanel.SetActive(false);
        _itemInfoPanel.SetActive(false);
        _useItemInfoPanel.SetActive(false);

        //�@�A�C�e���p�l���{�^��������ΑS�č폜
        for (int i = _content.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(_content.transform.GetChild(i).gameObject);
        }
        //�@�A�C�e�����g���L�����N�^�[�I���{�^��������ΑS�č폜
        for (int i = _useItemPanel.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(_useItemPanel.transform.GetChild(i).gameObject);
        }

        _itemPanelButtonList.Clear();

        _itemPanelCanvasGroup.interactable = false;
        _useItemPanelCanvasGroup.interactable = false;
    }

    //�@�I�������R�}���h�ŏ�������
    public void SelectCommand(string command)
    {
        if (command == "Status")
        {
            CurrentCommand = CommandMode.Status;
            _cameraController.IsActive = false;
            //�@UI�̃I���E�I�t��I���A�C�R���̐ݒ�
            _commandPanelCanvasGroup.interactable = false;
            _selectedGameObjectStack.Push(EventSystem.current.currentSelectedGameObject);

            ShowStatus(_playerStatus);
        }
        if (command == "Item")
        {
            CurrentCommand = CommandMode.Item;
            _commandPanelCanvasGroup.interactable = false;
            _selectedGameObjectStack.Push(EventSystem.current.currentSelectedGameObject);

            ShowItem(_playerStatus);
        }
        if (command == "Dictionary")
        {
            CurrentCommand = CommandMode.Dictionary;
            _cameraController.IsActive = false;
            _commandPanelCanvasGroup.interactable = false;
            _dictionaryCanvasGroup.interactable = true;
            _selectedGameObjectStack.Push(EventSystem.current.currentSelectedGameObject);

            ShowDictionary(_talkManager);
        }
        if (command == "Translation" && _talkManager.HintLists[EventSystem.current.currentSelectedGameObject.GetComponent<CommandButton>().SelectLangNum].IsHint == true)
        {
            CurrentCommand = CommandMode.Translation;
            _cameraController.IsActive = false;
            _commandPanelCanvasGroup.interactable = false;
            _dictionaryCanvasGroup.interactable = false;
            _selectedGameObjectStack.Push(EventSystem.current.currentSelectedGameObject);

            ShowTranslation(EventSystem.current.currentSelectedGameObject.GetComponent<CommandButton>().SelectLangNum, EventSystem.current.currentSelectedGameObject.GetComponent<CommandButton>().SelectLang);
        }
        if (command == "Hint")
        {
            CurrentCommand = CommandMode.Hint;
            _cameraController.IsActive = false;
            _commandPanelCanvasGroup.interactable = false;
            _dictionaryCanvasGroup.interactable = false;

            ShowHint(EventSystem.current.currentSelectedGameObject.GetComponent<CommandButton>().SelectLangNum);
        }
    }

    //�@�L�����N�^�[�̃X�e�[�^�X�\��
    public void ShowStatus(PlayerStatus playerStatus)
    {
        CurrentCommand = CommandMode.Status;
        _statusPanel.SetActive(true);
        //�@�L�����N�^�[�̖��O��\��
        _characterNameText.text = playerStatus.CharacterName;

        //�@�^�C�g���̕\��
        var text = "���x��\n";
        text += "HP\n";
        text += "MP\n";
        text += "���C\n";
        text += "�o���l\n";
        text += "��������\n";
        text += "�����Z\n";
        text += "�U����\n";
        text += "�h���\n";
        text += "��Ԉُ�\n";
        _statusTitleText.text = text;

        //�@�X�e�[�^�X�p�����[�^�̕\��
        text = playerStatus.Level + "\n";
        text += playerStatus.Hp + " / " + playerStatus.MaxHp + "\n";
        text += playerStatus.Mp + " / " + playerStatus.MaxMp + "\n";
        text += playerStatus.Mot + "\n";
        text += playerStatus.EarnedExperience + "\n";
        text += playerStatus.EquipWeapon != null ? playerStatus.EquipWeapon.ItemName : "";
        text += "\n";
        text += playerStatus.EquipArmor != null ? playerStatus.EquipArmor.ItemName : "";
        text += "\n";
        text += playerStatus.Power + (playerStatus.EquipWeapon?.ItemAmount ?? 0) + "\n";
        text += playerStatus.Defense + (playerStatus.EquipArmor?.ItemAmount ?? 0) + "\n";
        if (playerStatus.IsPoisonStatus == true)
        {
            text += "�� ";
        }
        if (playerStatus.IsSleepStatus == true)
        {
            text += "���� ";
        }
        if (playerStatus.IsParalysisStatus == true)
        {
            text += "��� ";
        }
        if (playerStatus.IsConfusionStatus == true)
        {
            text += "���� ";
        }
        if (playerStatus.IsDepressionStatus == true)
        {
            text += "�T ";
        }
        _statusParamText.text = text;
    }

    //�@�A�C�e���\��
    public void ShowItem(PlayerStatus playerStatus)
    {
        _itemInfoPanel.SetActive(true);

        //�@�A�C�e���p�l���{�^�������쐬�������ǂ���
        int itemPanelButtonNum = 0;
        GameObject itemButtonIns;
        //�@�v���C���[�̃A�C�e�������A�C�e���p�l���{�^�����쐬
        //�@�����Ă���A�C�e�����̃{�^���̍쐬�ƃN���b�N���̎��s���\�b�h�̐ݒ�
        foreach (var item in playerStatus.ItemDictionary.Keys)
        {
            itemButtonIns = Instantiate<GameObject>(_itemPanelButtonPrefab, _content.transform);
            itemButtonIns.transform.Find("ItemName").GetComponent<TextMeshProUGUI>().text = item.ItemName;
            itemButtonIns.GetComponent<Button>().onClick.AddListener(() => SelectItem(playerStatus, item));
            itemButtonIns.GetComponent<ItemPanelButtonScript>().SetParam(item);

            //�@�A�C�e������\��
            itemButtonIns.transform.Find("Num").GetComponent<TextMeshProUGUI>().text = playerStatus.ItemDictionary[item].ToString();

            //�@�������Ă��镐���h��ɂ͖��O�̑O��E��\�����A����Text��ێ����Ēu��
            if (playerStatus.EquipWeapon == item)
            {
                itemButtonIns.transform.Find("Equip").GetComponent<TextMeshProUGUI>().text = "E";
            }
            else if (playerStatus.EquipArmor == item)
            {
                itemButtonIns.transform.Find("Equip").GetComponent<TextMeshProUGUI>().text = "E";
            }

            //�@�A�C�e���{�^�����X�g�ɒǉ�
            _itemPanelButtonList.Add(itemButtonIns);
            //�@�A�C�e���p�l���{�^���ԍ����X�V
            itemPanelButtonNum++;
        }

        //�@�A�C�e���p�l���̕\���ƍŏ��̃A�C�e���̑I��
        if (_content.transform.childCount != 0)
        {
            //�@SelectCharacerPanel�ōŌ�ɂǂ̃Q�[���I�u�W�F�N�g��I�����Ă�����
            _selectedGameObjectStack.Push(EventSystem.current.currentSelectedGameObject);
            CurrentCommand = CommandMode.Item;
            _itemPanel.SetActive(true);
            _itemPanel.transform.SetAsLastSibling();
            _itemPanelCanvasGroup.interactable = true;
            EventSystem.current.SetSelectedGameObject(_content.transform.GetChild(0).gameObject);
        }
        else
        {
            _infoTitleText.text = "";
            _infoText.text = "�A�C�e���������Ă��܂���B";
        }
    }

    //�@�A�C�e�����ǂ����邩�̑I��
    public void SelectItem(PlayerStatus playerStatus, Item item)
    {
        //�@�A�C�e���̎�ނɉ����ďo���鍀�ڂ�ύX����
        if (item.ItemType == Item.Type.Armor
            || item.ItemType == Item.Type.Weapon
            )
        {
            var itemMenuButtonIns = Instantiate<GameObject>(_useItemPanelButtonPrefab, _useItemPanel.transform);
            if (item == playerStatus.EquipWeapon || item == playerStatus.EquipArmor)
            {
                itemMenuButtonIns.GetComponentInChildren<TextMeshProUGUI>().text = "�������O��";
                itemMenuButtonIns.GetComponent<Button>().onClick.AddListener(() => RemoveEquip(playerStatus, item));
            }
            else
            {
                itemMenuButtonIns.GetComponentInChildren<TextMeshProUGUI>().text = "��������";
                itemMenuButtonIns.GetComponent<Button>().onClick.AddListener(() => Equip(playerStatus, item));
            }

            itemMenuButtonIns = Instantiate<GameObject>(_useItemPanelButtonPrefab, _useItemPanel.transform);
            itemMenuButtonIns.GetComponentInChildren<TextMeshProUGUI>().text = "�̂Ă�";
            itemMenuButtonIns.GetComponent<Button>().onClick.AddListener(() => ThrowAwayItem(playerStatus, item));

        }
        else if (item.ItemType == Item.Type.HPRecovery
                 || item.ItemType == Item.Type.MPRecovery
                 || item.ItemType == Item.Type.PoisonRecovery
                 || item.ItemType == Item.Type.SleepRecovery
                 || item.ItemType == Item.Type.ParalysisRecovery
                 || item.ItemType == Item.Type.ConfusionRecovery
                 || item.ItemType == Item.Type.DepressionRecovery
                )
        {
            var itemMenuButtonIns = Instantiate<GameObject>(_useItemPanelButtonPrefab, _useItemPanel.transform);
            itemMenuButtonIns.GetComponentInChildren<TextMeshProUGUI>().text = "�g��";
            itemMenuButtonIns.GetComponent<Button>().onClick.AddListener(() => UseItem(playerStatus, item));

            itemMenuButtonIns = Instantiate<GameObject>(_useItemPanelButtonPrefab, _useItemPanel.transform);
            itemMenuButtonIns.GetComponentInChildren<TextMeshProUGUI>().text = "�̂Ă�";
            itemMenuButtonIns.GetComponent<Button>().onClick.AddListener(() => ThrowAwayItem(playerStatus, item));

        }
        else if (item.ItemType == Item.Type.Valuables)
        {
            _infoTitleText.text = item.ItemName;
            _infoText.text = item.ItemInfo;
        }

        if (item.ItemType != Item.Type.Valuables)
        {
            _useItemPanel.SetActive(true);
            _itemPanelCanvasGroup.interactable = false;
            CurrentCommand = CommandMode.UseItem;
            //�@ItemPanel�ōŌ�ɂǂ��I�����Ă������H
            _selectedGameObjectStack.Push(EventSystem.current.currentSelectedGameObject);

            _useItemPanel.transform.SetAsLastSibling();
            EventSystem.current.SetSelectedGameObject(_useItemPanel.transform.GetChild(0).gameObject);
            _useItemPanelCanvasGroup.interactable = true;
            Input.ResetInputAxes();

        }
    }

    //�@�A�C�e�����g��
    public void UseItem(PlayerStatus playerStatus, Item item)
    {
        _useItemInfoPanel.SetActive(true);

        if (item.ItemType == Item.Type.HPRecovery)
        {
            if (playerStatus.Hp == playerStatus.MaxHp)
            {
                _useItemInfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = playerStatus.CharacterName + "�͌��C�ł��B";
            }
            else
            {
                playerStatus.Hp += item.ItemAmount;
                //�@�A�C�e�����g�p�����|��\��
                _useItemInfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = "HP��" + item.ItemAmount + "�񕜂��܂����B";
                //�@�����Ă���A�C�e���������炷
                playerStatus.ItemDictionary[item] -= 1;
            }
        }
        else if (item.ItemType == Item.Type.MPRecovery)
        {
            if (playerStatus.Mp == playerStatus.MaxMp)
            {
                _useItemInfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = playerStatus.CharacterName + "��MP�͍ő�ł��B";
            }
            else
            {
                playerStatus.Mp += Mathf.FloorToInt(playerStatus.MaxHp * (item.ItemAmount * 0.01f));
                //�@�A�C�e�����g�p�����|��\��
                _useItemInfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = "MP��" + item.ItemAmount + "�񕜂��܂����B";
                //�@�����Ă���A�C�e���������炷
                playerStatus.ItemDictionary[item] -= 1;
            }
        }
        else if (item.ItemType == Item.Type.MotRecovery)
        {
            if (playerStatus.Mot == playerStatus.MaxMot)
            {
                _useItemInfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = playerStatus.CharacterName + "�̂��C�͍ő�ł��B";
            }
            else
            {
                playerStatus.Mot += item.ItemAmount;
                //�@�A�C�e�����g�p�����|��\��
                _useItemInfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = "���C��" + item.ItemAmount + "�񕜂��܂����B";
                //�@�����Ă���A�C�e���������炷
                playerStatus.ItemDictionary[item] -= 1;
            }
        }
        else if (item.ItemType == Item.Type.PoisonRecovery)
        {
            if (playerStatus.IsPoisonStatus != true)
            {
                _useItemInfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = playerStatus.CharacterName + "�͓ŏ�Ԃł͂���܂���B";
            }
            else
            {
                _useItemInfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = playerStatus.CharacterName + "�͓ł���񕜂��܂����B";
                playerStatus.IsPoisonStatus = false;
                playerStatus.Hp -= Mathf.FloorToInt(playerStatus.MaxHp * 0.2f);
                //�@�����Ă���A�C�e���������炷
                playerStatus.ItemDictionary[item] -= 1;
            }
        }
        else if (item.ItemType == Item.Type.SleepRecovery)
        {
            if (playerStatus.IsSleepStatus != true)
            {
                _useItemInfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = playerStatus.CharacterName + "�͍�����Ԃł͂���܂���B";
            }
            else
            {
                _useItemInfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = playerStatus.CharacterName + "�͍�������񕜂��܂����B";
                playerStatus.IsSleepStatus = false;
                playerStatus.Hp -= Mathf.FloorToInt(playerStatus.MaxHp * 0.2f);
                //�@�����Ă���A�C�e���������炷
                playerStatus.ItemDictionary[item] -= 1;
            }
        }
        else if (item.ItemType == Item.Type.ParalysisRecovery)
        {
            if (playerStatus.IsParalysisStatus != true)
            {
                _useItemInfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = playerStatus.CharacterName + "�͖�჏�Ԃł͂���܂���B";
            }
            else
            {
                _useItemInfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = playerStatus.CharacterName + "�͖�Ⴢ���񕜂��܂����B";
                playerStatus.IsParalysisStatus = false;
                playerStatus.Hp -= Mathf.FloorToInt(playerStatus.MaxHp * 0.2f);
                //�@�����Ă���A�C�e���������炷
                playerStatus.ItemDictionary[item] -= 1;
            }
        }
        else if (item.ItemType == Item.Type.ConfusionRecovery)
        {
            if (playerStatus.IsConfusionStatus != true)
            {
                _useItemInfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = playerStatus.CharacterName + "�͍�����Ԃł͂���܂���B";
            }
            else
            {
                _useItemInfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = playerStatus.CharacterName + "�͍�������񕜂��܂����B";
                playerStatus.IsConfusionStatus = false;
                playerStatus.Hp -= Mathf.FloorToInt(playerStatus.MaxHp * 0.2f);
                //�@�����Ă���A�C�e���������炷
                playerStatus.ItemDictionary[item] -= 1;
            }
        }
        else if (item.ItemType == Item.Type.DepressionRecovery)
        {
            if (playerStatus.IsDepressionStatus != true)
            {
                _useItemInfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = playerStatus.CharacterName + "�͟T��Ԃł͂���܂���B";
            }
            else
            {
                _useItemInfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = playerStatus.CharacterName + "�͟T����񕜂��܂����B";
                playerStatus.IsDepressionStatus = false;
                playerStatus.Hp -= Mathf.FloorToInt(playerStatus.MaxHp * 0.5f);
                //�@�����Ă���A�C�e���������炷
                playerStatus.ItemDictionary[item] -= 1;
            }
        }
        else if (item.ItemType == Item.Type.Revival)
        {
            _useItemInfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = "���̃A�C�e���͎g�p�ł��܂���B";
        }
        else if (item.ItemType == Item.Type.PowerUp)
        {
            _useItemInfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = "���̃A�C�e���͎g�p�ł��܂���B";
        }
        else if (item.ItemType == Item.Type.DefenseUp)
        {
            _useItemInfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = "���̃A�C�e���͎g�p�ł��܂���B";
        }

        //�@itemPanleButtonList����Y������A�C�e����T�������X�V����
        var itemButton = _itemPanelButtonList.Find(obj => obj.transform.Find("ItemName").GetComponent<TextMeshProUGUI>().text == item.ItemName);
        itemButton.transform.Find("Num").GetComponent<TextMeshProUGUI>().text = playerStatus.ItemDictionary[item].ToString();

        //�@�A�C�e������0��������{�^���ƃL�����N�^�[�X�e�[�^�X����A�C�e�����폜
        if (playerStatus.ItemDictionary[item] == 0)
        {
            //�@�A�C�e����0�ɂȂ������C��ItemPanel�ɖ߂��ׁAUseItemPanel���̃I�u�W�F�N�g�o�^���폜
            _selectedGameObjectStack.Pop();
            _selectedGameObjectStack.Pop();
            //�@itemPanelButtonList����A�C�e���p�l���{�^�����폜
            _itemPanelButtonList.Remove(itemButton);
            //�@�A�C�e���p�l���{�^�����g�̍폜
            Destroy(itemButton);
            //�@�A�C�e����n�����L�����N�^�[���g��ItemDictionary���炻�̃A�C�e�����폜
            playerStatus.ItemDictionary.Remove(item);
            //�@ItemPanel�ɖ߂�ׁAUseItemPanel���ɍ�����{�^����S�폜
            for (int i = _useItemPanel.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(_useItemPanel.transform.GetChild(i).gameObject);
            }
            //�@�A�C�e������0�ɂȂ����̂�CommandMode.NoItemPassed�ɕύX
            CurrentCommand = CommandMode.NoItemPassed;
            _useItemInfoPanel.transform.SetAsLastSibling();
            Input.ResetInputAxes();
        }
        else
        {
            //�@�A�C�e�������c���Ă���ꍇ��UseItemPanel�ŃA�C�e�����ǂ����邩�̑I���ɖ߂�
            CurrentCommand = CommandMode.UseItemToUseItem;
            _useItemInfoPanel.transform.SetAsLastSibling();
            Input.ResetInputAxes();
        }
    }

    //�@�A�C�e�����̂Ă�
    public void ThrowAwayItem(PlayerStatus playerStatus, Item item)
    {
        //�@�A�C�e���������炷
        playerStatus.ItemDictionary[item] -= 1;
        //�@�A�C�e������0�ɂȂ�����
        if (playerStatus.ItemDictionary[item] == 0)
        {

            //�@�������Ă��镐����̂Ă�ꍇ�̏���
            if (item == playerStatus.EquipArmor)
            {
                var equipArmorButton = _itemPanelButtonList.Find(itemPanelButton => itemPanelButton.transform.Find("ItemName").GetComponent<TextMeshProUGUI>().text == item.ItemName);
                equipArmorButton.transform.Find("Equip").GetComponent<TextMeshProUGUI>().text = "";
                equipArmorButton = null;
                playerStatus.EquipArmor = null;
            }
            else if (item == playerStatus.EquipWeapon)
            {
                var equipWeaponButton = _itemPanelButtonList.Find(itemPanelButton => itemPanelButton.transform.Find("ItemName").GetComponent<TextMeshProUGUI>().text == item.ItemName);
                equipWeaponButton.transform.Find("Equip").GetComponent<TextMeshProUGUI>().text = "";
                equipWeaponButton = null;
                playerStatus.EquipWeapon = null;
            }
        }
        //�@ItemPanel�̎q�v�f�̃A�C�e���p�l���{�^������Y������A�C�e���̃{�^����T���Đ����X�V����
        var itemButton = _itemPanelButtonList.Find(obj => obj.transform.Find("ItemName").GetComponent<TextMeshProUGUI>().text == item.ItemName);
        itemButton.transform.Find("Num").GetComponent<TextMeshProUGUI>().text = playerStatus.ItemDictionary[item].ToString();
        _useItemInfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = item.ItemName + "���̂Ă܂����B";

        //�@�A�C�e������0��������{�^���ƃL�����N�^�[�X�e�[�^�X����A�C�e�����폜
        if (playerStatus.ItemDictionary[item] == 0)
        {
            _selectedGameObjectStack.Pop();
            _itemPanelButtonList.Remove(itemButton);
            Destroy(itemButton);
            playerStatus.ItemDictionary.Remove(item);

            CurrentCommand = CommandMode.NoItemPassed;
            _useItemPanelCanvasGroup.interactable = false;
            _useItemPanel.SetActive(false);
            _useItemInfoPanel.transform.SetAsLastSibling();
            _useItemInfoPanel.SetActive(true);
            //�@ItemPanel�ɖ߂��UseItemPanel�̎q�v�f�̃{�^����S�폜
            for (int i = _useItemPanel.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(_useItemPanel.transform.GetChild(i).gameObject);
            }
        }
        else
        {
            _useItemPanelCanvasGroup.interactable = false;
            _useItemInfoPanel.transform.SetAsLastSibling();
            _useItemInfoPanel.SetActive(true);
            CurrentCommand = CommandMode.UseItemToUseItem;
        }

        Input.ResetInputAxes();
    }

    //�@�A�C�e���𑕔�����
    public void Equip(PlayerStatus playerStatus, Item item)
    {
        if (item.ItemType == Item.Type.Armor)
        {
            var equipArmorButton = _itemPanelButtonList.Find(itemPanelButton => itemPanelButton.transform.Find("ItemName").GetComponent<TextMeshProUGUI>().text == item.ItemName);
            equipArmorButton.transform.Find("Equip").GetComponent<TextMeshProUGUI>().text = "E";
            //�@�������Ă���Z�������ItemPanel��Equip��E���O��
            if (playerStatus.EquipArmor != null)
            {
                equipArmorButton = _itemPanelButtonList.Find(itemPanelButton => itemPanelButton.transform.Find("ItemName").GetComponent<TextMeshProUGUI>().text == playerStatus.EquipArmor.ItemName);
                equipArmorButton.transform.Find("Equip").GetComponent<TextMeshProUGUI>().text = "";
            }
            playerStatus.EquipArmor = item;
            _useItemInfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = item.ItemName + "�𑕔����܂����B";
        }
        else if (item.ItemType == Item.Type.Weapon)
        {
            var equipWeaponButton = _itemPanelButtonList.Find(itemPanelButton => itemPanelButton.transform.Find("ItemName").GetComponent<TextMeshProUGUI>().text == item.ItemName);
            equipWeaponButton.transform.Find("Equip").GetComponent<TextMeshProUGUI>().text = "E";
            //�@�������Ă��镐�킪�����ItemPanel��Equip��E���O��
            if (playerStatus.EquipWeapon != null)
            {
                equipWeaponButton = _itemPanelButtonList.Find(itemPanelButton => itemPanelButton.transform.Find("ItemName").GetComponent<TextMeshProUGUI>().text == playerStatus.EquipWeapon.ItemName);
                equipWeaponButton.transform.Find("Equip").GetComponent<TextMeshProUGUI>().text = "";
            }
            playerStatus.EquipWeapon = item;
            _useItemInfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = item.ItemName + "�𑕔����܂����B";
        }

        //�@������؂�ւ�����ItemPanel�ɖ߂�
        _useItemPanelCanvasGroup.interactable = false;
        _useItemPanel.SetActive(false);
        _itemPanelCanvasGroup.interactable = true;
        //�@ItemPanel�ɖ߂�̂�UseItemPanel�̎q�v�f�ɍ�����{�^����S�폜
        for (int i = _useItemPanel.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(_useItemPanel.transform.GetChild(i).gameObject);
        }

        _useItemInfoPanel.transform.SetAsLastSibling();
        _useItemInfoPanel.SetActive(true);

        CurrentCommand = CommandMode.UseItemToItem;

        Input.ResetInputAxes();
    }

    //�@�������O��
    public void RemoveEquip(PlayerStatus playerStatus, Item item)
    {
        //�@�A�C�e���̎�ނɉ����đ������O��
        if (item.ItemType == Item.Type.Armor)
        {
            var equipArmorButton = _itemPanelButtonList.Find(itemPanelButton => itemPanelButton.transform.Find("ItemName").GetComponent<TextMeshProUGUI>().text == item.ItemName);
            equipArmorButton.transform.Find("Equip").GetComponent<TextMeshProUGUI>().text = "";
            playerStatus.EquipArmor = null;
        }
        else if (item.ItemType == Item.Type.Weapon)
        {
            var equipArmorButton = _itemPanelButtonList.Find(itemPanelButton => itemPanelButton.transform.Find("ItemName").GetComponent<TextMeshProUGUI>().text == item.ItemName);
            equipArmorButton.transform.Find("Equip").GetComponent<TextMeshProUGUI>().text = "";
            playerStatus.EquipWeapon = null;
        }
        //�@�������O�����|��\��
        _useItemInfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = item.ItemName + "���O���܂����B";
        //�@�������O������ItemPanel�ɖ߂鏈��
        _useItemPanelCanvasGroup.interactable = false;
        _useItemPanel.SetActive(false);
        _itemPanelCanvasGroup.interactable = true;
        //�@ItemPanel�ɖ߂�̂�UseItemPanel�̎q�v�f�̃{�^����S�폜
        for (int i = _useItemPanel.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(_useItemPanel.transform.GetChild(i).gameObject);
        }

        _useItemInfoPanel.transform.SetAsLastSibling();
        _useItemInfoPanel.SetActive(true);

        CurrentCommand = CommandMode.UseItemToItem;
        Input.ResetInputAxes();
    }

    //�@�����\��
    public void ShowDictionary(TalkManager talkManager)
    {
        CurrentCommand = CommandMode.Dictionary;
        _dictionaryPanel.SetActive(true);
        //_nextButton.SetActive(true);
        //_previousButton.SetActive(true);

        int startY = 0;
        int i = 0;
        int j = 0;

        foreach (var word in talkManager.WordDictionary)
        {
            if (j == _maxWordsNum)
            {
                startY += 550;
                j = 0;
            }
            j++;

            //����𐶐�����ʒu
            Vector2 pos = new Vector2(600, startY - 60 * ((i - 1) + 1));
            //����𐶐�
            GameObject originReal = Instantiate(_originReal, new Vector3(0, 0, 0), Quaternion.identity, _dictionaryParent.transform);
            originReal.transform.localPosition = pos;
            _originReals.Add(originReal);
            //��������������{�^���Ƃ��Ď擾�AOnClick��ݒ�
            Button originRealButton = originReal.GetComponent<Button>();
            originRealButton.onClick.AddListener(() => SelectCommand("Translation"));
            //�I����������̃C���f�b�N�X�ԍ���n��
            CommandButton commandButton = originReal.GetComponent<CommandButton>();
            commandButton.SelectLangNum = i;
            commandButton.SelectLang = word.Key;
            TextMeshProUGUI origin = originReal.transform.Find("Origin").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI real = originReal.transform.Find("Real").GetComponent<TextMeshProUGUI>();
            //�����\��
            origin.text = _playerTalk.ConvFoolCoolFont(word.Value.OriginalWord);
            if (word.Value.SelectWord != null)
            {
                real.text = word.Value.SelectWord;
                //if (i == 2)
                //{
                //    EventSystem.current.SetSelectedGameObject(originReal);
                //}
            }
            else
            {
                real.text = "";
            }
            //�q���g�{�^������
            if (talkManager.HintLists[i].IsHint == true)
            {
                GameObject hintButtonObject = Instantiate(_hintButton, new Vector3(0, 0, 0), Quaternion.identity, _dictionaryParent.transform);
                hintButtonObject.transform.localPosition = pos + new Vector2(650, 0);
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

        //�P�O��\���i����ȊO�͔�\���j
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

    //�@�|��I��
    public void ShowTranslation(int num, string word)
    {
        CurrentCommand = CommandMode.Translation;
        _dictionaryChoisePanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_transChoise1);

        //�󂯎�����C���f�b�N�X�ԍ��ɉ��������A�������\��
        string[] wordChoises = new string[3];
        wordChoises[0] = _talkManager.WordChoises[num].Word1;
        wordChoises[1] = _talkManager.WordChoises[num].Word2;
        wordChoises[2] = _talkManager.WordChoises[num].Word3;
        wordChoises = wordChoises.OrderBy(a => Guid.NewGuid()).ToArray();
        _transChoiseText1.text = wordChoises[0];
        _transChoiseText2.text = wordChoises[1];
        _transChoiseText3.text = wordChoises[2];
        //�󂯎�����C���f�b�N�X�ԍ���|��I����CommandButton�ɓn��
        _transChoise1.GetComponent<CommandButton>().SelectLangNum = num;
        _transChoise2.GetComponent<CommandButton>().SelectLangNum = num;
        _transChoise3.GetComponent<CommandButton>().SelectLangNum = num;
        //�󂯎�����������|��I����CommandButton�ɓn��
        _transChoise1.GetComponent<CommandButton>().SelectLang = word;
        _transChoise2.GetComponent<CommandButton>().SelectLang = word;
        _transChoise3.GetComponent<CommandButton>().SelectLang = word;
    }

    //�@�|��I����ACanvas���X�V
    public void UpdateDictionary()
    {
        _dictionaryCanvasGroup.interactable = true;
        CurrentCommand = CommandMode.Dictionary;
        _dictionaryChoisePanel.SetActive(false);
        _dictionaryPanel.SetActive(false);
        DestroyOriginReal();
        ShowDictionary(_talkManager);
    }

    //�@OriginReal��j��
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

    //�@�q���g�{��
    public void ShowHint(int num)
    {
        CurrentCommand = CommandMode.Hint;
        _hintPanel.SetActive(true);

        //�@�q���g��\��
        _hint1.sprite = _talkManager.HintLists[num].Hint1;
    }

    //�@�����̃y�[�W����
    public void NextPage()
    {
        if (CurrentCommand != CommandMode.Dictionary)
        {
            return;
        }

        _currentPage++;
        _maxPage = _originReals.Count / _maxWordsNum + 1;
        bool isSelect = true;

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

    //�@�����̃y�[�W�߂�
    public void PreviousPage()
    {
        if (CurrentCommand != CommandMode.Dictionary)
        {
            return;
        }

        _currentPage--;
        _maxPage = _originReals.Count / _maxWordsNum + 1;
        bool isSelect = true;

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

    //�@ExitCommand�̎��Ƀp�l�������������Ă���
    public void ExitCommandPanel()
    {
        _dictionaryPanel.SetActive(false);
        _dictionaryChoisePanel.SetActive(false);
        _hintPanel.SetActive(false);
        //_nextButton.SetActive(false);
        //_previousButton.SetActive(false);
        DestroyOriginReal();
    }
}

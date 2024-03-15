using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class AllFlags
{
    [SerializeField]
    private string _flagName;
    [SerializeField]
    private bool _flagBool;

    public string FlagName { get => _flagName; set => _flagName = value; }
    public bool FlagBool { get => _flagBool; set => _flagBool = value; }
}

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;

    [SerializeField]
    private DataManager _dataManager;
    [SerializeField]
    private SceneMove _sceneMove;
    [SerializeField]
    private PlayerStatus _playerStatus;
    [SerializeField]
    private ItemDictionary _itemDictionary;
    [SerializeField]
    private TalkManager _talkManager;
    [SerializeField]
    private QuestManager _questManager;

    //�@enum�̃Z�[�u�����[�h�̂��߂̕ϐ�
    private int _enumValue;

    //�@�t���O���܂Ƃ߂ĊǗ�����
    private Dictionary<string, bool> _allGameFlags = new Dictionary<string, bool>();

    [SerializeField]
    private List<AllFlags> _allFlags = new List<AllFlags>();

    //�@�A�����A�i�����j�̉�b�t�F�[�Y�Ǘ�
    [SerializeField]
    private int _ameriaCount = 0;

    ////�@�A�����A�Ɖ�b���A������ꍇ�Ə����Ȃ��ꍇ
    //[SerializeField]
    //private bool _isHelpAmeria = false;
    //[SerializeField]
    //private bool _isNotHelpAmeria = false;
    ////�@�A�����A�̈˗����N���A
    //[SerializeField]
    //private bool _isClearAmeria = false;

    //�@�����̍D���x
    [SerializeField]
    private int _favorability = 1;
    //�@�Q�[������N��
    [SerializeField]
    private bool _initialize = true;
    // ����̉�b�̌�ɉ�b��������Z�b�g������
    private bool _firstConv = true;
    // �����t���O
    private bool _isSleep = false;

    public Dictionary<string, bool> AllGameFlags { get => _allGameFlags; set => _allGameFlags = value; }
    public List<AllFlags> AllFlags { get => _allFlags; set => _allFlags = value; }
    public int AmeriaCount { get => _ameriaCount; set => _ameriaCount = value; }
    public int Favorability { get => _favorability; set => _favorability = value; }
    public bool Initialize { get => _initialize; set => _initialize = value; }
    public bool FirstConv { get => _firstConv; set => _firstConv = value; }
    public bool IsSleep { get => _isSleep; set => _isSleep = value; }

    private void Awake()
    {
        // GameManger�͏�Ɉ�����ɂ���
        if (gameManager == null)
        {
            gameManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _dataManager = GetComponent<DataManager>();

        for (int i = 0; i < AllFlags.Count; i++)
        {
            AllGameFlags.Add(AllFlags[i].FlagName, AllFlags[i].FlagBool);
        }
    }

    private void Update()
    {
        // ���V�p���Z�b�g����
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            //_isHelpAmeria = false;
            //_isNotHelpAmeria = false;
            _allGameFlags["�A�����A��������"] = false;
            _allGameFlags["�A�����A�������Ȃ�"] = false;

            _questManager.QuestList[0].IsHappen = false;
            _questManager.QuestList[0].IsClear = false;
            _questManager.QuestList[1].IsHappen = false;
            _questManager.QuestList[1].IsClear = false;
            _questManager.QuestList[2].IsHappen = false;
            _questManager.QuestList[2].IsClear = false;
            _ameriaCount = 0;
        }
    }

    public void SaveGame()
    {
        Data data = new Data();
        //data.sceneMove = _sceneMove;
        data.playerStatus = _playerStatus;
        data.itemDictionary = _itemDictionary;
        data.talkManager = _talkManager;

        _enumValue = (int)_sceneMove.SceneType1;

        _dataManager.SaveData(data);
    }

    public void LoadGame()
    {
        Data data = _dataManager.LoadData();
        if (data != null)
        {
            //_sceneMove = data.sceneMove;
            _playerStatus = data.playerStatus;
            _itemDictionary = data.itemDictionary;
            _talkManager = data.talkManager;

            _sceneMove.SceneType1 = (SceneMove.SceneType)_enumValue;

            // �v���C���[�ƃA�C�e���̃f�[�^���Q�[�����̗v�f�ɔ��f����
            UpdateGameElements();
        }
    }

    private void UpdateGameElements()
    {
        // �Q�[�����v�f�̍X�V�������L�q
        // �K�v�ȗv�f����̓I�ɔ��f�����鏈������������

        //�@�V�[���̓ǂݍ���
        LoadSceneManager loadSceneManager = LoadSceneManager.loadSceneManager;
        loadSceneManager.GoToNextScene(_sceneMove.SceneType1);
    }
}

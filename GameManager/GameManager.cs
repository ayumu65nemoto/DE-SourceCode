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

    //　enumのセーブ＆ロードのための変数
    private int _enumValue;

    //　フラグをまとめて管理する
    private Dictionary<string, bool> _allGameFlags = new Dictionary<string, bool>();

    [SerializeField]
    private List<AllFlags> _allFlags = new List<AllFlags>();

    //　アメリア（村長）の会話フェーズ管理
    [SerializeField]
    private int _ameriaCount = 0;

    ////　アメリアと会話し、助ける場合と助けない場合
    //[SerializeField]
    //private bool _isHelpAmeria = false;
    //[SerializeField]
    //private bool _isNotHelpAmeria = false;
    ////　アメリアの依頼をクリア
    //[SerializeField]
    //private bool _isClearAmeria = false;

    //　少女の好感度
    [SerializeField]
    private int _favorability = 1;
    //　ゲーム初回起動
    [SerializeField]
    private bool _initialize = true;
    // 初手の会話の後に会話相手をリセットしたい
    private bool _firstConv = true;
    // 睡眠フラグ
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
        // GameMangerは常に一つだけにする
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
        // 試遊用リセット処理
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            //_isHelpAmeria = false;
            //_isNotHelpAmeria = false;
            _allGameFlags["アメリアを助ける"] = false;
            _allGameFlags["アメリアを助けない"] = false;

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

            // プレイヤーとアイテムのデータをゲーム内の要素に反映する
            UpdateGameElements();
        }
    }

    private void UpdateGameElements()
    {
        // ゲーム内要素の更新処理を記述
        // 必要な要素を具体的に反映させる処理を実装する

        //　シーンの読み込み
        LoadSceneManager loadSceneManager = LoadSceneManager.loadSceneManager;
        loadSceneManager.GoToNextScene(_sceneMove.SceneType1);
    }
}

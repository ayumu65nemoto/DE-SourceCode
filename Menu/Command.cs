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
    //　ユニティちゃんコマンドスクリプト
    private MenuCommand _menuCommand;

    //　最初に選択するButtonのTransform
    [SerializeField]
    private GameObject _firstSelectButton;

    //　コマンドパネル
    [SerializeField]
    private GameObject _commandPanel;
    //　ステータスボタン
    [SerializeField]
    private GameObject _statusButton;
    //　辞書ボタン
    [SerializeField]
    private GameObject _dictionaryButton;
    //　ステータス表示パネル
    [SerializeField]
    private GameObject _statusPanel;
    //　辞書表示パネル
    [SerializeField]
    private GameObject _dictionaryPanel;
    //　翻訳選択肢パネル
    [SerializeField]
    private GameObject _dictionaryChoisePanel;
    //　ヒントパネル
    [SerializeField]
    private GameObject _hintPanel;
    //　ページ送りボタン
    //[SerializeField]
    //private GameObject _nextButton;
    //[SerializeField]
    //private GameObject _previousButton;

    //　コマンドパネルのCanvasGroup
    private CanvasGroup _commandPanelCanvasGroup;
    //　辞書コマンドのCanvasGroup
    private CanvasGroup _dictionaryCanvasGroup;
    //　辞書コマンドのCanvasGroup
    private CanvasGroup _transCanvasGroup;

    //　キャラクター名
    [SerializeField]
    private TextMeshProUGUI _characterNameText;
    //　ステータスタイトルテキスト
    [SerializeField]
    private TextMeshProUGUI _statusTitleText;
    //　ステータスパラメータテキスト1
    [SerializeField]
    private TextMeshProUGUI _statusParamText;
    //　プレイヤーステータス
    [SerializeField]
    private PlayerStatus _playerStatus = null;
    //　辞書に表示する言語
    [SerializeField]
    private GameObject _originReal;
    //　ヒントボタン
    [SerializeField]
    private GameObject _hintButton;
    //　辞書の表示する部分
    [SerializeField]
    private GameObject _dictionaryParent;
    //　オリジナル言語
    private TextMeshProUGUI _originLangText;
    //　現実の言語
    private TextMeshProUGUI _realLangText;
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
    //　ヒント１
    [SerializeField]
    private Image _hint1;
    //　言語インデックス
    [SerializeField]
    private TalkManager _talkManager = null;
    //PlayerTalk
    private PlayerTalk _playerTalk;

    //　生成したOriginRealを格納しておく(１０個単位で表示するため)
    private List<GameObject> _originReals = new List<GameObject>();
    //　ヒントも一緒に格納
    private List<GameObject> _hints = new List<GameObject>();

    //　辞書の最大表示数
    [SerializeField]
    private int _maxWordsNum = 9;
    //　辞書の今見ているページ
    private int _currentPage = 1;
    //　辞書の最終ページ
    private int _maxPage = 0;

    //　CameraController
    [SerializeField]
    private CameraController _cameraController;

    //　最後に選択していたゲームオブジェクトをスタック
    private Stack<GameObject> _selectedGameObjectStack = new Stack<GameObject>();

    //　アイテム表示パネル
    [SerializeField]
    private GameObject _itemPanel;
    //　アイテムパネルボタンを表示する場所
    [SerializeField]
    private GameObject _content;
    //　アイテムを使う選択パネル
    [SerializeField]
    private GameObject _useItemPanel;
    //　情報表示パネル
    [SerializeField]
    private GameObject _itemInfoPanel;
    //　アイテム使用後の情報表示パネル
    [SerializeField]
    private GameObject _useItemInfoPanel;

    //　アイテムパネルのCanvas Group
    private CanvasGroup _itemPanelCanvasGroup;
    //　アイテムを使う選択パネルのCanvasGroup
    private CanvasGroup _useItemPanelCanvasGroup;
   
    //　情報表示タイトルテキスト
    [SerializeField]
    private TextMeshProUGUI _infoTitleText;
    //　情報表示テキスト
    [SerializeField]
    private TextMeshProUGUI _infoText;

    //　キャラクターアイテムのボタンのプレハブ
    [SerializeField]
    private GameObject _itemPanelButtonPrefab = null;
    //　アイテム使用時のボタンのプレハブ
    [SerializeField]
    private GameObject _useItemPanelButtonPrefab = null;

    //　アイテムボタン一覧
    private List<GameObject> _itemPanelButtonList = new List<GameObject>();

    public CommandMode CurrentCommand { get => _currentCommand; set => _currentCommand = value; }

    private void Awake()
    {
        instance = this;

        //　コマンド画面を開く処理をしているUnityChanCommandScriptを取得
        _menuCommand = GameObject.FindWithTag("Player").GetComponent<MenuCommand>();
        //　現在のコマンドを初期化
        CurrentCommand = CommandMode.None;
        //　CanvasGroup
        _commandPanelCanvasGroup = _commandPanel.GetComponent<CanvasGroup>();
        _dictionaryCanvasGroup = _dictionaryParent.GetComponent<CanvasGroup>();
        _transCanvasGroup = _dictionaryChoisePanel.GetComponent<CanvasGroup>();
        
        //　アイテムに使うCanvasGroup
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
        //　キャンセルボタンを押した時の処理
        if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
        {
            //　コマンド選択画面時
            if (CurrentCommand == CommandMode.Command)
            {
                _menuCommand.ExitCommand();
                gameObject.SetActive(false);
                _cameraController.IsActive = true;
            }
            else if (CurrentCommand == CommandMode.Status)
            {
                _statusPanel.SetActive(false);
                //　前のパネルで選択していたゲームオブジェクトを選択
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
                //　リストをクリア
                _itemPanelButtonList.Clear();
                //　ItemPanelでCancelを押したらcontent以下のアイテムパネルボタンを全削除
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
                //　UseItemPanelでCancelボタンを押したらUseItemPanelの子要素のボタンの全削除
                for (int i = _useItemPanel.transform.childCount - 1; i >= 0; i--)
                {
                    Destroy(_useItemPanel.transform.GetChild(i).gameObject);
                }

                EventSystem.current.SetSelectedGameObject(_selectedGameObjectStack.Pop());
                _itemPanelCanvasGroup.interactable = true;
                CurrentCommand = CommandMode.Item;
            }
        }

        //　アイテムを装備、装備を外す情報表示後の処理
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
        //　アイテムを捨てるを選択した後の状態
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
        //　アイテムを使用、渡す、捨てるを選択した後にそのアイテムの数が0になった時
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

                //　アイテムパネルボタンがあれば最初のアイテムパネルボタンを選択
                if (_content.transform.childCount != 0)
                {
                    EventSystem.current.SetSelectedGameObject(_content.transform.GetChild(0).gameObject);
                }
                else
                {
                    //　アイテムパネルボタンがなければ（アイテムを持っていない）ItemSelectPanelに戻る
                    CurrentCommand = CommandMode.Item;
                    _itemPanelCanvasGroup.interactable = false;
                    _itemPanel.SetActive(false);
                    EventSystem.current.SetSelectedGameObject(_selectedGameObjectStack.Pop());
                }
            }
        }

        //　辞書を開いている時の操作
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

        //　選択解除された時（マウスでUI外をクリックした）は現在のモードによって無理やり選択させる
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
        //　現在のコマンドの初期化
        CurrentCommand = CommandMode.Command;
        //　コマンドメニュー表示時に他のパネルは非表示にする
        _statusPanel.SetActive(false);

        _selectedGameObjectStack.Clear();

        _commandPanelCanvasGroup.interactable = true;
        EventSystem.current.SetSelectedGameObject(_firstSelectButton);

        //　TalkManager取得
        if (_talkManager == null)
        {
            _talkManager = TalkManager.instance;
        }

        _itemPanel.SetActive(false);
        _useItemPanel.SetActive(false);
        _itemInfoPanel.SetActive(false);
        _useItemInfoPanel.SetActive(false);

        //　アイテムパネルボタンがあれば全て削除
        for (int i = _content.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(_content.transform.GetChild(i).gameObject);
        }
        //　アイテムを使うキャラクター選択ボタンがあれば全て削除
        for (int i = _useItemPanel.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(_useItemPanel.transform.GetChild(i).gameObject);
        }

        _itemPanelButtonList.Clear();

        _itemPanelCanvasGroup.interactable = false;
        _useItemPanelCanvasGroup.interactable = false;
    }

    //　選択したコマンドで処理分け
    public void SelectCommand(string command)
    {
        if (command == "Status")
        {
            CurrentCommand = CommandMode.Status;
            _cameraController.IsActive = false;
            //　UIのオン・オフや選択アイコンの設定
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

    //　キャラクターのステータス表示
    public void ShowStatus(PlayerStatus playerStatus)
    {
        CurrentCommand = CommandMode.Status;
        _statusPanel.SetActive(true);
        //　キャラクターの名前を表示
        _characterNameText.text = playerStatus.CharacterName;

        //　タイトルの表示
        var text = "レベル\n";
        text += "HP\n";
        text += "MP\n";
        text += "やる気\n";
        text += "経験値\n";
        text += "装備武器\n";
        text += "装備鎧\n";
        text += "攻撃力\n";
        text += "防御力\n";
        text += "状態異常\n";
        _statusTitleText.text = text;

        //　ステータスパラメータの表示
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
            text += "毒 ";
        }
        if (playerStatus.IsSleepStatus == true)
        {
            text += "昏睡 ";
        }
        if (playerStatus.IsParalysisStatus == true)
        {
            text += "麻痺 ";
        }
        if (playerStatus.IsConfusionStatus == true)
        {
            text += "混乱 ";
        }
        if (playerStatus.IsDepressionStatus == true)
        {
            text += "鬱 ";
        }
        _statusParamText.text = text;
    }

    //　アイテム表示
    public void ShowItem(PlayerStatus playerStatus)
    {
        _itemInfoPanel.SetActive(true);

        //　アイテムパネルボタンを何個作成したかどうか
        int itemPanelButtonNum = 0;
        GameObject itemButtonIns;
        //　プレイヤーのアイテム数分アイテムパネルボタンを作成
        //　持っているアイテム分のボタンの作成とクリック時の実行メソッドの設定
        foreach (var item in playerStatus.ItemDictionary.Keys)
        {
            itemButtonIns = Instantiate<GameObject>(_itemPanelButtonPrefab, _content.transform);
            itemButtonIns.transform.Find("ItemName").GetComponent<TextMeshProUGUI>().text = item.ItemName;
            itemButtonIns.GetComponent<Button>().onClick.AddListener(() => SelectItem(playerStatus, item));
            itemButtonIns.GetComponent<ItemPanelButtonScript>().SetParam(item);

            //　アイテム数を表示
            itemButtonIns.transform.Find("Num").GetComponent<TextMeshProUGUI>().text = playerStatus.ItemDictionary[item].ToString();

            //　装備している武器や防具には名前の前にEを表示し、そのTextを保持して置く
            if (playerStatus.EquipWeapon == item)
            {
                itemButtonIns.transform.Find("Equip").GetComponent<TextMeshProUGUI>().text = "E";
            }
            else if (playerStatus.EquipArmor == item)
            {
                itemButtonIns.transform.Find("Equip").GetComponent<TextMeshProUGUI>().text = "E";
            }

            //　アイテムボタンリストに追加
            _itemPanelButtonList.Add(itemButtonIns);
            //　アイテムパネルボタン番号を更新
            itemPanelButtonNum++;
        }

        //　アイテムパネルの表示と最初のアイテムの選択
        if (_content.transform.childCount != 0)
        {
            //　SelectCharacerPanelで最後にどのゲームオブジェクトを選択していたか
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
            _infoText.text = "アイテムを持っていません。";
        }
    }

    //　アイテムをどうするかの選択
    public void SelectItem(PlayerStatus playerStatus, Item item)
    {
        //　アイテムの種類に応じて出来る項目を変更する
        if (item.ItemType == Item.Type.Armor
            || item.ItemType == Item.Type.Weapon
            )
        {
            var itemMenuButtonIns = Instantiate<GameObject>(_useItemPanelButtonPrefab, _useItemPanel.transform);
            if (item == playerStatus.EquipWeapon || item == playerStatus.EquipArmor)
            {
                itemMenuButtonIns.GetComponentInChildren<TextMeshProUGUI>().text = "装備を外す";
                itemMenuButtonIns.GetComponent<Button>().onClick.AddListener(() => RemoveEquip(playerStatus, item));
            }
            else
            {
                itemMenuButtonIns.GetComponentInChildren<TextMeshProUGUI>().text = "装備する";
                itemMenuButtonIns.GetComponent<Button>().onClick.AddListener(() => Equip(playerStatus, item));
            }

            itemMenuButtonIns = Instantiate<GameObject>(_useItemPanelButtonPrefab, _useItemPanel.transform);
            itemMenuButtonIns.GetComponentInChildren<TextMeshProUGUI>().text = "捨てる";
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
            itemMenuButtonIns.GetComponentInChildren<TextMeshProUGUI>().text = "使う";
            itemMenuButtonIns.GetComponent<Button>().onClick.AddListener(() => UseItem(playerStatus, item));

            itemMenuButtonIns = Instantiate<GameObject>(_useItemPanelButtonPrefab, _useItemPanel.transform);
            itemMenuButtonIns.GetComponentInChildren<TextMeshProUGUI>().text = "捨てる";
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
            //　ItemPanelで最後にどれを選択していたか？
            _selectedGameObjectStack.Push(EventSystem.current.currentSelectedGameObject);

            _useItemPanel.transform.SetAsLastSibling();
            EventSystem.current.SetSelectedGameObject(_useItemPanel.transform.GetChild(0).gameObject);
            _useItemPanelCanvasGroup.interactable = true;
            Input.ResetInputAxes();

        }
    }

    //　アイテムを使う
    public void UseItem(PlayerStatus playerStatus, Item item)
    {
        _useItemInfoPanel.SetActive(true);

        if (item.ItemType == Item.Type.HPRecovery)
        {
            if (playerStatus.Hp == playerStatus.MaxHp)
            {
                _useItemInfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = playerStatus.CharacterName + "は元気です。";
            }
            else
            {
                playerStatus.Hp += item.ItemAmount;
                //　アイテムを使用した旨を表示
                _useItemInfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = "HPを" + item.ItemAmount + "回復しました。";
                //　持っているアイテム数を減らす
                playerStatus.ItemDictionary[item] -= 1;
            }
        }
        else if (item.ItemType == Item.Type.MPRecovery)
        {
            if (playerStatus.Mp == playerStatus.MaxMp)
            {
                _useItemInfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = playerStatus.CharacterName + "のMPは最大です。";
            }
            else
            {
                playerStatus.Mp += Mathf.FloorToInt(playerStatus.MaxHp * (item.ItemAmount * 0.01f));
                //　アイテムを使用した旨を表示
                _useItemInfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = "MPを" + item.ItemAmount + "回復しました。";
                //　持っているアイテム数を減らす
                playerStatus.ItemDictionary[item] -= 1;
            }
        }
        else if (item.ItemType == Item.Type.MotRecovery)
        {
            if (playerStatus.Mot == playerStatus.MaxMot)
            {
                _useItemInfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = playerStatus.CharacterName + "のやる気は最大です。";
            }
            else
            {
                playerStatus.Mot += item.ItemAmount;
                //　アイテムを使用した旨を表示
                _useItemInfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = "やる気を" + item.ItemAmount + "回復しました。";
                //　持っているアイテム数を減らす
                playerStatus.ItemDictionary[item] -= 1;
            }
        }
        else if (item.ItemType == Item.Type.PoisonRecovery)
        {
            if (playerStatus.IsPoisonStatus != true)
            {
                _useItemInfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = playerStatus.CharacterName + "は毒状態ではありません。";
            }
            else
            {
                _useItemInfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = playerStatus.CharacterName + "は毒から回復しました。";
                playerStatus.IsPoisonStatus = false;
                playerStatus.Hp -= Mathf.FloorToInt(playerStatus.MaxHp * 0.2f);
                //　持っているアイテム数を減らす
                playerStatus.ItemDictionary[item] -= 1;
            }
        }
        else if (item.ItemType == Item.Type.SleepRecovery)
        {
            if (playerStatus.IsSleepStatus != true)
            {
                _useItemInfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = playerStatus.CharacterName + "は昏睡状態ではありません。";
            }
            else
            {
                _useItemInfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = playerStatus.CharacterName + "は昏睡から回復しました。";
                playerStatus.IsSleepStatus = false;
                playerStatus.Hp -= Mathf.FloorToInt(playerStatus.MaxHp * 0.2f);
                //　持っているアイテム数を減らす
                playerStatus.ItemDictionary[item] -= 1;
            }
        }
        else if (item.ItemType == Item.Type.ParalysisRecovery)
        {
            if (playerStatus.IsParalysisStatus != true)
            {
                _useItemInfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = playerStatus.CharacterName + "は麻痺状態ではありません。";
            }
            else
            {
                _useItemInfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = playerStatus.CharacterName + "は麻痺から回復しました。";
                playerStatus.IsParalysisStatus = false;
                playerStatus.Hp -= Mathf.FloorToInt(playerStatus.MaxHp * 0.2f);
                //　持っているアイテム数を減らす
                playerStatus.ItemDictionary[item] -= 1;
            }
        }
        else if (item.ItemType == Item.Type.ConfusionRecovery)
        {
            if (playerStatus.IsConfusionStatus != true)
            {
                _useItemInfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = playerStatus.CharacterName + "は混乱状態ではありません。";
            }
            else
            {
                _useItemInfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = playerStatus.CharacterName + "は混乱から回復しました。";
                playerStatus.IsConfusionStatus = false;
                playerStatus.Hp -= Mathf.FloorToInt(playerStatus.MaxHp * 0.2f);
                //　持っているアイテム数を減らす
                playerStatus.ItemDictionary[item] -= 1;
            }
        }
        else if (item.ItemType == Item.Type.DepressionRecovery)
        {
            if (playerStatus.IsDepressionStatus != true)
            {
                _useItemInfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = playerStatus.CharacterName + "は鬱状態ではありません。";
            }
            else
            {
                _useItemInfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = playerStatus.CharacterName + "は鬱から回復しました。";
                playerStatus.IsDepressionStatus = false;
                playerStatus.Hp -= Mathf.FloorToInt(playerStatus.MaxHp * 0.5f);
                //　持っているアイテム数を減らす
                playerStatus.ItemDictionary[item] -= 1;
            }
        }
        else if (item.ItemType == Item.Type.Revival)
        {
            _useItemInfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = "このアイテムは使用できません。";
        }
        else if (item.ItemType == Item.Type.PowerUp)
        {
            _useItemInfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = "このアイテムは使用できません。";
        }
        else if (item.ItemType == Item.Type.DefenseUp)
        {
            _useItemInfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = "このアイテムは使用できません。";
        }

        //　itemPanleButtonListから該当するアイテムを探し数を更新する
        var itemButton = _itemPanelButtonList.Find(obj => obj.transform.Find("ItemName").GetComponent<TextMeshProUGUI>().text == item.ItemName);
        itemButton.transform.Find("Num").GetComponent<TextMeshProUGUI>().text = playerStatus.ItemDictionary[item].ToString();

        //　アイテム数が0だったらボタンとキャラクターステータスからアイテムを削除
        if (playerStatus.ItemDictionary[item] == 0)
        {
            //　アイテムが0になったら一気にItemPanelに戻す為、UseItemPanel内のオブジェクト登録を削除
            _selectedGameObjectStack.Pop();
            _selectedGameObjectStack.Pop();
            //　itemPanelButtonListからアイテムパネルボタンを削除
            _itemPanelButtonList.Remove(itemButton);
            //　アイテムパネルボタン自身の削除
            Destroy(itemButton);
            //　アイテムを渡したキャラクター自身のItemDictionaryからそのアイテムを削除
            playerStatus.ItemDictionary.Remove(item);
            //　ItemPanelに戻る為、UseItemPanel内に作ったボタンを全削除
            for (int i = _useItemPanel.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(_useItemPanel.transform.GetChild(i).gameObject);
            }
            //　アイテム数が0になったのでCommandMode.NoItemPassedに変更
            CurrentCommand = CommandMode.NoItemPassed;
            _useItemInfoPanel.transform.SetAsLastSibling();
            Input.ResetInputAxes();
        }
        else
        {
            //　アイテム数が残っている場合はUseItemPanelでアイテムをどうするかの選択に戻る
            CurrentCommand = CommandMode.UseItemToUseItem;
            _useItemInfoPanel.transform.SetAsLastSibling();
            Input.ResetInputAxes();
        }
    }

    //　アイテムを捨てる
    public void ThrowAwayItem(PlayerStatus playerStatus, Item item)
    {
        //　アイテム数を減らす
        playerStatus.ItemDictionary[item] -= 1;
        //　アイテム数が0になった時
        if (playerStatus.ItemDictionary[item] == 0)
        {

            //　装備している武器を捨てる場合の処理
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
        //　ItemPanelの子要素のアイテムパネルボタンから該当するアイテムのボタンを探して数を更新する
        var itemButton = _itemPanelButtonList.Find(obj => obj.transform.Find("ItemName").GetComponent<TextMeshProUGUI>().text == item.ItemName);
        itemButton.transform.Find("Num").GetComponent<TextMeshProUGUI>().text = playerStatus.ItemDictionary[item].ToString();
        _useItemInfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = item.ItemName + "を捨てました。";

        //　アイテム数が0だったらボタンとキャラクターステータスからアイテムを削除
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
            //　ItemPanelに戻る為UseItemPanelの子要素のボタンを全削除
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

    //　アイテムを装備する
    public void Equip(PlayerStatus playerStatus, Item item)
    {
        if (item.ItemType == Item.Type.Armor)
        {
            var equipArmorButton = _itemPanelButtonList.Find(itemPanelButton => itemPanelButton.transform.Find("ItemName").GetComponent<TextMeshProUGUI>().text == item.ItemName);
            equipArmorButton.transform.Find("Equip").GetComponent<TextMeshProUGUI>().text = "E";
            //　装備している鎧があればItemPanelでEquipのEを外す
            if (playerStatus.EquipArmor != null)
            {
                equipArmorButton = _itemPanelButtonList.Find(itemPanelButton => itemPanelButton.transform.Find("ItemName").GetComponent<TextMeshProUGUI>().text == playerStatus.EquipArmor.ItemName);
                equipArmorButton.transform.Find("Equip").GetComponent<TextMeshProUGUI>().text = "";
            }
            playerStatus.EquipArmor = item;
            _useItemInfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = item.ItemName + "を装備しました。";
        }
        else if (item.ItemType == Item.Type.Weapon)
        {
            var equipWeaponButton = _itemPanelButtonList.Find(itemPanelButton => itemPanelButton.transform.Find("ItemName").GetComponent<TextMeshProUGUI>().text == item.ItemName);
            equipWeaponButton.transform.Find("Equip").GetComponent<TextMeshProUGUI>().text = "E";
            //　装備している武器があればItemPanelでEquipのEを外す
            if (playerStatus.EquipWeapon != null)
            {
                equipWeaponButton = _itemPanelButtonList.Find(itemPanelButton => itemPanelButton.transform.Find("ItemName").GetComponent<TextMeshProUGUI>().text == playerStatus.EquipWeapon.ItemName);
                equipWeaponButton.transform.Find("Equip").GetComponent<TextMeshProUGUI>().text = "";
            }
            playerStatus.EquipWeapon = item;
            _useItemInfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = item.ItemName + "を装備しました。";
        }

        //　装備を切り替えたらItemPanelに戻る
        _useItemPanelCanvasGroup.interactable = false;
        _useItemPanel.SetActive(false);
        _itemPanelCanvasGroup.interactable = true;
        //　ItemPanelに戻るのでUseItemPanelの子要素に作ったボタンを全削除
        for (int i = _useItemPanel.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(_useItemPanel.transform.GetChild(i).gameObject);
        }

        _useItemInfoPanel.transform.SetAsLastSibling();
        _useItemInfoPanel.SetActive(true);

        CurrentCommand = CommandMode.UseItemToItem;

        Input.ResetInputAxes();
    }

    //　装備を外す
    public void RemoveEquip(PlayerStatus playerStatus, Item item)
    {
        //　アイテムの種類に応じて装備を外す
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
        //　装備を外した旨を表示
        _useItemInfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = item.ItemName + "を外しました。";
        //　装備を外したらItemPanelに戻る処理
        _useItemPanelCanvasGroup.interactable = false;
        _useItemPanel.SetActive(false);
        _itemPanelCanvasGroup.interactable = true;
        //　ItemPanelに戻るのでUseItemPanelの子要素のボタンを全削除
        for (int i = _useItemPanel.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(_useItemPanel.transform.GetChild(i).gameObject);
        }

        _useItemInfoPanel.transform.SetAsLastSibling();
        _useItemInfoPanel.SetActive(true);

        CurrentCommand = CommandMode.UseItemToItem;
        Input.ResetInputAxes();
    }

    //　辞書表示
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

            //言語を生成する位置
            Vector2 pos = new Vector2(600, startY - 60 * ((i - 1) + 1));
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
            TextMeshProUGUI origin = originReal.transform.Find("Origin").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI real = originReal.transform.Find("Real").GetComponent<TextMeshProUGUI>();
            //言語を表示
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
            //ヒントボタン生成
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

        //１０個を表示（それ以外は非表示）
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
        CurrentCommand = CommandMode.Translation;
        _dictionaryChoisePanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_transChoise1);

        //受け取ったインデックス番号に応じたリアル言語を表示
        string[] wordChoises = new string[3];
        wordChoises[0] = _talkManager.WordChoises[num].Word1;
        wordChoises[1] = _talkManager.WordChoises[num].Word2;
        wordChoises[2] = _talkManager.WordChoises[num].Word3;
        wordChoises = wordChoises.OrderBy(a => Guid.NewGuid()).ToArray();
        _transChoiseText1.text = wordChoises[0];
        _transChoiseText2.text = wordChoises[1];
        _transChoiseText3.text = wordChoises[2];
        //受け取ったインデックス番号を翻訳選択のCommandButtonに渡す
        _transChoise1.GetComponent<CommandButton>().SelectLangNum = num;
        _transChoise2.GetComponent<CommandButton>().SelectLangNum = num;
        _transChoise3.GetComponent<CommandButton>().SelectLangNum = num;
        //受け取った文字列を翻訳選択のCommandButtonに渡す
        _transChoise1.GetComponent<CommandButton>().SelectLang = word;
        _transChoise2.GetComponent<CommandButton>().SelectLang = word;
        _transChoise3.GetComponent<CommandButton>().SelectLang = word;
    }

    //　翻訳選択後、Canvasを更新
    public void UpdateDictionary()
    {
        _dictionaryCanvasGroup.interactable = true;
        CurrentCommand = CommandMode.Dictionary;
        _dictionaryChoisePanel.SetActive(false);
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
        CurrentCommand = CommandMode.Hint;
        _hintPanel.SetActive(true);

        //　ヒントを表示
        _hint1.sprite = _talkManager.HintLists[num].Hint1;
    }

    //　辞書のページ送り
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

    //　辞書のページ戻し
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

    //　ExitCommandの時にパネルも無効化しておく
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

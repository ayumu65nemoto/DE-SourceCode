using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Linq;

public class ShopMenu : MonoBehaviour
{
    public enum ShopMode
    {
        None,
        SelectAction,
        Buy,
        Sell
    }

    [SerializeField]
    private ShopMode _currentMode;
    //　行動選択パネル
    [SerializeField]
    private GameObject _selectActionPanel;
    //　アイテム表示パネル
    [SerializeField]
    private GameObject _itemPanel;
    //　アイテムパネルボタンを表示する場所
    [SerializeField]
    private GameObject _content;
    //　アイテムを買うまたは売る選択パネル
    [SerializeField]
    private GameObject _confirmPanel;
    //　情報表示パネル
    [SerializeField]
    private GameObject _shopInfoPanel;
    //　情報表示テキスト
    [SerializeField]
    private TextMeshProUGUI _infoText;

    //　行動選択パネルのCanvas Group
    private CanvasGroup _selectActionPanelCanvasGroup;
    //　アイテムパネルのCanvas Group
    private CanvasGroup _itemPanelCanvasGroup;
    //　アイテムを買うまたは売る選択パネルのCanvasGroup
    private CanvasGroup _confirmPanelCanvasGroup;

    //　アイテムボタンのプレハブ
    [SerializeField]
    private GameObject _shopItemPanelButtonPrefab = null;

    //　アイテムボタン一覧
    private List<GameObject> _shopItemPanelButtonList = new List<GameObject>();

    //　最後に選択していたゲームオブジェクトをスタック
    private Stack<GameObject> _selectedGameObjectStack = new Stack<GameObject>();

    //　PlayerController
    [SerializeField]
    private PlayerController _playerController;
    //　CameraController
    [SerializeField]
    private CameraController _cameraController;

    //　ショップアイテムデータ
    [SerializeField]
    private BuyItem _buyItem;
    //　プレイヤーデータ（所持アイテムデータに使う）
    [SerializeField]
    private PlayerStatus _playerStatus;
    //　現在選択しているアイテム
    private Item _selectedItem;
    //　現在選択しているアイテムの個数テキスト
    private TextMeshProUGUI _numText;
    //　売却予定のアイテム群
    private Dictionary<Item, int> _sellDictionary = new Dictionary<Item, int>();

    //　プレイヤーの所持金
    [SerializeField]
    private GameObject _playerMoneyPanel;
    [SerializeField]
    private TextMeshProUGUI _playerMoneyText;

    //　スクロールの境目
    [SerializeField]
    private int _scrollDownNum = 6;
    [SerializeField]
    private int _scrollUpNum = 7;
    private ScrollManager _scrollManager;

    public BuyItem BuyItem { get => _buyItem; set => _buyItem = value; }
    public Item SelectedItem { get => _selectedItem; set => _selectedItem = value; }
    public TextMeshProUGUI NumText { get => _numText; set => _numText = value; }

    // Start is called before the first frame update
    private void Awake()
    {
        //　CanvasGroup
        _selectActionPanelCanvasGroup = _selectActionPanel.GetComponent<CanvasGroup>();
        _itemPanelCanvasGroup = _itemPanel.GetComponent<CanvasGroup>();
        _confirmPanelCanvasGroup = _confirmPanel.GetComponent<CanvasGroup>();
        _scrollManager = _content.GetComponent<ScrollManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
        {
            if (_currentMode == ShopMode.SelectAction)
            {
                StartCoroutine(ExitShopMenu());
                _cameraController.IsActive = true;
            }
            else if (_currentMode == ShopMode.Buy || _currentMode == ShopMode.Sell)
            {
                _itemPanelCanvasGroup.interactable = false;
                _itemPanel.SetActive(false);
                _playerMoneyPanel.SetActive(false);
                _shopInfoPanel.SetActive(false);
                //　リストをクリア
                _shopItemPanelButtonList.Clear();
                for (int i = _content.transform.childCount - 1; i >= 0; i--)
                {
                    Destroy(_content.transform.GetChild(i).gameObject);
                }

                EventSystem.current.SetSelectedGameObject(_selectedGameObjectStack.Pop());
                _selectActionPanelCanvasGroup.interactable = true;
                _currentMode = ShopMode.SelectAction;
            }
        }

        //　選択解除された時（マウスでUI外をクリックした）は現在のモードによって無理やり選択させる
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            if (_currentMode == ShopMode.SelectAction)
            {
                EventSystem.current.SetSelectedGameObject(_selectActionPanel.transform.GetChild(0).gameObject);
            }
            else if (_currentMode == ShopMode.Buy || _currentMode == ShopMode.Sell)
            {
                EventSystem.current.SetSelectedGameObject(_content.transform.GetChild(0).gameObject);
                _scrollManager.Reset();
            }
        }

        //　購入個数を選択
        if (_currentMode == ShopMode.Buy)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (_buyItem.ShopItemDictionary[SelectedItem] <= 0)
                {
                    return;
                }
                _buyItem.ShopItemDictionary[SelectedItem]--;
                _numText.text = _buyItem.ShopItemDictionary[SelectedItem].ToString();
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                int myItemCount = 0;
                if (_playerStatus.ItemDictionary.ContainsKey(SelectedItem) == true)
                {
                    myItemCount = _playerStatus.ItemDictionary[SelectedItem];
                }
                else
                {
                    myItemCount = 0;
                }

                if (_buyItem.ShopItemDictionary[SelectedItem] >= 99 - myItemCount)
                {
                    return;
                }
                _buyItem.ShopItemDictionary[SelectedItem]++;
                _numText.text = _buyItem.ShopItemDictionary[SelectedItem].ToString();
            }
        }

        //　売却個数を選択
        if (_currentMode == ShopMode.Sell)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (_sellDictionary[SelectedItem] <= 0)
                {
                    return;
                }
                _sellDictionary[SelectedItem]--;
                _numText.text = _sellDictionary[SelectedItem].ToString();
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                if (_sellDictionary[SelectedItem] >= _playerStatus.ItemDictionary[SelectedItem])
                {
                    return;
                }
                _sellDictionary[SelectedItem]++;
                _numText.text = _sellDictionary[SelectedItem].ToString();
            }
        }
    }

    private void OnEnable()
    {
        //　現在のコマンドの初期化
        _currentMode = ShopMode.SelectAction;
        //　他のパネルは非表示
        _itemPanel.SetActive(false);
        _playerMoneyPanel.SetActive(false);
        _shopInfoPanel.SetActive(false);
        _confirmPanel.SetActive(false);

        _selectedGameObjectStack.Clear();
        _selectActionPanelCanvasGroup.interactable = true;
        EventSystem.current.SetSelectedGameObject(_selectActionPanel.transform.GetChild(0).gameObject);

        //　アイテムパネルボタンがあれば全て削除
        for (int i = _content.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(_content.transform.GetChild(i).gameObject);
        }

        _shopItemPanelButtonList.Clear();
        _itemPanelCanvasGroup.interactable = false;
        _confirmPanelCanvasGroup.interactable = false;
    }

    //　選択した項目で処理分け
    public void SelectAction(string action)
    {
        if (action == "Buy")
        {
            _currentMode = ShopMode.Buy;
            _cameraController.IsActive = false;
            _selectActionPanelCanvasGroup.interactable = false;
            _selectedGameObjectStack.Push(EventSystem.current.currentSelectedGameObject);

            ShowBuyItem(_buyItem, _playerStatus);
        }
        if (action == "Sell")
        {
            _currentMode = ShopMode.Sell;
            _cameraController.IsActive = false;
            _selectActionPanelCanvasGroup.interactable = false;
            _selectedGameObjectStack.Push(EventSystem.current.currentSelectedGameObject);

            ShowSellItem(_playerStatus);
        }
        if (action == "Cancel")
        {
            StartCoroutine(ExitShopMenu());
            _cameraController.IsActive = true;
        }
    }

    //　購入できるアイテム表示
    private void ShowBuyItem(BuyItem buyItem, PlayerStatus playerStatus)
    {
        _shopInfoPanel.SetActive(true);

        //　アイテム一覧のスクロール初期化
        _scrollManager.Reset();

        //　アイテムパネルボタンを何個作成したかどうか
        int shopItemButtonNum = 0;
        //　アイテムパネルボタン
        GameObject shopItemButtonIns;

        //　販売しているアイテム分のボタンの作成
        foreach (var shopItem in buyItem.ShopItemDictionary.Keys.ToList())
        {
            shopItemButtonIns = Instantiate<GameObject>(_shopItemPanelButtonPrefab, _content.transform);
            shopItemButtonIns.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = shopItem.ItemName;
            shopItemButtonIns.GetComponent<Button>().onClick.AddListener(() => Buy(buyItem, playerStatus));
            shopItemButtonIns.GetComponent<ShopItemPanelButtonScript>().Item = shopItem;

            //　指定した番号のアイテムパネルボタンにアイテムスクロール用スクリプトを取り付ける
            if (shopItemButtonNum != 0
                && (shopItemButtonNum % _scrollDownNum == 0)
                )
            {
                shopItemButtonIns.AddComponent<ScrollDownScript>();
            }
            else if (shopItemButtonNum != 0
              && (shopItemButtonNum % _scrollUpNum == 0)
              )
            {
                //　アイテムスクロールスクリプトの取り付けて設定値のセット
                shopItemButtonIns.AddComponent<ScrollUpScript>();
            }

            //　金額表示
            shopItemButtonIns.transform.Find("Price").GetComponent<TextMeshProUGUI>().text = shopItem.BuyPrice.ToString() + "G";

            //　カートアイテム数を表示
            buyItem.ShopItemDictionary[shopItem] = 0;
            var numText = shopItemButtonIns.transform.Find("Num").GetComponent<TextMeshProUGUI>();
            numText.text = buyItem.ShopItemDictionary[shopItem].ToString();

            //　所持個数表示
            var stockText = shopItemButtonIns.transform.Find("Stock").GetComponent<TextMeshProUGUI>();
            if (playerStatus.ItemDictionary.ContainsKey(shopItem) == true)
            {
                stockText.text = playerStatus.ItemDictionary[shopItem].ToString();
            }
            else
            {
                stockText.text = "0";
            }

            //　所持金表示
            _playerMoneyText.text = playerStatus.Money.ToString();

            //　アイテムボタンリストに追加
            _shopItemPanelButtonList.Add(shopItemButtonIns);
            //　アイテムパネルボタン番号を更新
            shopItemButtonNum++;

            if (shopItemButtonNum == _scrollDownNum + 1)
            {
                shopItemButtonNum = 1;
            }
        }

        //　アイテムパネルの表示と最初のアイテムの選択
        if (_content.transform.childCount != 0)
        {
            //　SelectCharacerPanelで最後にどのゲームオブジェクトを選択していたか
            _selectedGameObjectStack.Push(EventSystem.current.currentSelectedGameObject);
            _currentMode = ShopMode.Buy;
            _itemPanel.SetActive(true);
            _playerMoneyPanel.SetActive(true);
            _itemPanel.transform.SetAsLastSibling();
            _itemPanelCanvasGroup.interactable = true;
            EventSystem.current.SetSelectedGameObject(_content.transform.GetChild(0).gameObject);
        }
    }

    //　売却できるアイテム表示
    private void ShowSellItem(PlayerStatus playerStatus)
    {
        _shopInfoPanel.SetActive(true);

        //　アイテムパネルボタンを何個作成したかどうか
        int myItemButtonNum = 0;
        //　アイテムパネルボタン
        GameObject myItemButtonIns;

        //　所持しているアイテム分のボタンの作成
        foreach (var myItem in playerStatus.ItemDictionary.Keys)
        {
            //　装備しているアイテムはスルー
            if (myItem == playerStatus.EquipWeapon || myItem == playerStatus.EquipArmor)
            {
                continue;
            }

            myItemButtonIns = Instantiate<GameObject>(_shopItemPanelButtonPrefab, _content.transform);
            myItemButtonIns.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = myItem.ItemName;
            myItemButtonIns.GetComponent<Button>().onClick.AddListener(() => Sell(playerStatus));
            myItemButtonIns.GetComponent<ShopItemPanelButtonScript>().Item = myItem;

            //　指定した番号のアイテムパネルボタンにアイテムスクロール用スクリプトを取り付ける
            if (myItemButtonNum != 0
                && (myItemButtonNum % _scrollDownNum == 0)
                )
            {
                myItemButtonIns.AddComponent<ScrollDownScript>();
            }
            else if (myItemButtonNum != 0
              && (myItemButtonNum % _scrollUpNum == 0)
              )
            {
                //　アイテムスクロールスクリプトの取り付けて設定値のセット
                myItemButtonIns.AddComponent<ScrollUpScript>();
            }

            //　売却予定のアイテム群に所持アイテムをKeyとして追加
            _sellDictionary[myItem] = 0;

            //　金額表示
            myItemButtonIns.transform.Find("Price").GetComponent<TextMeshProUGUI>().text = myItem.SellPrice.ToString() + "G";

            //　カートアイテム数を表示
            var numText = myItemButtonIns.transform.Find("Num").GetComponent<TextMeshProUGUI>();
            numText.text = _sellDictionary[myItem].ToString();

            //　所持個数表示
            var stockText = myItemButtonIns.transform.Find("Stock").GetComponent<TextMeshProUGUI>();
            stockText.text = playerStatus.ItemDictionary[myItem].ToString();

            //　所持金表示
            _playerMoneyText.text = playerStatus.Money.ToString();

            //　アイテムボタンリストに追加
            _shopItemPanelButtonList.Add(myItemButtonIns);
            //　アイテムパネルボタン番号を更新
            myItemButtonNum++;

            if (myItemButtonNum == _scrollDownNum + 1)
            {
                myItemButtonNum = 1;
            }
        }

        //　アイテムパネルの表示と最初のアイテムの選択
        if (_content.transform.childCount != 0)
        {
            //　SelectCharacerPanelで最後にどのゲームオブジェクトを選択していたか
            _selectedGameObjectStack.Push(EventSystem.current.currentSelectedGameObject);
            _currentMode = ShopMode.Sell;
            _itemPanel.SetActive(true);
            _playerMoneyPanel.SetActive(true);
            _itemPanel.transform.SetAsLastSibling();
            _itemPanelCanvasGroup.interactable = true;
            EventSystem.current.SetSelectedGameObject(_content.transform.GetChild(0).gameObject);
        }
    }

    //　ショップから抜ける処理
    private IEnumerator ExitShopMenu()
    {
        EventSystem.current.SetSelectedGameObject(null);
        _currentMode = ShopMode.None;
        _shopInfoPanel.SetActive(true);
        _infoText.text = "また来てくれよな！";
        yield return new WaitForSeconds(1);
        _playerController.SetPlayerState(PlayerController.PlayerState.Normal);
        this.gameObject.SetActive(false);
    }

    //　アイテムを購入する処理
    private void Buy(BuyItem buyItem, PlayerStatus playerStatus)
    {
        //　アイテムをカートに入れていなければ処理しない
        if (buyItem.ShopItemDictionary.Values.Sum() == 0)
        {
            return;
        }

        //　合計金額
        int totalPrice = 0;
        foreach (var item in buyItem.ShopItemDictionary)
        {
            if (item.Value > 0)
            {
                totalPrice += (item.Key.BuyPrice * item.Value);
            }
        }

        //　お金が足りなければ処理しない
        if (totalPrice > playerStatus.Money)
        {
            _infoText.text = "お金が足りないみたいだね";
            return;
        }

        foreach (var shopItem in buyItem.ShopItemDictionary.ToList())
        {
            if (playerStatus.ItemDictionary.ContainsKey(shopItem.Key))
            {
                //　既に所持しているアイテムであれば数を増やす
                playerStatus.ItemDictionary[shopItem.Key] += shopItem.Value;
            }
            else
            {
                //　まだ所持していないアイテムであれば追加
                playerStatus.ItemDictionary[shopItem.Key] = shopItem.Value;
            }

            //　購入個数をリセット
            buyItem.ShopItemDictionary[shopItem.Key] = 0;
        }

        ResetShopUI(playerStatus);

        playerStatus.Money -= totalPrice;
        //　所持金表示
        _playerMoneyText.text = playerStatus.Money.ToString();
        _infoText.text = "毎度あり！";
    }

    private void Sell(PlayerStatus playerStatus)
    {
        //　アイテムを売却リストに入れていなければ処理しない
        if (_sellDictionary.Values.Sum() == 0)
        {
            return;
        }

        //　合計金額
        int totalPrice = 0;
        foreach (var item in _sellDictionary)
        {
            if (item.Value > 0)
            {
                totalPrice += (item.Key.SellPrice * item.Value);
            }
        }

        foreach (var myItem in _sellDictionary.ToList())
        {
            //　手持ちアイテムから減らす
            playerStatus.ItemDictionary[myItem.Key] -= myItem.Value;

            //　0個になったら
            if (playerStatus.ItemDictionary[myItem.Key] == 0)
            {
                _sellDictionary.Remove(myItem.Key);
                playerStatus.ItemDictionary.Remove(myItem.Key);
            }

            //　購入個数をリセット
            _sellDictionary[myItem.Key] = 0;
        }

        ResetShopUI(playerStatus);

        playerStatus.Money += totalPrice;
        //　所持金表示
        _playerMoneyText.text = playerStatus.Money.ToString();
        _infoText.text = "毎度あり！";
    }

    private void ResetShopUI(PlayerStatus playerStatus)
    {
        //　表示される個数もリセット
        foreach (var shopItemPanelButton in _shopItemPanelButtonList)
        {
            //　購入個数
            var numText = shopItemPanelButton.transform.Find("Num").GetComponent<TextMeshProUGUI>();
            numText.text = "0";

            //　所持個数
            var item = shopItemPanelButton.GetComponent<ShopItemPanelButtonScript>().Item;
            var stockText = shopItemPanelButton.transform.Find("Stock").GetComponent<TextMeshProUGUI>();
            if (playerStatus.ItemDictionary.ContainsKey(item) == true)
            {
                stockText.text = playerStatus.ItemDictionary[item].ToString();
            }
            else
            {
                Destroy(shopItemPanelButton);
            }
        }
    }
}

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
    //�@�s���I���p�l��
    [SerializeField]
    private GameObject _selectActionPanel;
    //�@�A�C�e���\���p�l��
    [SerializeField]
    private GameObject _itemPanel;
    //�@�A�C�e���p�l���{�^����\������ꏊ
    [SerializeField]
    private GameObject _content;
    //�@�A�C�e���𔃂��܂��͔���I���p�l��
    [SerializeField]
    private GameObject _confirmPanel;
    //�@���\���p�l��
    [SerializeField]
    private GameObject _shopInfoPanel;
    //�@���\���e�L�X�g
    [SerializeField]
    private TextMeshProUGUI _infoText;

    //�@�s���I���p�l����Canvas Group
    private CanvasGroup _selectActionPanelCanvasGroup;
    //�@�A�C�e���p�l����Canvas Group
    private CanvasGroup _itemPanelCanvasGroup;
    //�@�A�C�e���𔃂��܂��͔���I���p�l����CanvasGroup
    private CanvasGroup _confirmPanelCanvasGroup;

    //�@�A�C�e���{�^���̃v���n�u
    [SerializeField]
    private GameObject _shopItemPanelButtonPrefab = null;

    //�@�A�C�e���{�^���ꗗ
    private List<GameObject> _shopItemPanelButtonList = new List<GameObject>();

    //�@�Ō�ɑI�����Ă����Q�[���I�u�W�F�N�g���X�^�b�N
    private Stack<GameObject> _selectedGameObjectStack = new Stack<GameObject>();

    //�@PlayerController
    [SerializeField]
    private PlayerController _playerController;
    //�@CameraController
    [SerializeField]
    private CameraController _cameraController;

    //�@�V���b�v�A�C�e���f�[�^
    [SerializeField]
    private BuyItem _buyItem;
    //�@�v���C���[�f�[�^�i�����A�C�e���f�[�^�Ɏg���j
    [SerializeField]
    private PlayerStatus _playerStatus;
    //�@���ݑI�����Ă���A�C�e��
    private Item _selectedItem;
    //�@���ݑI�����Ă���A�C�e���̌��e�L�X�g
    private TextMeshProUGUI _numText;
    //�@���p�\��̃A�C�e���Q
    private Dictionary<Item, int> _sellDictionary = new Dictionary<Item, int>();

    //�@�v���C���[�̏�����
    [SerializeField]
    private GameObject _playerMoneyPanel;
    [SerializeField]
    private TextMeshProUGUI _playerMoneyText;

    //�@�X�N���[���̋���
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
        //�@CanvasGroup
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
                //�@���X�g���N���A
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

        //�@�I���������ꂽ���i�}�E�X��UI�O���N���b�N�����j�͌��݂̃��[�h�ɂ���Ė������I��������
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

        //�@�w������I��
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

        //�@���p����I��
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
        //�@���݂̃R�}���h�̏�����
        _currentMode = ShopMode.SelectAction;
        //�@���̃p�l���͔�\��
        _itemPanel.SetActive(false);
        _playerMoneyPanel.SetActive(false);
        _shopInfoPanel.SetActive(false);
        _confirmPanel.SetActive(false);

        _selectedGameObjectStack.Clear();
        _selectActionPanelCanvasGroup.interactable = true;
        EventSystem.current.SetSelectedGameObject(_selectActionPanel.transform.GetChild(0).gameObject);

        //�@�A�C�e���p�l���{�^��������ΑS�č폜
        for (int i = _content.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(_content.transform.GetChild(i).gameObject);
        }

        _shopItemPanelButtonList.Clear();
        _itemPanelCanvasGroup.interactable = false;
        _confirmPanelCanvasGroup.interactable = false;
    }

    //�@�I���������ڂŏ�������
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

    //�@�w���ł���A�C�e���\��
    private void ShowBuyItem(BuyItem buyItem, PlayerStatus playerStatus)
    {
        _shopInfoPanel.SetActive(true);

        //�@�A�C�e���ꗗ�̃X�N���[��������
        _scrollManager.Reset();

        //�@�A�C�e���p�l���{�^�������쐬�������ǂ���
        int shopItemButtonNum = 0;
        //�@�A�C�e���p�l���{�^��
        GameObject shopItemButtonIns;

        //�@�̔����Ă���A�C�e�����̃{�^���̍쐬
        foreach (var shopItem in buyItem.ShopItemDictionary.Keys.ToList())
        {
            shopItemButtonIns = Instantiate<GameObject>(_shopItemPanelButtonPrefab, _content.transform);
            shopItemButtonIns.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = shopItem.ItemName;
            shopItemButtonIns.GetComponent<Button>().onClick.AddListener(() => Buy(buyItem, playerStatus));
            shopItemButtonIns.GetComponent<ShopItemPanelButtonScript>().Item = shopItem;

            //�@�w�肵���ԍ��̃A�C�e���p�l���{�^���ɃA�C�e���X�N���[���p�X�N���v�g�����t����
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
                //�@�A�C�e���X�N���[���X�N���v�g�̎��t���Đݒ�l�̃Z�b�g
                shopItemButtonIns.AddComponent<ScrollUpScript>();
            }

            //�@���z�\��
            shopItemButtonIns.transform.Find("Price").GetComponent<TextMeshProUGUI>().text = shopItem.BuyPrice.ToString() + "G";

            //�@�J�[�g�A�C�e������\��
            buyItem.ShopItemDictionary[shopItem] = 0;
            var numText = shopItemButtonIns.transform.Find("Num").GetComponent<TextMeshProUGUI>();
            numText.text = buyItem.ShopItemDictionary[shopItem].ToString();

            //�@�������\��
            var stockText = shopItemButtonIns.transform.Find("Stock").GetComponent<TextMeshProUGUI>();
            if (playerStatus.ItemDictionary.ContainsKey(shopItem) == true)
            {
                stockText.text = playerStatus.ItemDictionary[shopItem].ToString();
            }
            else
            {
                stockText.text = "0";
            }

            //�@�������\��
            _playerMoneyText.text = playerStatus.Money.ToString();

            //�@�A�C�e���{�^�����X�g�ɒǉ�
            _shopItemPanelButtonList.Add(shopItemButtonIns);
            //�@�A�C�e���p�l���{�^���ԍ����X�V
            shopItemButtonNum++;

            if (shopItemButtonNum == _scrollDownNum + 1)
            {
                shopItemButtonNum = 1;
            }
        }

        //�@�A�C�e���p�l���̕\���ƍŏ��̃A�C�e���̑I��
        if (_content.transform.childCount != 0)
        {
            //�@SelectCharacerPanel�ōŌ�ɂǂ̃Q�[���I�u�W�F�N�g��I�����Ă�����
            _selectedGameObjectStack.Push(EventSystem.current.currentSelectedGameObject);
            _currentMode = ShopMode.Buy;
            _itemPanel.SetActive(true);
            _playerMoneyPanel.SetActive(true);
            _itemPanel.transform.SetAsLastSibling();
            _itemPanelCanvasGroup.interactable = true;
            EventSystem.current.SetSelectedGameObject(_content.transform.GetChild(0).gameObject);
        }
    }

    //�@���p�ł���A�C�e���\��
    private void ShowSellItem(PlayerStatus playerStatus)
    {
        _shopInfoPanel.SetActive(true);

        //�@�A�C�e���p�l���{�^�������쐬�������ǂ���
        int myItemButtonNum = 0;
        //�@�A�C�e���p�l���{�^��
        GameObject myItemButtonIns;

        //�@�������Ă���A�C�e�����̃{�^���̍쐬
        foreach (var myItem in playerStatus.ItemDictionary.Keys)
        {
            //�@�������Ă���A�C�e���̓X���[
            if (myItem == playerStatus.EquipWeapon || myItem == playerStatus.EquipArmor)
            {
                continue;
            }

            myItemButtonIns = Instantiate<GameObject>(_shopItemPanelButtonPrefab, _content.transform);
            myItemButtonIns.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = myItem.ItemName;
            myItemButtonIns.GetComponent<Button>().onClick.AddListener(() => Sell(playerStatus));
            myItemButtonIns.GetComponent<ShopItemPanelButtonScript>().Item = myItem;

            //�@�w�肵���ԍ��̃A�C�e���p�l���{�^���ɃA�C�e���X�N���[���p�X�N���v�g�����t����
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
                //�@�A�C�e���X�N���[���X�N���v�g�̎��t���Đݒ�l�̃Z�b�g
                myItemButtonIns.AddComponent<ScrollUpScript>();
            }

            //�@���p�\��̃A�C�e���Q�ɏ����A�C�e����Key�Ƃ��Ēǉ�
            _sellDictionary[myItem] = 0;

            //�@���z�\��
            myItemButtonIns.transform.Find("Price").GetComponent<TextMeshProUGUI>().text = myItem.SellPrice.ToString() + "G";

            //�@�J�[�g�A�C�e������\��
            var numText = myItemButtonIns.transform.Find("Num").GetComponent<TextMeshProUGUI>();
            numText.text = _sellDictionary[myItem].ToString();

            //�@�������\��
            var stockText = myItemButtonIns.transform.Find("Stock").GetComponent<TextMeshProUGUI>();
            stockText.text = playerStatus.ItemDictionary[myItem].ToString();

            //�@�������\��
            _playerMoneyText.text = playerStatus.Money.ToString();

            //�@�A�C�e���{�^�����X�g�ɒǉ�
            _shopItemPanelButtonList.Add(myItemButtonIns);
            //�@�A�C�e���p�l���{�^���ԍ����X�V
            myItemButtonNum++;

            if (myItemButtonNum == _scrollDownNum + 1)
            {
                myItemButtonNum = 1;
            }
        }

        //�@�A�C�e���p�l���̕\���ƍŏ��̃A�C�e���̑I��
        if (_content.transform.childCount != 0)
        {
            //�@SelectCharacerPanel�ōŌ�ɂǂ̃Q�[���I�u�W�F�N�g��I�����Ă�����
            _selectedGameObjectStack.Push(EventSystem.current.currentSelectedGameObject);
            _currentMode = ShopMode.Sell;
            _itemPanel.SetActive(true);
            _playerMoneyPanel.SetActive(true);
            _itemPanel.transform.SetAsLastSibling();
            _itemPanelCanvasGroup.interactable = true;
            EventSystem.current.SetSelectedGameObject(_content.transform.GetChild(0).gameObject);
        }
    }

    //�@�V���b�v���甲���鏈��
    private IEnumerator ExitShopMenu()
    {
        EventSystem.current.SetSelectedGameObject(null);
        _currentMode = ShopMode.None;
        _shopInfoPanel.SetActive(true);
        _infoText.text = "�܂����Ă����ȁI";
        yield return new WaitForSeconds(1);
        _playerController.SetPlayerState(PlayerController.PlayerState.Normal);
        this.gameObject.SetActive(false);
    }

    //�@�A�C�e�����w�����鏈��
    private void Buy(BuyItem buyItem, PlayerStatus playerStatus)
    {
        //�@�A�C�e�����J�[�g�ɓ���Ă��Ȃ���Ώ������Ȃ�
        if (buyItem.ShopItemDictionary.Values.Sum() == 0)
        {
            return;
        }

        //�@���v���z
        int totalPrice = 0;
        foreach (var item in buyItem.ShopItemDictionary)
        {
            if (item.Value > 0)
            {
                totalPrice += (item.Key.BuyPrice * item.Value);
            }
        }

        //�@����������Ȃ���Ώ������Ȃ�
        if (totalPrice > playerStatus.Money)
        {
            _infoText.text = "����������Ȃ��݂�������";
            return;
        }

        foreach (var shopItem in buyItem.ShopItemDictionary.ToList())
        {
            if (playerStatus.ItemDictionary.ContainsKey(shopItem.Key))
            {
                //�@���ɏ������Ă���A�C�e���ł���ΐ��𑝂₷
                playerStatus.ItemDictionary[shopItem.Key] += shopItem.Value;
            }
            else
            {
                //�@�܂��������Ă��Ȃ��A�C�e���ł���Βǉ�
                playerStatus.ItemDictionary[shopItem.Key] = shopItem.Value;
            }

            //�@�w���������Z�b�g
            buyItem.ShopItemDictionary[shopItem.Key] = 0;
        }

        ResetShopUI(playerStatus);

        playerStatus.Money -= totalPrice;
        //�@�������\��
        _playerMoneyText.text = playerStatus.Money.ToString();
        _infoText.text = "���x����I";
    }

    private void Sell(PlayerStatus playerStatus)
    {
        //�@�A�C�e���𔄋p���X�g�ɓ���Ă��Ȃ���Ώ������Ȃ�
        if (_sellDictionary.Values.Sum() == 0)
        {
            return;
        }

        //�@���v���z
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
            //�@�莝���A�C�e�����猸�炷
            playerStatus.ItemDictionary[myItem.Key] -= myItem.Value;

            //�@0�ɂȂ�����
            if (playerStatus.ItemDictionary[myItem.Key] == 0)
            {
                _sellDictionary.Remove(myItem.Key);
                playerStatus.ItemDictionary.Remove(myItem.Key);
            }

            //�@�w���������Z�b�g
            _sellDictionary[myItem.Key] = 0;
        }

        ResetShopUI(playerStatus);

        playerStatus.Money += totalPrice;
        //�@�������\��
        _playerMoneyText.text = playerStatus.Money.ToString();
        _infoText.text = "���x����I";
    }

    private void ResetShopUI(PlayerStatus playerStatus)
    {
        //�@�\�������������Z�b�g
        foreach (var shopItemPanelButton in _shopItemPanelButtonList)
        {
            //�@�w����
            var numText = shopItemPanelButton.transform.Find("Num").GetComponent<TextMeshProUGUI>();
            numText.text = "0";

            //�@������
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

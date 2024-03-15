using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ShopItemPanelButtonScript : MonoBehaviour, ISelectHandler
{
    private Item _item;
    private ShopMenu _shopMenu;
    //�@���e�L�X�g
    [SerializeField]
    private TextMeshProUGUI _num;
    //�@�A�C�e�����\���e�L�X�g
    private TextMeshProUGUI _shopItemInfoText;

    public Item Item { get => _item; set => _item = value; }

    private void Awake()
    {
        _shopMenu = transform.root.GetComponent<ShopMenu>();
        _shopItemInfoText = transform.root.Find("ShopInfoPanel/InfoText").GetComponent<TextMeshProUGUI>();
    }

    //�@�{�^�����I�����ꂽ���Ɏ��s
    public void OnSelect(BaseEventData eventData)
    {
        ShowItemInformation();
        _shopMenu.SelectedItem = Item;
        _shopMenu.NumText = _num;
    }

    //�@�A�C�e�����̕\��
    public void ShowItemInformation()
    {
        _shopItemInfoText.text = Item.ItemInfo;
    }
}

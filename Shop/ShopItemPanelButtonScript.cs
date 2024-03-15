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
    //　個数テキスト
    [SerializeField]
    private TextMeshProUGUI _num;
    //　アイテム情報表示テキスト
    private TextMeshProUGUI _shopItemInfoText;

    public Item Item { get => _item; set => _item = value; }

    private void Awake()
    {
        _shopMenu = transform.root.GetComponent<ShopMenu>();
        _shopItemInfoText = transform.root.Find("ShopInfoPanel/InfoText").GetComponent<TextMeshProUGUI>();
    }

    //　ボタンが選択された時に実行
    public void OnSelect(BaseEventData eventData)
    {
        ShowItemInformation();
        _shopMenu.SelectedItem = Item;
        _shopMenu.NumText = _num;
    }

    //　アイテム情報の表示
    public void ShowItemInformation()
    {
        _shopItemInfoText.text = Item.ItemInfo;
    }
}

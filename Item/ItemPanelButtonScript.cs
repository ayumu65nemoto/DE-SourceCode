using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ItemPanelButtonScript : MonoBehaviour, ISelectHandler
{
    private Item _item;
    //　アイテムタイトル表示テキスト
    private TextMeshProUGUI _itemTitleText;
    //　アイテム情報表示テキスト
    private TextMeshProUGUI _itemInformationText;

    private void Awake()
    {
        _itemTitleText = transform.root.Find("ItemInfoPanel/Title").GetComponent<TextMeshProUGUI>();
        _itemInformationText = transform.root.Find("ItemInfoPanel/Info").GetComponent<TextMeshProUGUI>();
    }

    //　ボタンが選択された時に実行
    public void OnSelect(BaseEventData eventData)
    {
        ShowItemInformation();
    }

    //　アイテム情報の表示
    public void ShowItemInformation()
    {
        _itemTitleText.text = _item.ItemName;
        _itemInformationText.text = _item.ItemInfo;
    }
    //　データをセットする
    public void SetParam(Item item)
    {
        this._item = item;
    }
}

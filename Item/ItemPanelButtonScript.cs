using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ItemPanelButtonScript : MonoBehaviour, ISelectHandler
{
    private Item _item;
    //�@�A�C�e���^�C�g���\���e�L�X�g
    private TextMeshProUGUI _itemTitleText;
    //�@�A�C�e�����\���e�L�X�g
    private TextMeshProUGUI _itemInformationText;

    private void Awake()
    {
        _itemTitleText = transform.root.Find("ItemInfoPanel/Title").GetComponent<TextMeshProUGUI>();
        _itemInformationText = transform.root.Find("ItemInfoPanel/Info").GetComponent<TextMeshProUGUI>();
    }

    //�@�{�^�����I�����ꂽ���Ɏ��s
    public void OnSelect(BaseEventData eventData)
    {
        ShowItemInformation();
    }

    //�@�A�C�e�����̕\��
    public void ShowItemInformation()
    {
        _itemTitleText.text = _item.ItemName;
        _itemInformationText.text = _item.ItemInfo;
    }
    //�@�f�[�^���Z�b�g����
    public void SetParam(Item item)
    {
        this._item = item;
    }
}

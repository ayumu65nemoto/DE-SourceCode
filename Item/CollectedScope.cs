using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollectedScope : MonoBehaviour
{
    // 獲得アイテム
    [SerializeField]
    private Item _getItem;
    // 獲得数
    [SerializeField]
    private int _getAmount;
    // QuestManager
    private QuestManager _questManager;
    // 取得イベントUI
    [SerializeField]
    private GameObject _eventUI;
    [SerializeField]
    private TextMeshProUGUI _eventText;

    private void Start()
    {
        _questManager = QuestManager.questManager;
    }

    void OnTriggerStay(Collider col)
    {
        if (col.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                PlayerStatus playerStatus = col.GetComponent<PlayerController>().PlayerStatus;
                if (playerStatus.ItemDictionary.ContainsKey(_getItem))
                {
                    playerStatus.ItemDictionary[_getItem]++;
                }
                else
                {
                    playerStatus.ItemDictionary.Add(_getItem, 1);
                }

                // 特定のアイテムだった場合はクエストクリアフラグを立てる
                if (_getItem.ItemName == "AlphaFhase1")
                {
                    _questManager.QuestList[0].IsClear = true;
                }

                // イベントUI表示
                _eventText.text = _getItem.ItemName + "を拾いました";
                _eventUI.SetActive(true);
                _eventUI.GetComponent<EventUI>().StartActiveFalse();
                _questManager.ShowQuest("依頼主に報告しよう");

                transform.parent.gameObject.SetActive(false);
            }
        }
    }
}

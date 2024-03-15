using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollectedScope : MonoBehaviour
{
    // �l���A�C�e��
    [SerializeField]
    private Item _getItem;
    // �l����
    [SerializeField]
    private int _getAmount;
    // QuestManager
    private QuestManager _questManager;
    // �擾�C�x���gUI
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

                // ����̃A�C�e���������ꍇ�̓N�G�X�g�N���A�t���O�𗧂Ă�
                if (_getItem.ItemName == "AlphaFhase1")
                {
                    _questManager.QuestList[0].IsClear = true;
                }

                // �C�x���gUI�\��
                _eventText.text = _getItem.ItemName + "���E���܂���";
                _eventUI.SetActive(true);
                _eventUI.GetComponent<EventUI>().StartActiveFalse();
                _questManager.ShowQuest("�˗���ɕ񍐂��悤");

                transform.parent.gameObject.SetActive(false);
            }
        }
    }
}

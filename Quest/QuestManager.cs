using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
    public static QuestManager questManager;
    // �N�G�X�g�f�[�^��ێ����Ă���
    [SerializeField]
    private List<Quest> _questList = new List<Quest>();
    // �������Ă���N�G�X�g
    [SerializeField]
    private List<Quest> _happendQuestList = new List<Quest>();
    // �N�G�X�g�{�[�h
    [SerializeField]
    private GameObject _questPanel;
    // �N�G�X�g�^�C�g��
    [SerializeField]
    private TextMeshProUGUI _questTitle;
    // �N�G�X�g����
    [SerializeField]
    private TextMeshProUGUI _questInfo;

    public List<Quest> QuestList { get => _questList; set => _questList = value; }
    public List<Quest> HappendQuestList { get => _happendQuestList; set => _happendQuestList = value; }

    private void Awake()
    {
        if (questManager == null)
        {
            questManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Tab))
        //{
        //    if (_questPanel.activeSelf)
        //    {
        //        _questPanel.SetActive(false);
        //    }
        //    else
        //    {
        //        _questPanel.SetActive(true);
        //    }
        //}

        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    _questTitle.text = "�N�G�X�g���Z�b�g";
        //    _questInfo.text = "�N�G�X�g���Z�b�g";
        //}
    }

    public void ShowQuest(string info = null)
    {
        if (_happendQuestList.Count == 0)
        {
            _questTitle.text = "�N���A";
            _questInfo.text = "�N���A";
        }
        else
        {
            _questTitle.text = _happendQuestList[0].Name;
            if (info == null)
            {
                _questInfo.text = _happendQuestList[0].Info;
            }
            else
            {
                _questInfo.text = info;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
    public static QuestManager questManager;
    // クエストデータを保持しておく
    [SerializeField]
    private List<Quest> _questList = new List<Quest>();
    // 発生しているクエスト
    [SerializeField]
    private List<Quest> _happendQuestList = new List<Quest>();
    // クエストボード
    [SerializeField]
    private GameObject _questPanel;
    // クエストタイトル
    [SerializeField]
    private TextMeshProUGUI _questTitle;
    // クエスト説明
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
        //    _questTitle.text = "クエストリセット";
        //    _questInfo.text = "クエストリセット";
        //}
    }

    public void ShowQuest(string info = null)
    {
        if (_happendQuestList.Count == 0)
        {
            _questTitle.text = "クリア";
            _questInfo.text = "クリア";
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

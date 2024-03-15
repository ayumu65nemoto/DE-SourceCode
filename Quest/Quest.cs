using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Quest", menuName = "CreateQuest")]
public class Quest : ScriptableObject
{
    public enum QuestType
    {
        Main,
        Sub
    }

    // クエストの種類
    [SerializeField]
    private QuestType questType;
    // クエスト発生フラグ
    [SerializeField]
    private bool _isHappen = false;
    // クエストクリアフラグ
    [SerializeField]
    private bool _isClear = false;
    // クエストの名前
    [SerializeField]
    private string _name;
    // クエストの説明
    [SerializeField]
    private string _info;
    // 報酬
    [SerializeField]
    private List<Item> _rewardItem = new List<Item>();
    [SerializeField]
    private List<int> _rewardHint = new List<int>();

    public QuestType QuestType1 { get => questType; set => questType = value; }
    public bool IsHappen { get => _isHappen; set => _isHappen = value; }
    public bool IsClear { get => _isClear; set => _isClear = value; }
    public string Name { get => _name; set => _name = value; }
    public string Info { get => _info; set => _info = value; }
    public List<Item> RewardItem { get => _rewardItem; set => _rewardItem = value; }
    public List<int> RewardHint { get => _rewardHint; set => _rewardHint = value; }
}

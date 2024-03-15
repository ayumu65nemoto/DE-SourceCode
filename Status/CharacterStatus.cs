using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public abstract class CharacterStatus : ScriptableObject
{
    //　キャラクターの名前
    [SerializeField]
    private string _characterName = "";
    //　キャラクターのレベル
    [SerializeField]
    private int _level = 1;
    //　最大HP
    [SerializeField]
    private int _maxHp = 100;
    //　HP
    [SerializeField]
    private int _hp = 100;
    //　最大MP
    [SerializeField]
    private int _maxMp = 50;
    //　MP
    [SerializeField]
    private int _mp = 50;
    //　最大やる気
    [SerializeField]
    private int _maxMot = 100;
    //　やる気
    [SerializeField]
    private int _mot = 100;
    //　力
    [SerializeField]
    private int _power = 10;
    //　魔法攻撃力
    //[SerializeField]
    //private int _magicPower = 10;
    //　防御力
    [SerializeField]
    private int _defense = 10;
    //　持っているスキル
    [SerializeField]
    private List<Skill> _skillList = null;

    //　状態異常
    //　毒
    [SerializeField]
    private bool _isPoisonStatus;
    //　昏睡
    [SerializeField]
    private bool _isSleepStatus;
    //　麻痺
    [SerializeField]
    private bool _isParalysisStatus;
    //　混乱
    [SerializeField]
    private bool _isConfusionStatus;
    //　鬱
    [SerializeField]
    private bool _isDepressionStatus;

    public string CharacterName { get => _characterName; set => _characterName = value; }
    public int Level { get => _level; set => _level = value; }
    public int MaxHp { get => _maxHp; set => _maxHp = value; }
    //public int Hp { get => _hp; set => _hp = value; }
    public int Hp
    {
        get { return _hp; }
        set { _hp = Mathf.Clamp(value, 0, MaxHp); }
    }
    public int MaxMp { get => _maxMp; set => _maxMp = value; }
    public int Mp { get => _mp; set => _mp = value; }
    public int Power { get => _power; set => _power = value; }
    public int Defense { get => _defense; set => _defense = value; }
    public int Mot { get => _mot; set => _mot = value; }
    public int MaxMot { get => _maxMot; set => _maxMot = value; }
    public bool IsPoisonStatus { get => _isPoisonStatus; set => _isPoisonStatus = value; }
    public bool IsSleepStatus { get => _isSleepStatus; set => _isSleepStatus = value; }
    public bool IsParalysisStatus { get => _isParalysisStatus; set => _isParalysisStatus = value; }
    public bool IsConfusionStatus { get => _isConfusionStatus; set => _isConfusionStatus = value; }
    public bool IsDepressionStatus { get => _isDepressionStatus; set => _isDepressionStatus = value; }
    //public int MagicPower { get => _magicPower; set => _magicPower = value; }
    public List<Skill> SkillList { get => _skillList; set => _skillList = value; }
}

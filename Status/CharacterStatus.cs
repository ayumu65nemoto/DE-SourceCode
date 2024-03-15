using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public abstract class CharacterStatus : ScriptableObject
{
    //�@�L�����N�^�[�̖��O
    [SerializeField]
    private string _characterName = "";
    //�@�L�����N�^�[�̃��x��
    [SerializeField]
    private int _level = 1;
    //�@�ő�HP
    [SerializeField]
    private int _maxHp = 100;
    //�@HP
    [SerializeField]
    private int _hp = 100;
    //�@�ő�MP
    [SerializeField]
    private int _maxMp = 50;
    //�@MP
    [SerializeField]
    private int _mp = 50;
    //�@�ő���C
    [SerializeField]
    private int _maxMot = 100;
    //�@���C
    [SerializeField]
    private int _mot = 100;
    //�@��
    [SerializeField]
    private int _power = 10;
    //�@���@�U����
    //[SerializeField]
    //private int _magicPower = 10;
    //�@�h���
    [SerializeField]
    private int _defense = 10;
    //�@�����Ă���X�L��
    [SerializeField]
    private List<Skill> _skillList = null;

    //�@��Ԉُ�
    //�@��
    [SerializeField]
    private bool _isPoisonStatus;
    //�@����
    [SerializeField]
    private bool _isSleepStatus;
    //�@���
    [SerializeField]
    private bool _isParalysisStatus;
    //�@����
    [SerializeField]
    private bool _isConfusionStatus;
    //�@�T
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

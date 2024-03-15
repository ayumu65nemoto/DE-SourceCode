using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
[CreateAssetMenu(fileName = "LevelUpData", menuName = "CreateLevelUpData")]
public class LevelUpData : ScriptableObject
{
    //�@���x���A�b�v�ɕK�v�ȃg�[�^���o���l
    [SerializeField]
    private LevelUpDictionary _requiredExperience = null;
    //�@MaxHp���オ�闦
    [SerializeField]
    private float _probabilityToIncreaseMaxHP = 100f;
    //�@MaxMP���オ�闦
    [SerializeField]
    private float _probabliityToIncreaseMaxMP = 100f;
    //�@�͂��オ�闦
    [SerializeField]
    private float _probabilityToIncreasePower = 100f;
    //�@�ł��ꋭ�����オ�闦
    [SerializeField]
    private float _probabilityToIncreaseDefense = 100f;

    //�@MaxHP���オ�������̍Œ�l
    [SerializeField]
    private int _minHPRisingLimit = 1;
    //�@MaxMP���オ�������̍Œ�l
    [SerializeField]
    private int _minMPRisingLimit = 1;
    //�@�͂��オ�������̍Œ�l
    [SerializeField]
    private int _minPowerRisingLimit = 1;
    //�@�ł��ꋭ�����オ�������̍Œ�l
    [SerializeField]
    private int _minDefenseRisingLimit = 1;

    //�@MaxHP���オ�������̍ō��l
    [SerializeField]
    private int _maxHPRisingLimit = 50;
    //�@MaxMP���オ�������̍ō��l
    [SerializeField]
    private int _maxMPRisingLimit = 50;
    //�@�͂��オ�������̍ō��l
    [SerializeField]
    private int _maxPowerRisingLimit = 2;
    //�@�ł��ꋭ�����オ�������̍ō��l
    [SerializeField]
    private int _maxDefenseRisingLimit = 2;

    public LevelUpDictionary RequiredExperience { get => _requiredExperience; set => _requiredExperience = value; }
    public float ProbabilityToIncreaseMaxHP { get => _probabilityToIncreaseMaxHP; set => _probabilityToIncreaseMaxHP = value; }
    public float ProbabliityToIncreaseMaxMP { get => _probabliityToIncreaseMaxMP; set => _probabliityToIncreaseMaxMP = value; }
    public float ProbabilityToIncreasePower { get => _probabilityToIncreasePower; set => _probabilityToIncreasePower = value; }
    public float ProbabilityToIncreaseDefense { get => _probabilityToIncreaseDefense; set => _probabilityToIncreaseDefense = value; }

    public int MinHPRisingLimit { get => _minHPRisingLimit; set => _minHPRisingLimit = value; }
    public int MinMPRisingLimit { get => _minMPRisingLimit; set => _minMPRisingLimit = value; }
    public int MinPowerRisingLimit { get => _minPowerRisingLimit; set => _minPowerRisingLimit = value; }
    public int MinDefenseRisingLimit { get => _minDefenseRisingLimit; set => _minDefenseRisingLimit = value; }

    public int MaxHPRisingLimit { get => _maxHPRisingLimit; set => _maxHPRisingLimit = value; }
    public int MaxMPRisingLimit { get => _maxMPRisingLimit; set => _maxMPRisingLimit = value; }
    public int MaxPowerRisingLimit { get => _maxPowerRisingLimit; set => _maxPowerRisingLimit = value; }
    public int MaxDefenseRisingLimit { get => _maxDefenseRisingLimit; set => _maxDefenseRisingLimit = value; }

    //�@���̃��x���ɕK�v�Ȍo���l
    public int GetRequiredExperience(int level)
    {
        return _requiredExperience.Keys.Contains(level) ? _requiredExperience[level] : int.MaxValue;
    }
}

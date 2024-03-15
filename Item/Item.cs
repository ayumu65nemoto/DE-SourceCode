using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Item", menuName = "CreateItem")]
public class Item : ScriptableObject
{
    public enum Type
    {
        HPRecovery,
        MPRecovery,
        MotRecovery,
        PoisonRecovery,
        SleepRecovery,
        ParalysisRecovery,
        ConfusionRecovery,
        DepressionRecovery,
        Revival,
        PowerUp,
        DefenseUp,
        Weapon,
        Armor,
        Valuables
    }

    //�@�A�C�e���̎��
    [SerializeField]
    private Type _itemType = Type.HPRecovery;
    //�@�A�C�e����
    [SerializeField]
    private string _itemName = "";
    //�@�A�C�e�����
    [SerializeField]
    private string _itemInfo = "";
    //�@�A�C�e���̃p�����[�^
    [SerializeField]
    private int _itemAmount = 0;
    //�@�����i�̍U���㏸�l
    [SerializeField]
    private float _moreAtk = 1;
    //�@�����i�̖h��㏸�l
    [SerializeField]
    private float _moreDef = 1;
    //�@�����i�̃_���[�W�㏸�l
    [SerializeField]
    private float _moreDmg = 1;
    //�@�w�����z
    [SerializeField]
    private int _buyPrice = 0;
    //�@���p���z
    [SerializeField]
    private int _sellPrice = 0;

    public Type ItemType { get => _itemType; set => _itemType = value; }
    public string ItemName { get => _itemName; set => _itemName = value; }
    public string ItemInfo { get => _itemInfo; set => _itemInfo = value; }
    public int ItemAmount { get => _itemAmount; set => _itemAmount = value; }
    public float MoreAtk { get => _moreAtk; set => _moreAtk = value; }
    public float MoreDef { get => _moreDef; set => _moreDef = value; }
    public float MoreDmg { get => _moreDmg; set => _moreDmg = value; }
    public int BuyPrice { get => _buyPrice; set => _buyPrice = value; }
    public int SellPrice { get => _sellPrice; set => _sellPrice = value; }
}

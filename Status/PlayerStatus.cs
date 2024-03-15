using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "PlayerStatus", menuName = "CreatePlayerStatus")]
public class PlayerStatus : CharacterStatus
{
    //�@�l���o���l
    [SerializeField]
    private int _earnedExperience = 0;
    //�@������
    [SerializeField]
    private int _money = 0;
    //�@�������Ă��镐��
    [SerializeField]
    private Item _equipWeapon = null;
    //�@�������Ă���Z
    [SerializeField]
    private Item _equipArmor = null;
    //�@�A�C�e���ƌ���Dictionary
    [SerializeField]
    private ItemDictionary _itemDictionary = null;
    //�@���x���A�b�v�f�[�^
    [SerializeField]
    private LevelUpData levelUpData = null;

    public int EarnedExperience { get => _earnedExperience; set => _earnedExperience = value; }
    public Item EquipWeapon { get => _equipWeapon; set => _equipWeapon = value; }
    public Item EquipArmor { get => _equipArmor; set => _equipArmor = value; }
    public ItemDictionary ItemDictionary { get => _itemDictionary; set => _itemDictionary = value; }
    public LevelUpData LevelUpData { get => levelUpData; set => levelUpData = value; }
    public int Money { get => _money; set => _money = value; }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "PlayerStatus", menuName = "CreatePlayerStatus")]
public class PlayerStatus : CharacterStatus
{
    //　獲得経験値
    [SerializeField]
    private int _earnedExperience = 0;
    //　所持金
    [SerializeField]
    private int _money = 0;
    //　装備している武器
    [SerializeField]
    private Item _equipWeapon = null;
    //　装備している鎧
    [SerializeField]
    private Item _equipArmor = null;
    //　アイテムと個数のDictionary
    [SerializeField]
    private ItemDictionary _itemDictionary = null;
    //　レベルアップデータ
    [SerializeField]
    private LevelUpData levelUpData = null;

    public int EarnedExperience { get => _earnedExperience; set => _earnedExperience = value; }
    public Item EquipWeapon { get => _equipWeapon; set => _equipWeapon = value; }
    public Item EquipArmor { get => _equipArmor; set => _equipArmor = value; }
    public ItemDictionary ItemDictionary { get => _itemDictionary; set => _itemDictionary = value; }
    public LevelUpData LevelUpData { get => levelUpData; set => levelUpData = value; }
    public int Money { get => _money; set => _money = value; }
}

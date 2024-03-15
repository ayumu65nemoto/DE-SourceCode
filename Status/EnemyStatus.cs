using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "EnemyStatus", menuName ="CreateEnemyStatus")]
public class EnemyStatus : CharacterStatus
{
    //�@�|�����Ƃ��ɓ�����o���l
    [SerializeField]
    private int _getExperience;
    //�@�|�����Ƃ��ɓ����邨��
    [SerializeField]
    private int _getMoney;
    //�@���Ƃ��A�C�e���Ɨ��Ƃ��m��
    [SerializeField]
    private ItemDictionary _dropItemDictionary;

    public int GetExperience { get => _getExperience; set => _getExperience = value; }
    public int GetMoney { get => _getMoney; set => _getMoney = value; }
    public ItemDictionary DropItemDictionary { get => _dropItemDictionary; set => _dropItemDictionary = value; }
}

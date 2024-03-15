using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "EnemyStatus", menuName ="CreateEnemyStatus")]
public class EnemyStatus : CharacterStatus
{
    //　倒したときに得られる経験値
    [SerializeField]
    private int _getExperience;
    //　倒したときに得られるお金
    [SerializeField]
    private int _getMoney;
    //　落とすアイテムと落とす確率
    [SerializeField]
    private ItemDictionary _dropItemDictionary;

    public int GetExperience { get => _getExperience; set => _getExperience = value; }
    public int GetMoney { get => _getMoney; set => _getMoney = value; }
    public ItemDictionary DropItemDictionary { get => _dropItemDictionary; set => _dropItemDictionary = value; }
}

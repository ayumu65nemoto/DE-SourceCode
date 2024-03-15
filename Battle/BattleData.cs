using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "BattleData", menuName = "CreateBattleData")]
public class BattleData : ScriptableObject
{
    [SerializeField]
    private BattlePartyStatus _battlePartyStatus;
    private EnemyPartyStatus _enemyPartyStatus;

    public BattlePartyStatus BattlePartyStatus { get => _battlePartyStatus; set => _battlePartyStatus = value; }
    public EnemyPartyStatus EnemyPartyStatus { get => _enemyPartyStatus; set => _enemyPartyStatus = value; }
}

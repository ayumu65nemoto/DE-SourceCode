using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "EnemyPartyStatusList", menuName = "CreateEnemyPartyStatusList")]
public class EnemyPartyStatusList : ScriptableObject
{
    [SerializeField]
    private List<EnemyPartyStatus> _partyMembersList;

    public List<EnemyPartyStatus> PartyMembersList { get => _partyMembersList; set => _partyMembersList = value; }
}

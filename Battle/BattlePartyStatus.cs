using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "BattlePartyStatus", menuName = "CreateBattlePartyStatus")]
public class BattlePartyStatus : ScriptableObject
{
    [SerializeField]
    private List<GameObject> _partyMember;

    public List<GameObject> PartyMember { get => _partyMember; set => _partyMember = value; }
}

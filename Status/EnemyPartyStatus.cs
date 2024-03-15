using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "EnemyPartyStatus", menuName = "CreateEnemyPartyStatus")]
public class EnemyPartyStatus : ScriptableObject
{
    [SerializeField]
    private string _partyName;
    [SerializeField]
    private List<GameObject> _partyMembers;

    public string PartyName { get => _partyName; set => _partyName = value; }
    public List<GameObject> PartyMembers { get => _partyMembers; set => _partyMembers = value; }
}

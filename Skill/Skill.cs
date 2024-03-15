using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
[CreateAssetMenu(fileName = "Skill", menuName = "CreateSkill")]
public class Skill : ScriptableObject
{
    public enum Type
    {
        DirectAttack,
        Guard,
        GetAway,
        Item,
        MagicAttack,
        RecoveryMagic,
        PoisonRecoveryMagic,
        SleepRecoveryMagic,
        ParalysisRecoveryMagic,
        ConfusionRecoveryMagic,
        DepressionRecoveryMagic,
        IncreaseAttackPowerMagic,
        IncreaseDefensePowerMagic
    }

    [SerializeField]
    private Type _skillType = Type.DirectAttack;
    [SerializeField]
    private string _name = "";
    [SerializeField]
    private string _info = "";
    //　使用者のエフェクト
    [SerializeField]
    private GameObject _skillUserEffect = null;
    //　魔法を受ける側のエフェクト
    [SerializeField]
    private GameObject _skillReceivingEffect = null;

    public Type SkillType { get => _skillType; set => _skillType = value; }
    public string Name { get => _name; set => _name = value; }
    public string Info { get => _info; set => _info = value; }
    public GameObject SkillUserEffect { get => _skillUserEffect; set => _skillUserEffect = value; }
    public GameObject SkillReceivingEffect { get => _skillReceivingEffect; set => _skillReceivingEffect = value; }
}

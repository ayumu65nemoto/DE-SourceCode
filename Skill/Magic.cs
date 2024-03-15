using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
[CreateAssetMenu(fileName = "Magic", menuName = "CreateMagic")]
public class Magic : Skill
{
    public enum MagicAttribute
    {
        Normal,
        Fire,
        Water,
        Thunder,
    }

    //　魔法力
    [SerializeField]
    private int _magicAmount = 0;
    //　使用MP
    [SerializeField]
    private int _magicPoints = 0;
    //　魔法の属性
    [SerializeField]
    private MagicAttribute _magicAttribute = MagicAttribute.Normal;

    public int MagicAmount { get => _magicAmount; set => _magicAmount = value; }
    public int MagicPoints { get => _magicPoints; set => _magicPoints = value; }
    public MagicAttribute MagicAttribute1 { get => _magicAttribute; set => _magicAttribute = value; }
}

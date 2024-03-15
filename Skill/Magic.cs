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

    //�@���@��
    [SerializeField]
    private int _magicAmount = 0;
    //�@�g�pMP
    [SerializeField]
    private int _magicPoints = 0;
    //�@���@�̑���
    [SerializeField]
    private MagicAttribute _magicAttribute = MagicAttribute.Normal;

    public int MagicAmount { get => _magicAmount; set => _magicAmount = value; }
    public int MagicPoints { get => _magicPoints; set => _magicPoints = value; }
    public MagicAttribute MagicAttribute1 { get => _magicAttribute; set => _magicAttribute = value; }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Data
{
    //�@�V�[���f�[�^
    [SerializeField]
    public SceneMove sceneMove;

    //�@�v���C���[�f�[�^
    [SerializeField]
    public PlayerStatus playerStatus;

    //�@�A�C�e���f�[�^
    [SerializeField]
    public ItemDictionary itemDictionary;

    //��b�f�[�^
    [SerializeField]
    public TalkManager talkManager;
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Data
{
    //　シーンデータ
    [SerializeField]
    public SceneMove sceneMove;

    //　プレイヤーデータ
    [SerializeField]
    public PlayerStatus playerStatus;

    //　アイテムデータ
    [SerializeField]
    public ItemDictionary itemDictionary;

    //会話データ
    [SerializeField]
    public TalkManager talkManager;
}

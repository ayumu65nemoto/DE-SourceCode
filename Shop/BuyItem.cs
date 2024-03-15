using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "BuyItem", menuName = "CreateBuyItem")]
public class BuyItem : ScriptableObject
{
    //�@�A�C�e���ƍw������Dictionary
    [SerializeField]
    private ItemDictionary _shopItemDictionary = null;

    public ItemDictionary ShopItemDictionary { get => _shopItemDictionary; set => _shopItemDictionary = value; }
}

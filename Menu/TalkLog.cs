using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TalkLog : MonoBehaviour
{
    //メッセージ表示位置
    [SerializeField]
    private Transform _content;
    //メッセージ要素Prefab
    [SerializeField]
    private GameObject _messageElement;
    
    public void AddMessage(string name, string messages)
    {
        //メッセージブロック生成
        GameObject messageBlock = Instantiate(_messageElement, _content);

        //名前を表示
        TextMeshProUGUI charName = messageBlock.transform.Find("CharName").GetComponent<TextMeshProUGUI>();
        charName.text = name;

        //本文を表示
        TextMeshProUGUI messageText = messageBlock.transform.Find("Message").GetComponent<TextMeshProUGUI>();
        messageText.text = messages;
    }
}

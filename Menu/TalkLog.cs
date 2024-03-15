using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TalkLog : MonoBehaviour
{
    //���b�Z�[�W�\���ʒu
    [SerializeField]
    private Transform _content;
    //���b�Z�[�W�v�fPrefab
    [SerializeField]
    private GameObject _messageElement;
    
    public void AddMessage(string name, string messages)
    {
        //���b�Z�[�W�u���b�N����
        GameObject messageBlock = Instantiate(_messageElement, _content);

        //���O��\��
        TextMeshProUGUI charName = messageBlock.transform.Find("CharName").GetComponent<TextMeshProUGUI>();
        charName.text = name;

        //�{����\��
        TextMeshProUGUI messageText = messageBlock.transform.Find("Message").GetComponent<TextMeshProUGUI>();
        messageText.text = messages;
    }
}

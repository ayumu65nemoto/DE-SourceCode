using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollDownScript : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    private ScrollManager _scrollManager;

    // Start is called before the first frame update
    void Start()
    {
        _scrollManager = GetComponentInParent<ScrollManager>();
    }

    //�@�{�^�����I�����ꂽ���Ɏ��s
    public void OnSelect(BaseEventData eventData)
    {
        _scrollManager.ScrollDown(transform);

        _scrollManager.PreSelectedButton = gameObject;
    }

    //�@�{�^�����I���������ꂽ���Ɏ��s
    public void OnDeselect(BaseEventData eventData)
    {

    }
}

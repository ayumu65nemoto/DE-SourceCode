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

    //　ボタンが選択された時に実行
    public void OnSelect(BaseEventData eventData)
    {
        _scrollManager.ScrollDown(transform);

        _scrollManager.PreSelectedButton = gameObject;
    }

    //　ボタンが選択解除された時に実行
    public void OnDeselect(BaseEventData eventData)
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollController : MonoBehaviour, IScrollHandler
{
    [SerializeField]
    private Scrollbar _scrollbar;
    [SerializeField]
    private float _scrollSpeed = 0.1f;

    public void OnScroll(PointerEventData eventData)
    {
        float scrollDelta = eventData.scrollDelta.y * _scrollSpeed;
        _scrollbar.value = Mathf.Clamp01(_scrollbar.value + scrollDelta);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollManager : MonoBehaviour
{
    //　アイテムボタン表示用コンテンツ
    private Transform _content;
    //　スクロール中かどうか
    private bool _changeScrollValue;
    //　スクロールの目的の値
    private float _destinationValue;
    //　スクロールスピード
    [SerializeField]
    private float _scrollSpeed = 1000f;
    //　一回でスクロールする値
    [SerializeField]
    private float _scrollValue = 215f;
    //　アイテム一覧のスクロールのデフォルト値
    private Vector3 _defaultScrollValue;
    //　前に選択していたボタン
    private GameObject preSelectedButton;

    public GameObject PreSelectedButton { get => preSelectedButton; set => preSelectedButton = value; }

    private void Awake()
    {
        _content = this.transform;
        _defaultScrollValue = new Vector3(0, -3000, 0);
        _destinationValue = -3000;
        transform.localPosition = _defaultScrollValue;
    }

    // Update is called once per frame
    void Update()
    {
        if (_changeScrollValue == false)
        {
            return;
        }

        //　徐々に目的の値に変化させる
        _content.transform.localPosition = new Vector3(_content.transform.localPosition.x, Mathf.MoveTowards(_content.transform.localPosition.y, _destinationValue, _scrollSpeed * Time.deltaTime), _content.transform.localPosition.z);

        //　ある程度移動したら目的地に設定
        if (Mathf.Abs(_content.transform.localPosition.y - _destinationValue) < 0.2f)
        {
            _changeScrollValue = false;
            _content.transform.localPosition = new Vector3(0f, _destinationValue, 0f);
        }
    }

    //　下にスクロール
    public void ScrollDown(Transform button)
    {
        if (_changeScrollValue == true)
        {
            _changeScrollValue = false;
            _content.transform.localPosition = new Vector3(_content.transform.localPosition.x, _destinationValue, _content.transform.localPosition.z);
        }

        if (preSelectedButton != null
            && button.position.y < preSelectedButton.transform.position.y)
        {
            _destinationValue = _content.transform.localPosition.y + _scrollValue;
            _changeScrollValue = true;
        }

    }
    //　上にスクロール
    public void ScrollUp(Transform button)
    {
        if (_changeScrollValue == true)
        {
            _content.transform.localPosition = new Vector3(_content.transform.localPosition.x, _destinationValue, _content.transform.localPosition.z);
            _changeScrollValue = false;
        }

        if (preSelectedButton != null
            && button.position.y > preSelectedButton.transform.position.y)
        {
            _destinationValue = _content.transform.localPosition.y - _scrollValue;
            _changeScrollValue = true;
        }
    }

    public void Reset()
    {
        PreSelectedButton = null;
        transform.localPosition = _defaultScrollValue;
    }
}

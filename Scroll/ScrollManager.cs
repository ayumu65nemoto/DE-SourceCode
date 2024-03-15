using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollManager : MonoBehaviour
{
    //�@�A�C�e���{�^���\���p�R���e���c
    private Transform _content;
    //�@�X�N���[�������ǂ���
    private bool _changeScrollValue;
    //�@�X�N���[���̖ړI�̒l
    private float _destinationValue;
    //�@�X�N���[���X�s�[�h
    [SerializeField]
    private float _scrollSpeed = 1000f;
    //�@���ŃX�N���[������l
    [SerializeField]
    private float _scrollValue = 215f;
    //�@�A�C�e���ꗗ�̃X�N���[���̃f�t�H���g�l
    private Vector3 _defaultScrollValue;
    //�@�O�ɑI�����Ă����{�^��
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

        //�@���X�ɖړI�̒l�ɕω�������
        _content.transform.localPosition = new Vector3(_content.transform.localPosition.x, Mathf.MoveTowards(_content.transform.localPosition.y, _destinationValue, _scrollSpeed * Time.deltaTime), _content.transform.localPosition.z);

        //�@������x�ړ�������ړI�n�ɐݒ�
        if (Mathf.Abs(_content.transform.localPosition.y - _destinationValue) < 0.2f)
        {
            _changeScrollValue = false;
            _content.transform.localPosition = new Vector3(0f, _destinationValue, 0f);
        }
    }

    //�@���ɃX�N���[��
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
    //�@��ɃX�N���[��
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

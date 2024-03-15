using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CommandButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    //�@�{�^����I���������ɕ\������摜
    private Image _selectedImage;
    private SoundManager _soundManager;
    //�@�ǂ̃{�^�����������̂��c��
    [SerializeField]
    private string _selectLang;
    private int _selectLangNum;

    public string SelectLang { get => _selectLang; set => _selectLang = value; }
    public int SelectLangNum { get => _selectLangNum; set => _selectLangNum = value; }

    void Awake()
    {
        if (transform.Find("Image") != null)
        {
            _selectedImage = transform.Find("Image").GetComponent<Image>();
        }
        else
        {
            GameObject parent = transform.GetChild(0).gameObject;
            _selectedImage = parent.transform.Find("Image").GetComponent<Image>();
        }
    }

    private void OnEnable()
    {
        _soundManager = SoundManager.soundManager;
        //�@�A�N�e�B�u�ɂȂ��������g��EventSystem�őI������Ă�����
        if (EventSystem.current.currentSelectedGameObject == this.gameObject)
        {
            _selectedImage.enabled = true;
            _soundManager.PlaySelectSE();
        }
        else
        {
            _selectedImage.enabled = false;
        }
    }

    //�@�{�^�����I�����ꂽ���Ɏ��s
    public void OnSelect(BaseEventData eventData)
    {
        _soundManager = SoundManager.soundManager;
        if (transform.Find("Image") != null)
        {
            _selectedImage = transform.Find("Image").GetComponent<Image>();
        }
        else
        {
            GameObject parent = transform.GetChild(0).gameObject;
            _selectedImage = parent.transform.Find("Image").GetComponent<Image>();
        }
        _selectedImage.enabled = true;
        _soundManager.PlaySelectSE();
    }
    //�@�{�^�����I���������ꂽ���Ɏ��s
    public void OnDeselect(BaseEventData eventData)
    {
        if (transform.Find("Image") != null)
        {
            _selectedImage = transform.Find("Image").GetComponent<Image>();
        }
        else
        {
            GameObject parent = transform.GetChild(0).gameObject;
            _selectedImage = parent.transform.Find("Image").GetComponent<Image>();
        }
        _selectedImage.enabled = false;
    }
}

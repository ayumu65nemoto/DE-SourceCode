using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CommandButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    //　ボタンを選択した時に表示する画像
    private Image _selectedImage;
    private SoundManager _soundManager;
    //　どのボタンを押したのか把握
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
        //　アクティブになった時自身がEventSystemで選択されていたら
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

    //　ボタンが選択された時に実行
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
    //　ボタンが選択解除された時に実行
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

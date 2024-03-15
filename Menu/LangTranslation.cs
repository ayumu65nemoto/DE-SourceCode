using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LangTranslation : MonoBehaviour
{
    [SerializeField]
    private TalkManager _talkManager;
    [SerializeField]
    private CommandButton _commandButton;
    [SerializeField]
    private DictionaryMode _dictionaryMode;
    //�I���������t
    private string _selectWord;
    //�@�L���������t
    [SerializeField]
    private TMP_InputField _inputText;
    //�@�|�󂷂�P��
    private string _selectLang;

    public string SelectLang { get => _selectLang; set => _selectLang = value; }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    public void OnClick()
    {
        if (_talkManager == null)
        {
            _talkManager = TalkManager.instance;
        }
        _selectWord = this.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
        _talkManager.WordDictionary[_commandButton.SelectLang].SelectWord = _selectWord;
        _dictionaryMode.UpdateDictionary();
    }

    public void InputTranslation()
    {
        if (_talkManager == null)
        {
            _talkManager = TalkManager.instance;
        }
        _talkManager.WordDictionary[SelectLang].SelectWord = _inputText.text;
        _dictionaryMode.UpdateDictionary();
    }
}

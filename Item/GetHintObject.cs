using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GetHintObject : MonoBehaviour
{
    [SerializeField]
    private TalkManager _talkManager;
    [SerializeField]
    private GameObject _eventUI;
    [SerializeField]
    private TextMeshProUGUI _eventText;
    [SerializeField]
    private int _translateNum = 0;
    [SerializeField]
    private GameObject _selecter;

    // Start is called before the first frame update
    void Start()
    {
        _talkManager = TalkManager.instance;
        _selecter.SetActive(false);
    }

    void OnTriggerStay(Collider col)
    {
        if (col.tag == "Player")
        {
            _selecter.SetActive(true);

            if (Input.GetKeyDown(KeyCode.F))
            {
                _eventUI.SetActive(true);
                if (_talkManager.Vocabulary[_translateNum].SelectWord != "")
                {
                    _eventText.text = _talkManager.Vocabulary[_translateNum].SelectWord;
                }
                else
                {
                    _talkManager.HintLists[_translateNum].IsHint = true;
                    _eventText.text = col.GetComponent<PlayerTalk>().ConvFoolCoolFont(_talkManager.Vocabulary[_translateNum].OriginalWord);
                }
                StartCoroutine(DisableObjectAfterDelay());
            }
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
        {
            _selecter.SetActive(false);
        }
    }

    private IEnumerator DisableObjectAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        _eventUI.SetActive(false);
    }
}

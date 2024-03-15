using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GetWordsObject : MonoBehaviour
{
    [SerializeField]
    private TalkManager _talkManager;
    [SerializeField]
    private GameObject _eventUI;
    [SerializeField]
    private TextMeshProUGUI _eventText;
    [SerializeField]
    private List<int> _translateList = new List<int>();
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
                _eventText.text = "Ç¢Ç≠Ç¬Ç©ÇÃíPåÍÇäoÇ¶ÇΩÅI";
                _eventUI.SetActive(true);
                for (int i = 0; i < _translateList.Count; i++)
                {
                    _talkManager.Vocabulary[_translateList[i]].SelectWord = _talkManager.Vocabulary[_translateList[i]].Key;
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

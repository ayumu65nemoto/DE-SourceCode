using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoGetHints : MonoBehaviour
{
    [SerializeField]
    private TalkManager _talkManager;
    [SerializeField]
    private GameObject _gameObject;

    private float _time;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < _talkManager.HintLists.Count; i++)
        {
            _talkManager.HintLists[i].IsHint = false;
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (col.tag == "Player")
        {
           if (Input.GetKeyDown(KeyCode.F))
           {
                for (int i = 0; i < _talkManager.HintLists.Count; i++)
                {
                    _gameObject.SetActive(true);
                    _talkManager = TalkManager.instance;
                    _talkManager.HintLists[i].IsHint = true;
                    StartCoroutine(DisableObjectAfterDelay());
                }
            }
        }
    }

    private IEnumerator DisableObjectAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        _gameObject.SetActive(false);
    }
}

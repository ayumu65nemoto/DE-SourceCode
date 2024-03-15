using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    //NPCが話しかけられたか
    private bool _npcState = false;
    [SerializeField]
    protected Conversation _conversation1 = null;
    //　PlayerのTransform
    private Transform _conversationPartnerTransform;
    //　村人がユニティちゃんの方向に回転するスピード
    [SerializeField]
    private float _rotationSpeed = 2f;
    
    protected GameManager _gameManager;
    [SerializeField]
    protected PlayerTalk _playerTalk;

    //　上下させたい好感度の値
    [SerializeField]
    private int _favValue1;
    [SerializeField]
    private int _favValue2;

    // TipsController
    [SerializeField]
    protected TipsController _tipsController;
    // QuestManager
    [SerializeField]
    protected QuestManager _questManager;
    // TalkManager
    [SerializeField]
    protected TalkManager _talkManager;

    //public int FavRate { get => _favRate; set => _favRate = value; }
    public bool NpcState { get => _npcState; set => _npcState = value; }


    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameManager.gameManager;
        _playerTalk = PlayerTalk.instance;
        _talkManager = TalkManager.instance;
        _questManager = QuestManager.questManager;
        _tipsController = TipsController.tipsController;
    }

    // Update is called once per frame
    void Update()
    {
        if (NpcState == true)
        {
            //　村人がユニティちゃんの方向をある程度向くまで回転させる
            if (Vector3.Angle(transform.forward, new Vector3(_conversationPartnerTransform.position.x, transform.position.y, _conversationPartnerTransform.position.z) - transform.position) > 5f)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(new Vector3(_conversationPartnerTransform.position.x, transform.position.y, _conversationPartnerTransform.position.z) - transform.position), _rotationSpeed * Time.deltaTime);
                //_animator.SetFloat("Speed", 1f);
            }
            else
            {
                //_animator.SetFloat("Speed", 0f);
            }

            NpcState = false;
        }
    }

    //private IEnumerator MoveNPCCoroutine()
    //{
    //    Vector3 startPosition = _gameObj.transform.position;
    //    Vector3 targetPosition = _gameObj.transform.position + new Vector3(0f, 0f, -18f);
    //    float currentTime = 0f;

    //    while (currentTime < 2f)
    //    {
    //        currentTime += Time.deltaTime;
    //        float t = Mathf.Clamp01(currentTime / 2f);
    //        _gameObj.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
    //        yield return null;
    //    }
    //}

    //　Conversionスクリプトを返す
    //public Conversation GetConversation()
    //{
    //    if (_gameManager.AllGameFlags["アメリアを助ける"] == true && _gameManager.AllGameFlags["アメリアの依頼達成"] == false)
    //    {
    //        return _conversation4_1;
    //    }
    //    else if (_gameManager.AllGameFlags["アメリアを助けない"] == true && _gameManager.AllGameFlags["アメリアの依頼達成"] == false)
    //    {
    //        return _conversation4_2;
    //    }
    //    else if (_gameManager.AllGameFlags["アメリアの依頼達成"] == true)
    //    {
    //        return _conversation5;
    //    }
    //    return _conversation;
    //}

    //public Conversation GetConversation2()
    //{
    //    return _conversation2;
    //}

    //public Conversation GetConversation3()
    //{
    //    if (_gameManager.IsEvent1 == true)
    //    {
    //        StartCoroutine(MoveNPCCoroutine());
    //        _gameManager.IsEvent1 = false;
    //    }

    //    _playerTalk.IsActiveOneFlag = "アメリアを助ける";
    //    _playerTalk.FavRate1 = _favValue1;
    //    _playerTalk.IsActiveTwoFlag = "アメリアを助けない";
    //    _playerTalk.FavRate2 = _favValue2;
    //    return _conversation3;
    //}
}

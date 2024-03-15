using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Encount : MonoBehaviour
{
    //　LoadSceneManager
    private LoadSceneManager _loadSceneManager;
    //　プレイヤー
    private Transform _playerObject;
    //　プレイヤーのスクリプト
    private PlayerController _playerController;
    //　戦闘データ
    [SerializeField]
    private BattleData _battleData;
    //　敵パーティリスト
    [SerializeField]
    private EnemyPartyStatusList _enemyPartyStatusList;
    [SerializeField]
    private SceneMove _sceneMove = null;
    //　イベントバトルならば
    [SerializeField]
    private Quest _quest;

    // Start is called before the first frame update
    void Start()
    {
        _loadSceneManager = LoadSceneManager.loadSceneManager;
        if (_quest != null)
        {
            if (_quest.IsHappen == true)
            {
                gameObject.SetActive(true);
            }
            if (_quest.IsClear == true)
            {
                gameObject.SetActive(false);
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            _playerObject = col.transform;
            col.GetComponent<PlayerController>().SetPlayerState(PlayerController.PlayerState.Wait);
            //　敵の遭遇をランダムに決定
            int num = Random.Range(0, _enemyPartyStatusList.PartyMembersList.Count);
            _battleData.EnemyPartyStatus = _enemyPartyStatusList.PartyMembersList[num];

            //　戦闘用のパラメータをScriptableObjectのデータに入れる
            //battleParam.friendLists = col.GetComponent<FriendParty>().friendLists;
            //battleParam.enemyLists = enemyParty.enemyLists;
            ////　主人公の位置を入れる
            //battleParam.pos = col.transform.position;
            //battleParam.rot = col.transform.rotation;

            //　戦闘前の位置情報格納
            _sceneMove.WorldMapPos = _playerObject.transform.position;
            _sceneMove.WorldMapRot = _playerObject.transform.rotation;

            //　戦闘シーンの読み込み
            //if (SceneManager.GetActiveScene().name == "Map1")
            //{
            //    _loadSceneManager.GoToNextScene(SceneMove.SceneType.VillageToBattle);
            //}
            //else
            //{
            //    _loadSceneManager.GoToNextScene(SceneMove.SceneType.WorldMapToBattle);
            //}

            if (_quest != null)
            {
                _quest.IsClear = true;
                QuestManager.questManager.ShowQuest("依頼主に報告しよう");
            }
            Destroy(gameObject);
        }
    }
}

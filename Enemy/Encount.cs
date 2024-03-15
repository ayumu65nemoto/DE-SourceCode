using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Encount : MonoBehaviour
{
    //�@LoadSceneManager
    private LoadSceneManager _loadSceneManager;
    //�@�v���C���[
    private Transform _playerObject;
    //�@�v���C���[�̃X�N���v�g
    private PlayerController _playerController;
    //�@�퓬�f�[�^
    [SerializeField]
    private BattleData _battleData;
    //�@�G�p�[�e�B���X�g
    [SerializeField]
    private EnemyPartyStatusList _enemyPartyStatusList;
    [SerializeField]
    private SceneMove _sceneMove = null;
    //�@�C�x���g�o�g���Ȃ��
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
            //�@�G�̑����������_���Ɍ���
            int num = Random.Range(0, _enemyPartyStatusList.PartyMembersList.Count);
            _battleData.EnemyPartyStatus = _enemyPartyStatusList.PartyMembersList[num];

            //�@�퓬�p�̃p�����[�^��ScriptableObject�̃f�[�^�ɓ����
            //battleParam.friendLists = col.GetComponent<FriendParty>().friendLists;
            //battleParam.enemyLists = enemyParty.enemyLists;
            ////�@��l���̈ʒu������
            //battleParam.pos = col.transform.position;
            //battleParam.rot = col.transform.rotation;

            //�@�퓬�O�̈ʒu���i�[
            _sceneMove.WorldMapPos = _playerObject.transform.position;
            _sceneMove.WorldMapRot = _playerObject.transform.rotation;

            //�@�퓬�V�[���̓ǂݍ���
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
                QuestManager.questManager.ShowQuest("�˗���ɕ񍐂��悤");
            }
            Destroy(gameObject);
        }
    }
}

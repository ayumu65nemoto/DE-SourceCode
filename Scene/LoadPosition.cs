using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadPosition : MonoBehaviour
{
    [SerializeField]
    private SceneMove _sceneMove = null;

    // Start is called before the first frame update
    void Start()
    {
        // �V�[���J�ڂ̎�ނɉ����ď����ʒu�̃Q�[���I�u�W�F�N�g�̈ʒu�Ɗp�x�ɐݒ�
        if (_sceneMove.GetSceneType() == SceneMove.SceneType.StartGame)
        {
            var initialPosition = GameObject.Find("InitialPosition").transform;
            transform.position = initialPosition.position;
            transform.rotation = initialPosition.rotation;
        }
        else if (_sceneMove.GetSceneType() == SceneMove.SceneType.FirstVillage)
        {
            var initialPosition = GameObject.Find("InitialPosition").transform;
            transform.position = initialPosition.position;
            transform.rotation = initialPosition.rotation;
        }
        else if (_sceneMove.GetSceneType() == SceneMove.SceneType.FirstVillageToRoom)
        {
            var initialPosition = GameObject.Find("InitialPositionFirstToRoom").transform;
            transform.position = initialPosition.position;
            transform.rotation = initialPosition.rotation;
        }
        else if (_sceneMove.GetSceneType() == SceneMove.SceneType.RoomToFirstVillage)
        {
            var initialPosition = GameObject.Find("InitialPositionRoomToFirstVillage").transform;
            transform.position = initialPosition.position;
            transform.rotation = initialPosition.rotation;
        }
        //else if (_sceneMove.GetSceneType() == SceneMove.SceneType.FirstVillageToWorldMap)
        //{
        //    var initialPosition = GameObject.Find("InitialPositionFirstToWorld").transform;
        //    transform.position = initialPosition.position;
        //    transform.rotation = initialPosition.rotation;
        //}
        //else if (_sceneMove.GetSceneType() == SceneMove.SceneType.BattleToWorldMap)
        //{
        //    transform.position = _sceneMove.WorldMapPos;
        //    transform.rotation = _sceneMove.WorldMapRot;
        //}
        //else if (_sceneMove.GetSceneType() == SceneMove.SceneType.BattleToVillage)
        //{
        //    transform.position = _sceneMove.WorldMapPos;
        //    transform.rotation = _sceneMove.WorldMapRot;
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

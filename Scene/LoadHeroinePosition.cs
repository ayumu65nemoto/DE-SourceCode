using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadHeroinePosition : MonoBehaviour
{
    [SerializeField]
    private SceneMove _sceneMove = null;

    // Start is called before the first frame update
    void Start()
    {
        // �V�[���J�ڂ̎�ނɉ����ď����ʒu�̃Q�[���I�u�W�F�N�g�̈ʒu�Ɗp�x�ɐݒ�
        if (_sceneMove.GetSceneType() == SceneMove.SceneType.StartGame)
        {
            var initialPosition = GameObject.Find("HeroinePosition").transform;
            transform.position = initialPosition.position;
            transform.rotation = initialPosition.rotation;
        }
        else if (_sceneMove.GetSceneType() == SceneMove.SceneType.FirstVillage)
        {
            var initialPosition = GameObject.Find("HeroinePosition").transform;
            transform.position = initialPosition.position;
            transform.rotation = initialPosition.rotation;
        }
        else if (_sceneMove.GetSceneType() == SceneMove.SceneType.RoomToFirstVillage)
        {
            var initialPosition = GameObject.Find("HeroinePosRoomToFirstVillage").transform;
            transform.position = initialPosition.position;
            transform.rotation = initialPosition.rotation;
        }
    }
}

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
        // シーン遷移の種類に応じて初期位置のゲームオブジェクトの位置と角度に設定
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneMove", menuName = "CreateSceneMove")]
public class SceneMove : ScriptableObject
{
    public enum SceneType
    {
        StartGame,
        FirstVillage,
        FirstVillageToRoom,
        RoomToFirstVillage,
        //FirstVillageToWorldMap,
        //WorldMapToBattle,
        //BattleToWorldMap,
        //VillageToBattle,
        //BattleToVillage
    }

    [SerializeField]
    private SceneType _sceneType;

    //　ワールドマップ→戦闘シーンへ移行した時のワールドマップの位置情報
    private Vector3 _worldMapPos;
    //　ワールドマップ→戦闘シーンへ移行した時のワールドマップの位置情報
    private Quaternion _worldMapRot;

    public SceneType SceneType1 { get => _sceneType; set => _sceneType = value; }
    public Vector3 WorldMapPos { get => _worldMapPos; set => _worldMapPos = value; }
    public Quaternion WorldMapRot { get => _worldMapRot; set => _worldMapRot = value; }

    public void OnEnable()
    {
        SceneType1 = SceneType.StartGame;
    }

    public void SetSceneType(SceneType scene)
    {
        SceneType1 = scene;
    }

    public SceneType GetSceneType()
    {
        return SceneType1;
    }
}

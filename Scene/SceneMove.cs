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

    //�@���[���h�}�b�v���퓬�V�[���ֈڍs�������̃��[���h�}�b�v�̈ʒu���
    private Vector3 _worldMapPos;
    //�@���[���h�}�b�v���퓬�V�[���ֈڍs�������̃��[���h�}�b�v�̈ʒu���
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

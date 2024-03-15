using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToOtherScene : MonoBehaviour
{
    private LoadSceneManager sceneManager;
    //�@�ǂ̃V�[���֑J�ڂ��邩
    [SerializeField]
    private SceneMove.SceneType scene = SceneMove.SceneType.FirstVillage;
    //�@�V�[���J�ڒ����ǂ���
    private bool isTransition;

    private void Awake()
    {
        sceneManager = FindObjectOfType<LoadSceneManager>();
    }

    private void OnTriggerEnter(Collider col)
    {
        //�@���̃V�[���֑J�ړr���łȂ���
        if (col.tag == "Player" && !isTransition)
        {
            isTransition = true;
            sceneManager.GoToNextScene(scene);
        }
    }
    //�@�t�F�[�h��������ɃV�[���ǂݍ���
    IEnumerator FadeAndLoadScene(SceneMove.SceneType scene)
    {
        /*
        ���̑��̏���
        */
        isTransition = false;
        yield return null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepingBed : MonoBehaviour
{
    private LoadSceneManager _sceneManager;
    private GameManager _gameManager;
    //�@�ǂ̃V�[���֑J�ڂ��邩
    [SerializeField]
    private SceneMove.SceneType scene = SceneMove.SceneType.FirstVillage;
    //�@�V�[���J�ڒ����ǂ���
    private bool isTransition;

    // Start is called before the first frame update
    void Start()
    {
        _sceneManager =LoadSceneManager.loadSceneManager;
        _gameManager = GameManager.gameManager;
    }

    private void OnTriggerStay(Collider col)
    {
        //�@���̃V�[���֑J�ړr���łȂ���
        if (col.tag == "Player" && !isTransition 
            && _gameManager.AllGameFlags["Chapter1_ed_2"] == true 
            && _gameManager.AllGameFlags["Chapter2_op_1"] == false
            && _gameManager.IsSleep == false)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                isTransition = true;
                _gameManager.IsSleep = true;
                _gameManager.Favorability = 3;
                _sceneManager.GoToNextScene(scene);
            }
        }
    }
}

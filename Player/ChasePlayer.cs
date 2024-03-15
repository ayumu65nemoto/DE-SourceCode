using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChasePlayer : MonoBehaviour
{
    [SerializeField]
    private GameObject _target;
    private NavMeshAgent _agent;
    private GameManager _gameManager;
    private Animator _animator;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _gameManager = GameManager.gameManager;
        _animator = GetComponent<Animator>();

        if (_gameManager.FirstConv == true)
        {
            this.enabled = false;
        }

        if (_agent == null)
        {
            Debug.LogError("NavMeshAgent コンポーネントが見つかりませんでした。");
        }
        else
        {
            SetStoppingDistance(_gameManager.Favorability); // 好感度に応じてStopping Distanceを初期化
        }
    }

    void Update()
    {
        if (_target && _agent) // targetとagentがnullでないことを確認
        {
            // エージェントをプレイヤーの方向に移動させる
            _agent.destination = _target.transform.position;
            _animator.SetFloat("Speed", _agent.velocity.magnitude);
        }
    }

    // 信頼度によってStopping Distanceを変更する関数
    public void SetStoppingDistance(int favorability)
    {
        // もしagentがnullの場合は何もせずに終了
        if (_agent == null)
        {
            Debug.LogError("NavMeshAgent コンポーネントが見つかりませんでした。");
            return;
        }

        // 好感度に応じてStopping Distanceを設定する
        if (favorability > 5)
        {
            _agent.stoppingDistance = 3;
        }
        else if (favorability > 2)
        {
            _agent.stoppingDistance = 6;
        }
        else
        {
            _agent.stoppingDistance = 9;
        }
    }
}







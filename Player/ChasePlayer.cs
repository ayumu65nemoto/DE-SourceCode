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
            Debug.LogError("NavMeshAgent �R���|�[�l���g��������܂���ł����B");
        }
        else
        {
            SetStoppingDistance(_gameManager.Favorability); // �D���x�ɉ�����Stopping Distance��������
        }
    }

    void Update()
    {
        if (_target && _agent) // target��agent��null�łȂ����Ƃ��m�F
        {
            // �G�[�W�F���g���v���C���[�̕����Ɉړ�������
            _agent.destination = _target.transform.position;
            _animator.SetFloat("Speed", _agent.velocity.magnitude);
        }
    }

    // �M���x�ɂ����Stopping Distance��ύX����֐�
    public void SetStoppingDistance(int favorability)
    {
        // ����agent��null�̏ꍇ�͉��������ɏI��
        if (_agent == null)
        {
            Debug.LogError("NavMeshAgent �R���|�[�l���g��������܂���ł����B");
            return;
        }

        // �D���x�ɉ�����Stopping Distance��ݒ肷��
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







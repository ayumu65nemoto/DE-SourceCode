using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    //�R���|�[�l���g
    private Rigidbody _rb;
    private Animator _animator;
    //����
    private float _horInput;
    private float _verInput;
    //�ړ��X�s�[�h
    [SerializeField] 
    private float _moveSpeed;
    //�v���C���[�̏��
    public enum PlayerState
    {
        Normal,
        Talk,
        Wait,
        Command,
        //Shop
    }
    [SerializeField]
    private PlayerState _playerState;
    //PlayerTalk
    private PlayerTalk _playerTalk;
    // PlayerStatus
    [SerializeField]
    private PlayerStatus _playerStatus;

    //�@Camera
    [SerializeField]
    private GameObject _camera;
    [SerializeField]
    private CameraController _cameraController;

    //�@TipsController
    [SerializeField]
    private TipsController _tipsController;

    private GameManager _gameManager;

    // Audio
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _walkSE;

    //�����Ă��邩
    private bool isWalk = false;

    public PlayerState PlayerState1 { get => _playerState; set => _playerState = value; }
    public PlayerStatus PlayerStatus { get => _playerStatus; set => _playerStatus = value; }

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        PlayerState1 = PlayerState.Normal;
        _playerTalk = GetComponent<PlayerTalk>();
        PlayerState1 = PlayerState.Wait;
        _tipsController = TipsController.tipsController;
        _gameManager = GameManager.gameManager;

        //�����b�i�������j����n�߂�
        if (_playerTalk.GetConversationPartner() != null && _gameManager.Initialize == true)
        {
            _gameManager.Initialize = false;
            SetPlayerState(PlayerState.Talk);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerState1 == PlayerState.Normal)
        {
            PlayerMove();
        }
        else if (PlayerState1 == PlayerState.Talk)
        {

        }
        else if (PlayerState1 == PlayerState.Wait)
        {
            if (_playerTalk.GetConversationPartner() != null
                && Input.GetKeyDown(KeyCode.F)
                )
            {
                SetPlayerState(PlayerState.Talk);
            }
        }

        //�@�V�[���J�ڌ�Ɉړ�������ƃf�t�H���g�̈ʒu�ɃL�����N�^�[���Z�b�g����Ă��܂��̂ŉ��
        if (PlayerState1 != PlayerState.Wait)
        {
            Vector3 newVelocity = _rb.velocity;
            newVelocity.y = Physics.gravity.y * Time.deltaTime;
            _rb.velocity = newVelocity;
        }
        else
        {
            if (!Mathf.Approximately(Input.GetAxis("Horizontal"), 0f) || !Mathf.Approximately(Input.GetAxis("Vertical"), 0f))
            {
                SetPlayerState(PlayerState.Normal);
            }
        }
    }

    private void PlayerMove()
    {
        //WASD�݂̂ňړ�
        if (Input.GetKey(KeyCode.W))
        {
            _verInput = 1f;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            _verInput = -1f;
        }
        else
        {
            _verInput = 0f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            _horInput = -1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            _horInput = 1f;
        }
        else
        {
            _horInput = 0f;
        }

        // �����Ă����痎�Ƃ�
        Vector3 rayPosition = transform.position + new Vector3(0.0f, 0.4f, 0.0f);
        Ray ray = new Ray(rayPosition, Vector3.down);
        bool isGround = Physics.Raycast(ray, 0.6f);
        Debug.DrawRay(rayPosition, Vector3.down * 0.6f, Color.red);
        if (isGround == false)
        {
            _rb.AddForce(Vector3.down * 500f, ForceMode.Impulse);
        }

        // �J�����̕�������AX-Z���ʂ̒P�ʃx�N�g�����擾
        Vector3 cameraForward = Vector3.Scale(_camera.transform.forward, new Vector3(1, 0, 1)).normalized;
        // �����L�[�̓��͒l�ƃJ�����̌�������A�ړ�����������
        Vector3 moveForward = cameraForward * _verInput + _camera.transform.right * _horInput;
        //�ړ�����
        _rb.velocity = moveForward * _moveSpeed;
        //������ς���
        if (moveForward != Vector3.zero)
        {
            this.transform.rotation = Quaternion.LookRotation(moveForward);
        }
        //�A�j���[�^�[�J��
        _animator.SetFloat("Speed", _rb.velocity.magnitude);

        //SE�Đ�
        CheckWalking();

        if (_playerTalk.GetConversationPartner() != null
            && _tipsController.IsActiveTips == false
            && Input.GetKeyDown(KeyCode.F)
                )
        {
            SetPlayerState(PlayerState.Talk);
        }
    }

    //�@��ԕύX�Ə����ݒ�
    public void SetPlayerState(PlayerState state)
    {
        PlayerState1 = state;

        if (_cameraController == null && FindAnyObjectByType<CameraController>() != null)
        {
            _cameraController = FindAnyObjectByType<CameraController>();
        }

        if (state == PlayerState.Normal)
        {
            _cameraController.IsActive = true;
        }
        else if (state == PlayerState.Talk)
        {
            _rb.velocity = Vector3.zero;
            _animator.SetFloat("Speed", 0f);
            _cameraController.IsActive = false;
            _playerTalk.StartTalking();
        }
        else if (state == PlayerState.Command)
        {
            _rb.velocity = Vector3.zero;
            _animator.SetFloat("Speed", 0f);
            _cameraController.IsActive = false;
        }
        else if (state == PlayerState.Wait)
        {
            _rb.velocity = Vector3.zero;
            _animator.SetFloat("Speed", 0f);
            _cameraController.IsActive = false;
        }
    }

    public PlayerState GetState()
    {
        return PlayerState1;
    }

    private void CheckWalking()
    {
        float characterSpeed = GetComponent<Rigidbody>().velocity.magnitude;
        isWalk = characterSpeed > 0.1f;
    }
}

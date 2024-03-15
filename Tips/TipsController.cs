using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipsController : MonoBehaviour
{
    public static TipsController tipsController;

    // Tips�̃��X�g
    [SerializeField]
    private List<GameObject> _tipsList = new List<GameObject>();
    // �g�p�ς�Tips�̃��X�g
    [SerializeField]
    private List<int> _usedTipsList = new List<int>();
    // Tips���J���Ă��邩
    private bool _isActiveTips = false;
    // ���Ԃ�Tips���J���Ă��邩
    private int _isActiveNum = 0;
    // PlayerController
    [SerializeField]
    private PlayerController _playerController;

    public List<GameObject> TipsList { get => _tipsList; set => _tipsList = value; }
    public bool IsActiveTips { get => _isActiveTips; set => _isActiveTips = value; }
    public List<int> UsedTipsList { get => _usedTipsList; set => _usedTipsList = value; }

    private void Awake()
    {
        // LoadSceneManger�͏�Ɉ�����ɂ���
        if (tipsController == null)
        {
            tipsController = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //ShowTips(0);
    }

    private void Update()
    {
        if (IsActiveTips == true)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
            {
                _tipsList[_isActiveNum].SetActive(false);
                StartCoroutine(CloseTipsCoroutine());
            }
        }
    }

    public void ShowTips(int number)
    {
        if (UsedTipsList.Contains(number) == true)
        {
            return;
        }
        _tipsList[number].SetActive(true);
        IsActiveTips = true;
        _isActiveNum = number;
        // �g�p�ς݃��X�g�ɒǉ�
        UsedTipsList.Add(number);
    }

    private IEnumerator CloseTipsCoroutine()
    {
        yield return new WaitForSeconds(1);
        IsActiveTips = false;
        _playerController.SetPlayerState(PlayerController.PlayerState.Normal);
    }
}

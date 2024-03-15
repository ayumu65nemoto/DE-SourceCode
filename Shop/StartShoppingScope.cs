using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartShoppingScope : MonoBehaviour
{
    [SerializeField]
    private GameObject _shopUI;
    [SerializeField]
    private BuyItem _buyItem;
    //OnTriggerEnter�̒���Input�����Ȃ���������̂�
    private bool _isFree = false;
    private PlayerController _playerController;

    private void Update()
    {
        //if (_isFree == true && Input.GetKeyDown(KeyCode.F) && _playerController.GetState() != PlayerController.PlayerState.Shop)
        //{
        //    //�@�v���C���[���V���b�v��ԂɑJ��
        //    _playerController.SetPlayerState(PlayerController.PlayerState.Shop);
        //    //�@BuyItem�󂯓n��
        //    _shopUI.GetComponent<ShopMenu>().BuyItem = _buyItem;
        //    //�@�V���b�v���j���[���J��
        //    _shopUI.SetActive(true);
        //}
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            _isFree = true;
            _playerController = col.GetComponent<PlayerController>();
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
        {
            _isFree = false;
        }
    }
}

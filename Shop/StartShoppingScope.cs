using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartShoppingScope : MonoBehaviour
{
    [SerializeField]
    private GameObject _shopUI;
    [SerializeField]
    private BuyItem _buyItem;
    //OnTriggerEnterの中でInputが取れない時があるので
    private bool _isFree = false;
    private PlayerController _playerController;

    private void Update()
    {
        //if (_isFree == true && Input.GetKeyDown(KeyCode.F) && _playerController.GetState() != PlayerController.PlayerState.Shop)
        //{
        //    //　プレイヤーをショップ状態に遷移
        //    _playerController.SetPlayerState(PlayerController.PlayerState.Shop);
        //    //　BuyItem受け渡し
        //    _shopUI.GetComponent<ShopMenu>().BuyItem = _buyItem;
        //    //　ショップメニューを開く
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleStatusScript : MonoBehaviour
{
    public enum Status
    {
        HP,
        MP,
        Mot
    }
    //　プレイヤーステータス
    [SerializeField]
    private PlayerStatus _playerStatus;
    //　エネミーステータス
    [SerializeField]
    private EnemyStatus _enemyStatus;
    //　プレイヤー名前
    [SerializeField]
    private TextMeshProUGUI _characterName;
    //　プレイヤーHPスライダー
    [SerializeField]
    private Slider _hpSlider;
    //　プレイヤーHPテキスト
    [SerializeField]
    private TextMeshProUGUI _hpText;
    //　プレイヤーMPスライダー
    [SerializeField]
    private Slider _mpSlider;
    //　プレイヤーMPテキスト
    [SerializeField]
    private TextMeshProUGUI _mpText;
    //　プレイヤーMotスライダー
    [SerializeField]
    private Slider _motSlider;
    //　プレイヤーMotテキスト
    [SerializeField]
    private TextMeshProUGUI _motText;

    // Start is called before the first frame update
    void Start()
    {
        //　初めのデータ表示
        if (_playerStatus != null)
        {
            DisplayStatus();
        }
        else
        {
            DisplayEnemyStatus();
        }
    }

    //　ステータス表示
    public void DisplayStatus()
    {
        _characterName.text = _playerStatus.CharacterName;
        _hpSlider.value = (float)_playerStatus.Hp / _playerStatus.MaxHp;
        _hpText.text = _playerStatus.Hp.ToString() + "/" + _playerStatus.MaxHp.ToString();
        _mpSlider.value = (float)_playerStatus.Mp / _playerStatus.MaxMp;
        _mpText.text = _playerStatus.Mp.ToString() + "/" + _playerStatus.MaxMp.ToString();
        _motSlider.value = (float)_playerStatus.Mot / _playerStatus.MaxMot;
        _motText.text = _playerStatus.Mot.ToString() + "/" + _playerStatus.MaxMot.ToString();
    }

    //　敵ステータス表示
    public void DisplayEnemyStatus()
    {
        _characterName.text = _enemyStatus.CharacterName;
        _hpSlider.value = (float)_enemyStatus.Hp / _enemyStatus.MaxHp;
        //_hpText.text = _playerStatus.Hp.ToString() + "/" + _playerStatus.MaxHp.ToString();
        _mpSlider.value = (float)_enemyStatus.Mp / _enemyStatus.MaxMp;
        //_mpText.text = _playerStatus.Mp.ToString() + "/" + _playerStatus.MaxMp.ToString();
        _motSlider.value = (float)_enemyStatus.Mot / _enemyStatus.MaxMot;
        //_motText.text = _playerStatus.Mot.ToString() + "/" + _playerStatus.MaxMot.ToString();
    }

    //　ステータス更新
    public void UpdateStatus(Status status, int destinationValue)
    {
        if (status == Status.HP)
        {
            _hpSlider.value = (float)destinationValue / _playerStatus.MaxHp;
            _hpText.text = destinationValue.ToString() + "/" + _playerStatus.MaxHp.ToString();
        }
        else if (status == Status.MP)
        {
            _mpSlider.value = (float)destinationValue / _playerStatus.MaxMp;
            _mpText.text = destinationValue.ToString() + "/" + _playerStatus.MaxMp.ToString();
        }
        else if (status == Status.Mot)
        {
            _motSlider.value = (float)destinationValue / _playerStatus.MaxMot;
            _motText.text = destinationValue.ToString() + "/" + _playerStatus.MaxMot.ToString();
        }
    }

    //　敵ステータス更新
    public void UpdateEnemyStatus(Status status, int destinationValue)
    {
        if (status == Status.HP)
        {
            _hpSlider.value = (float)destinationValue / _enemyStatus.MaxHp;
            //_hpText.text = destinationValue.ToString() + "/" + _enemyStatus.MaxHp.ToString();
        }
        else if (status == Status.MP)
        {
            _mpSlider.value = (float)destinationValue / _enemyStatus.MaxMp;
            //_mpText.text = destinationValue.ToString() + "/" + _enemyStatus.MaxMp.ToString();
        }
        else if (status == Status.Mot)
        {
            _motSlider.value = (float)destinationValue / _enemyStatus.MaxMot;
            //_motText.text = destinationValue.ToString() + "/" + _enemyStatus.MaxMot.ToString();
        }
    }
}

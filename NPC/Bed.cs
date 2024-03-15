using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : MonoBehaviour
{
    [SerializeField]
    private GameObject _selecter;
    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _selecter.SetActive(false);
        _gameManager = GameManager.gameManager;
    }

    void OnTriggerStay(Collider col)
    {
        if (col.tag == "Player" && _gameManager.AllGameFlags["Chapter1_ed_2"] == true && _gameManager.IsSleep == false)
        {
            _selecter.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
        {
            _selecter.SetActive(false);
        }
    }
}

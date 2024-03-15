using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selected : MonoBehaviour
{
    [SerializeField]
    private GameObject _selecter;

    // Start is called before the first frame update
    void Start()
    {
        _selecter.SetActive(false);
    }

    void OnTriggerStay(Collider col)
    {
        if (col.tag == "Player")
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

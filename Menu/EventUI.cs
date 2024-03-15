using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EventUI : MonoBehaviour
{
    public void StartActiveFalse()
    {
        StartCoroutine(DisableObjectAfterDelay());
    }

    // �C�x���gUI��\��
    private IEnumerator DisableObjectAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
}

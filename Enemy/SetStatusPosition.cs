using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetStatusPosition : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        //　カメラと同じ向きに設定
        transform.rotation = Camera.main.transform.rotation;
    }
}

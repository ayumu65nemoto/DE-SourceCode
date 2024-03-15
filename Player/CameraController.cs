using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform _target;    //Transform型をいれる
    [SerializeField]
    private float _distance = 9.0f;  //float型が入る
    [SerializeField]
    private float _xSpeed = 250.0f;
    [SerializeField]
    private float _ySpeed = 120.0f;
    [SerializeField]
    private float _yMinLimit = -45f;
    [SerializeField]
    private float _yMaxLimit = 85f;
    private float _x = 0.0f;
    private float _y = 0.0f;

    private bool _isActive = true;

    public Transform Target { get => _target; set => _target = value; }
    public float Distance { get => _distance; set => _distance = value; }
    public float XSpeed { get => _xSpeed; set => _xSpeed = value; }
    public float YSpeed { get => _ySpeed; set => _ySpeed = value; }
    public float YMinLimit { get => _yMinLimit; set => _yMinLimit = value; }
    public float YMaxLimit { get => _yMaxLimit; set => _yMaxLimit = value; }
    public bool IsActive { get => _isActive; set => _isActive = value; }

    // Start is called before the first frame update
    void Start()
    {
        var angles = transform.eulerAngles;     //このゲームオブジェクト（カメラ）の角度
        _x = angles.y;   //「y」の値を取得（代入）
        _y = angles.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (Target != null && IsActive == true)
        {
            _x += Input.GetAxis("Mouse X") * XSpeed * 0.02f;
            _y -= Input.GetAxis("Mouse Y") * YSpeed * 0.02f;

            _y = ClampAngle(_y, YMinLimit, YMaxLimit);

            var rotation = Quaternion.Euler(_y, _x, 0);
            var position = rotation * new Vector3(0.0f, 0.0f, -Distance) + Target.position;

            //実際にゲーム内に反映
            transform.rotation = rotation;
            transform.position = position;
        }
    }

    static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360) { angle += 360; }
        if (angle > 360) { angle -= 360; }
        return Mathf.Clamp(angle, min, max);
    }
}

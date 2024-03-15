using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EffectNumericalDisplayScript : MonoBehaviour
{
    public enum NumberType
    {
        Damage,
        Heal,
        StatusUp,
        StatusDown
    }

    //　ダメージポイント表示用プレハブ
    [SerializeField]
    private GameObject _damagePointText;
    //　回復ポイント表示用プレハブ
    [SerializeField]
    private GameObject _healingPointText;
    //　ステータスアップポイント表示用プレハブ
    [SerializeField]
    private GameObject _statusUpPointText;
    //　ステータスダウンポイント表示用プレハブ
    [SerializeField]
    private GameObject _statusDownPointText;
    //　ポイントの表示オフセット値
    [SerializeField]
    private Vector3 offset = new Vector3(0f, 0.8f, -0.5f);

    public void InstantiatePointText(NumberType numberType, Transform target, int point)
    {
        var rot = Quaternion.LookRotation(target.position - Camera.main.transform.position);
        if (numberType == NumberType.Damage)
        {
            var pointTextIns = Instantiate<GameObject>(_damagePointText, target.position + offset, rot);
            pointTextIns.GetComponent<TextMeshPro>().text = point.ToString();
            Destroy(pointTextIns, 3f);
        }
        else if (numberType == NumberType.Heal)
        {
            var pointTextIns = Instantiate<GameObject>(_healingPointText, target.position + offset, rot);
            pointTextIns.GetComponent<TextMeshPro>().text = point.ToString();
            Destroy(pointTextIns, 3f);
        }
    }
}

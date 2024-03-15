using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateEnemy : MonoBehaviour
{
    //　地面のゲームオブジェクト
	[SerializeField]
    private Terrain _terrain;
    //　モンスターの最大数
	[SerializeField]
    private int _maxNum;
    //　モンスターのプレハブ
	[SerializeField]
    private GameObject[] _monsters;
    //　他のキャラとの距離
	[SerializeField]
    private float _radius;

    private void Start()
    {
		InstantiateEnemy();
    }

    public void InstantiateEnemy () {
 
		//　配置する敵の親のゲームオブジェクトを生成する
		GameObject parentObj = new GameObject("Enemys");
 
		//　配置する最大数分繰り返し
		for (int i = 0; i < _maxNum; i++) {
 
			//　インスタンス化が成功したかどうか？
			bool check = false;
			RaycastHit hit;
 
			//　ランダム値を入れる変数
			float randX;
			float randZ;
			//　敵や主人公と位置が重なったらカウントする数字
			int count = 0;
 
			//　敵の配置が出来たか、繰り返しが3回を越えたら終了
			while(!check && count < 3) {
				//　Terrainのサイズに合わせてランダム値を作成
				//randX = Random.Range (_terrain.GetPosition ().x, _terrain.GetPosition ().x + _terrain.terrainData.size.x);
				//randZ = Random.Range (_terrain.GetPosition ().z, _terrain.GetPosition ().z + _terrain.terrainData.size.z);
				randX = Random.Range (0, 10);
				randZ = Random.Range (0, 10);

				//　Terrainと接触した位置を探す
				if (Physics.Raycast (new Vector3(randX, _terrain.GetPosition ().y + _terrain.terrainData.size.y, randZ), Vector3.down, out hit, _terrain.GetPosition ().y + _terrain.terrainData.size.y + 100f, LayerMask.GetMask ("Field"))) {
					//　Player、Monster、Blockという名前のレイヤーと接触してなければ地面の接触ポイントに敵を配置
					if (!Physics.SphereCast (new Vector3(randX, _terrain.GetPosition ().y + _terrain.terrainData.size.y, randZ), _radius, Vector3.down, out hit, _terrain.GetPosition ().y + _terrain.terrainData.size.y + 100f, LayerMask.GetMask ("Player", "Monster"))) {
						GameObject tempObj = Instantiate (_monsters [Random.Range (0, _monsters.Length)], hit.point+new Vector3(randX, 1, randZ), Quaternion.identity) as GameObject;
						tempObj.transform.SetParent (parentObj.transform);
						check = true;
					} else {
						count++;
					}
				}
			}
		}
	}
}

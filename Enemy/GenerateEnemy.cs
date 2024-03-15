using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateEnemy : MonoBehaviour
{
    //�@�n�ʂ̃Q�[���I�u�W�F�N�g
	[SerializeField]
    private Terrain _terrain;
    //�@�����X�^�[�̍ő吔
	[SerializeField]
    private int _maxNum;
    //�@�����X�^�[�̃v���n�u
	[SerializeField]
    private GameObject[] _monsters;
    //�@���̃L�����Ƃ̋���
	[SerializeField]
    private float _radius;

    private void Start()
    {
		InstantiateEnemy();
    }

    public void InstantiateEnemy () {
 
		//�@�z�u����G�̐e�̃Q�[���I�u�W�F�N�g�𐶐�����
		GameObject parentObj = new GameObject("Enemys");
 
		//�@�z�u����ő吔���J��Ԃ�
		for (int i = 0; i < _maxNum; i++) {
 
			//�@�C���X�^���X���������������ǂ����H
			bool check = false;
			RaycastHit hit;
 
			//�@�����_���l������ϐ�
			float randX;
			float randZ;
			//�@�G���l���ƈʒu���d�Ȃ�����J�E���g���鐔��
			int count = 0;
 
			//�@�G�̔z�u���o�������A�J��Ԃ���3����z������I��
			while(!check && count < 3) {
				//�@Terrain�̃T�C�Y�ɍ��킹�ă����_���l���쐬
				//randX = Random.Range (_terrain.GetPosition ().x, _terrain.GetPosition ().x + _terrain.terrainData.size.x);
				//randZ = Random.Range (_terrain.GetPosition ().z, _terrain.GetPosition ().z + _terrain.terrainData.size.z);
				randX = Random.Range (0, 10);
				randZ = Random.Range (0, 10);

				//�@Terrain�ƐڐG�����ʒu��T��
				if (Physics.Raycast (new Vector3(randX, _terrain.GetPosition ().y + _terrain.terrainData.size.y, randZ), Vector3.down, out hit, _terrain.GetPosition ().y + _terrain.terrainData.size.y + 100f, LayerMask.GetMask ("Field"))) {
					//�@Player�AMonster�ABlock�Ƃ������O�̃��C���[�ƐڐG���ĂȂ���Βn�ʂ̐ڐG�|�C���g�ɓG��z�u
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

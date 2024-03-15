using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
	[SerializeField]
	private SoundManager _soundManager;
	[SerializeField]
	private AudioClip _startBGM;

    private void Start()
    {
		_soundManager.PlayBGM(_startBGM);
    }
    //　スタートボタンを押したら実行する
    public void StartGameButton()
	{
		SceneManager.LoadScene("Map1");
	}
}

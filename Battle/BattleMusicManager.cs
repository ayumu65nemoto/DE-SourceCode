using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMusicManager : MonoBehaviour
{
    private AudioSource _audioSource;
    //Å@êÌì¨èIóπå„ÇÃBGM
    [SerializeField]
    private AudioClip _bgmAfterTheBattle;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void ChangeBGM()
    {
        _audioSource.Stop();
        _audioSource.loop = false;
        _audioSource.clip = _bgmAfterTheBattle;
        _audioSource.Play();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager soundManager;
    [SerializeField]
    private AudioSource _audioBGM;
    [SerializeField]
    private AudioSource _audioSE;
    [SerializeField]
    private AudioClip _selectSE;

    private void Awake()
    {
        if (soundManager == null)
        {
            soundManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayBGM(AudioClip audioClip)
    {
        _audioBGM.clip = audioClip;
        _audioBGM.Play();
    }

    public void PlaySE(AudioClip audioClip)
    {
        _audioSE.clip = audioClip;
        _audioSE.Play();
    }

    //�@�J�[�\���I�������ۂ�SE�i���������̂őS����AudioClip�A�^�b�`����̖ʓ|�Łc�j
    public void PlaySelectSE()
    {
        _audioSE.clip = _selectSE;
        _audioSE.Play();
    }
}

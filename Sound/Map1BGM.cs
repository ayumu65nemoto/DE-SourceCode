using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map1BGM : MonoBehaviour
{
    [SerializeField]
    private SoundManager _soundManager;
    [SerializeField]
    private AudioClip _map1BGM;

    // Start is called before the first frame update
    void Start()
    {
        _soundManager = SoundManager.soundManager;
        _soundManager.PlayBGM(_map1BGM);
    }
}

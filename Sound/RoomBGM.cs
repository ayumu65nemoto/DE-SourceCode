using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBGM : MonoBehaviour
{
    [SerializeField]
    private SoundManager _soundManager;
    [SerializeField]
    private AudioClip _roomBGM;

    // Start is called before the first frame update
    void Start()
    {
        _soundManager = SoundManager.soundManager;
        _soundManager.PlayBGM(_roomBGM);
    }
}

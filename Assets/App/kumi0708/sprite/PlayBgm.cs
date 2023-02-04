using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBgm : MonoBehaviour
{
    [SerializeField]
    private string playBgmString = "rpg_01_loop";
    [SerializeField]
    private bool isLoop = true;
    [SerializeField]
    private float volume = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.PlayBGM(playBgmString, isLoop, volume);
    }
}

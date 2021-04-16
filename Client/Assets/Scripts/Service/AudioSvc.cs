using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSvc : MonoBehaviour
{
    public static AudioSvc Instance = null;
    public AudioSource bgAudio;
    public AudioSource uiAudio;

    public void InitSvc()
    {
        Instance = this;
    }

    public void PlayBGMusic(string name, bool isLoop=true)
    {
        AudioClip audio = ResSvc.Instance.LoadAudio(name, isLoop);
        
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class ResSvc : MonoBehaviour
{
    public static ResSvc Instance = null;
    public void InitSvc()
    {
        Instance = this;
    }

    private Action prgCB = null;
    public void AsyncLoadScene(string name,Action loaded)
    {
        GameRoot.Instance.loadingWnd.SetWndState();
        AsyncOperation sceneAsync = SceneManager.LoadSceneAsync(name);
        prgCB = () =>
        {
            float progress = sceneAsync.progress;
            GameRoot.Instance.loadingWnd.SetProgress(progress);
            if (progress == 1)
            {
                if (loaded != null)
                    loaded();
                prgCB = null;
                sceneAsync = null;
                GameRoot.Instance.loadingWnd.SetWndState(false);
            }
        };

    }

    void Update()
    {
        if (prgCB != null)
        {
            prgCB();
        }
    }

    private Dictionary<string, AudioClip> adDic = new Dictionary<string, AudioClip>();
    public AudioClip LoadAudio(string path, bool cache = false)
    {
        AudioClip au = null;
        if (!adDic.TryGetValue(path, out au))
        {
            au = Resources.Load<AudioClip>(path);
            if (cache)
            {
                adDic.Add(path,au);
            }
        }
        return au;
    }
}

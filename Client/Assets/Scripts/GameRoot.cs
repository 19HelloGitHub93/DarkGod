using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoot : MonoBehaviour
{
    public static GameRoot Instance = null;
    
    public LoadingWnd loadingWnd;
    
    void Start()
    {
        Instance = this;
        DontDestroyOnLoad(this);
        Init();
        
    }

    void Init()
    {
        ResSvc resSvc = GetComponent<ResSvc>();
        resSvc.InitSvc();
        AudioSvc audioSvc = GetComponent<AudioSvc>();
        audioSvc.InitSvc();

        LoginSys loginSys = GetComponent<LoginSys>();
        loginSys.InitSys();

        loginSys.EnterLogin();
    }
}

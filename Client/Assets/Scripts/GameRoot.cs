using System.Collections;
using System.Collections.Generic;
using PEProtocol;
using UnityEngine;

public class GameRoot : MonoBehaviour
{
    public static GameRoot Instance = null;
    
    public LoadingWnd loadingWnd;
    public DynamicWnd dynamicWnd;
    
    public PlayerData playerData { get; private set; }
    void Start()
    {
        Instance = this;
        DontDestroyOnLoad(this);
        ClearUIRoot();
        Init();
    }

    private void ClearUIRoot()
    {
        Transform canvas = transform.Find("Canvas");
        for (int i = 0; i < canvas.childCount; i++)
        {
            canvas.GetChild(i).gameObject.SetActive(false);
        }
        
        dynamicWnd.SetWndState();
    }

    void Init()
    {
        NetSvc netSvc = GetComponent<NetSvc>();
        netSvc.InitSvc();
        ResSvc resSvc = GetComponent<ResSvc>();
        resSvc.InitSvc();
        AudioSvc audioSvc = GetComponent<AudioSvc>();
        audioSvc.InitSvc();

        LoginSys loginSys = GetComponent<LoginSys>(); 
        loginSys.InitSys();

        loginSys.EnterLogin();
    }

    public static void AddTips(string tips)
    {
        Instance.dynamicWnd.AddTips(tips);
    }

    public void SetPlayerData(RspLogin data)
    {
        playerData = data.playerData;
    }

    public void SetPlayerName(string name)
    {
        playerData.name = name;
    }
}

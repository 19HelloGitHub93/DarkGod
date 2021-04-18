using System.Collections;
using System.Collections.Generic;
using PEProtocol;
using UnityEngine;

public class LoginSys : SystemRoot
{
    public static LoginSys Instance = null;
    
    public LoginWnd loginWnd;
    public CreateWnd createWnd;
    public override void InitSys()
    {
        base.InitSys();
        Instance = this;
    }

    public void EnterLogin()
    {
        resSvc.AsyncLoadScene(Constants.SceneLogin, () =>
        {
            loginWnd.SetWndState();
            audioSvc.PlayBGMusic(Constants.BGLogin);
        });
    }

    public void RspLogin(GameMsg msg)
    {
        GameRoot.AddTips("登录成功");
        GameRoot.Instance.SetPlayerData(msg.rspLogin);
        if (msg.rspLogin.playerData.name == "")
        {
            createWnd.SetWndState();
        }
        else
        {
            
        }
        loginWnd.SetWndState(false);
    }

    public void RspRename(GameMsg msg)
    {
        GameRoot.Instance.SetPlayerName(msg.rspRename.name);
        
        
        createWnd.SetWndState(false);
    }
}

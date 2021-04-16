using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginSys : SystemRoot
{
    public static LoginSys Instance = null;
    public LoginWnd loginWnd;
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
}

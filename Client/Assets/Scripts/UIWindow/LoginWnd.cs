using System;
using System.Collections;
using System.Collections.Generic;
using PEProtocol;
using UnityEngine;
using UnityEngine.UI;

public class LoginWnd : WindowRoot
{
    public InputField iptAcct;
    public InputField iptPass;
    public Button btnEnter;
    public Button btnNotice;
    
    private const string AcctKey = "Acct";
    private const string PassKey = "Pass";
    
    protected override void InitWnd()
    {
        base.InitWnd();
        
        if (PlayerPrefs.HasKey(AcctKey) && PlayerPrefs.HasKey(PassKey))
        {
            iptAcct.text = PlayerPrefs.GetString(AcctKey);
            iptPass.text = PlayerPrefs.GetString(PassKey);
        }
        else
        {
            iptAcct.text = "";
            iptPass.text = "";
        }
    }

    public void ClickEnterBtn()
    {
        audioSvc.PlayUIAduio(Constants.UILoginBtn);
        string acct = iptAcct.text;
        string pass = iptPass.text;
        if (acct != "" && pass != "")
        {
            PlayerPrefs.SetString(AcctKey,acct);
            PlayerPrefs.SetString(PassKey,pass);

            GameMsg msg = new GameMsg
            {
                cmd = (int)CMD.ReqLogin,
                reqLogin = new ReqLogin
                {
                    acct = acct,
                    pass = pass
                }
            };
            netSvc.SendMsg(msg);
        }
        else
        {
            GameRoot.AddTips("账号或密码为空");
        }
    }

    public void ClickNoticeBtn()
    {
        audioSvc.PlayUIAduio(Constants.UIClickBtn);
        GameRoot.AddTips("功能正在开发中...");
    }
}

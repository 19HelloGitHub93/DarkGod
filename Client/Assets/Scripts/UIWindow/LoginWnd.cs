using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginWnd : WindowRoot
{
    public InputField iptAcct;
    public InputField iptPass;
    public Button btnEnter;
    public Button btnNotice;
    
    protected override void InitWnd()
    {
        base.InitWnd();
        
        if (PlayerPrefs.HasKey("Acct") && PlayerPrefs.HasKey("Pass"))
        {
            iptAcct.text = PlayerPrefs.GetString("Acct");
            iptPass.text = PlayerPrefs.GetString("Pass");
        }
        else
        {
            iptAcct.text = "";
            iptPass.text = "";
        }
    }
}

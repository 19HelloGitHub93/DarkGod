using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginSys : MonoBehaviour
{
    public static LoginSys Instance = null;
    public LoginWnd loginWnd;
    public void InitSys()
    {
        Instance = this;
    }

    public void EnterLogin()
    {
        ResSvc.Instance.AsyncLoadScene(Constants.SceneLogin, () =>
        {
            loginWnd.SetWndState();
        });
    }
}

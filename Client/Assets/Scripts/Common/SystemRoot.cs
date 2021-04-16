using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemRoot : MonoBehaviour
{
    protected ResSvc resSvc;
    protected AudioSvc audioSvc;

    public virtual void InitSys()
    {
        resSvc = ResSvc.Instance;
        audioSvc = AudioSvc.Instance;
    }
}

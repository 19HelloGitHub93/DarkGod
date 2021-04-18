using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using PEProtocol;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using LogType = PEProtocol.LogType;

public class ResSvc : MonoBehaviour
{
    public static ResSvc Instance = null;
    public void InitSvc()
    {
        Instance = this;
        InitRDNameCfg();
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

    #region InitCfgs
    private List<string> surnameList = new List<string>();
    private List<string> manList = new List<string>();
    private List<string> womanList = new List<string>();
    private void InitRDNameCfg()
    {
        TextAsset xml = Resources.Load<TextAsset>(PathDefine.RDNameCfg);
        if (!xml)
        {
            PECommon.Log("xml file:"+PathDefine.RDNameCfg+"not exist",LogType.Error);
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);

            XmlNodeList nodeList = doc.SelectSingleNode("root").ChildNodes;
            for (int i = 0; i < nodeList.Count; i++)
            {
                XmlElement ele = nodeList[i] as XmlElement;
                if(ele.GetAttributeNode("ID")==null)
                    continue;
                int id = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText) ;
                foreach (XmlElement e in nodeList[i].ChildNodes)
                {
                    switch (e.Name)
                    {
                       case "surname":
                           surnameList.Add(e.InnerText);
                           break;
                       case "man":
                           manList.Add(e.InnerText);
                           break;
                       case "woman":
                           womanList.Add(e.InnerText);
                           break;
                    }
                }
            }
        }
    }
    public string GetRDNameData(bool man=true)
    {
        System.Random rd =new System.Random();
        string rdName = surnameList[PETools.RDInt(0, surnameList.Count - 1, rd)];
        if (man)
            rdName += manList[PETools.RDInt(0, manList.Count - 1, rd)];
        else
            rdName += womanList[PETools.RDInt(0, womanList.Count - 1, rd)];
        
        return rdName;
    }
    
    #endregion
    
}

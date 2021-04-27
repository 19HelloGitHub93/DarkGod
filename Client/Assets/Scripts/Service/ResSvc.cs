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
        InitRDNameCfg(PathDefine.RDNameCfg);
        InitMapCfg(PathDefine.MapCfg);
        InitGuideCfg(PathDefine.GuideCfg);
        InitStrongCfg(PathDefine.StrongCfg);
    }

    private Action prgCB = null;

    public void AsyncLoadScene(string name, Action loaded)
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
                adDic.Add(path, au);
            }
        }

        return au;
    }

    private Dictionary<string, GameObject> goDic = new Dictionary<string, GameObject>();
    public GameObject LoadPrefab(string path, bool cache)
    {
        GameObject prefab = null;
        if (!goDic.TryGetValue(path, out prefab))
        {
            prefab = Resources.Load<GameObject>(path);
            if(cache)
                goDic.Add(path,prefab);
        }

        GameObject go = null;
        if (prefab)
            go = Instantiate(prefab);
        return go;
    }

    private Dictionary<string, Sprite> spDic = new Dictionary<string, Sprite>();
    public Sprite LoadSprite(string path,bool cache=false)
    {
        Sprite sp;
        if (!spDic.TryGetValue(path, out sp))
        {
            sp = Resources.Load<Sprite>(path);
            if(cache)
                spDic.Add(path,sp);
        }
        return sp;
    }

    #region InitCfgs

    private List<string> surnameList = new List<string>();
    private List<string> manList = new List<string>();
    private List<string> womanList = new List<string>();

    private void InitRDNameCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {
            PECommon.Log("xml file:" + path + "not exist", LogType.Error);
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);

            XmlNodeList nodeList = doc.SelectSingleNode("root").ChildNodes;
            for (int i = 0; i < nodeList.Count; i++)
            {
                XmlElement ele = nodeList[i] as XmlElement;
                if (ele.GetAttributeNode("ID") == null)
                    continue;
                int id = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
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

    public string GetRDNameData(bool man = true)
    {
        System.Random rd = new System.Random();
        string rdName = surnameList[PETools.RDInt(0, surnameList.Count - 1, rd)];
        if (man)
            rdName += manList[PETools.RDInt(0, manList.Count - 1, rd)];
        else
            rdName += womanList[PETools.RDInt(0, womanList.Count - 1, rd)];

        return rdName;
    }

    private Dictionary<int, MapCfg> mapCfgDataDic = new Dictionary<int, MapCfg>(); 
    private void InitMapCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {
            PECommon.Log("xml file:" + path + "not exist", LogType.Error);
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);

            XmlNodeList nodeList = doc.SelectSingleNode("root").ChildNodes;
            for (int i = 0; i < nodeList.Count; i++)
            {
                XmlElement ele = nodeList[i] as XmlElement;
                if (string.IsNullOrEmpty(ele.GetAttributeNode("ID").InnerText))
                    continue;
                int id = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
                MapCfg mapCfg = new MapCfg
                {
                    ID = id
                };

                foreach (XmlElement e in nodeList[i].ChildNodes)
                {
                    switch (e.Name)
                    {
                        case "mapName":
                            mapCfg.mapName = e.InnerText;
                            break;
                        case "sceneName":
                            mapCfg.sceneName = e.InnerText;
                            break;
                        case "mainCamPos":
                        {
                            string[] valArr = e.InnerText.Split(',');
                            mapCfg.mainCamPos = new Vector3(float.Parse(valArr[0]), float.Parse(valArr[1]),
                                float.Parse(valArr[2]));
                        }
                            break;
                        case "mainCamRote":
                        {
                            string[] valArr = e.InnerText.Split(',');
                            mapCfg.mainCamRote = new Vector3(float.Parse(valArr[0]), float.Parse(valArr[1]),
                                float.Parse(valArr[2]));
                        }
                            break;
                        case "playerBornPos":
                        {
                            string[] valArr = e.InnerText.Split(',');
                            mapCfg.playerBornPos = new Vector3(float.Parse(valArr[0]), float.Parse(valArr[1]),
                                float.Parse(valArr[2]));
                        }
                            break;
                        case "playerBornRote":
                        {
                            string[] valArr = e.InnerText.Split(',');
                            mapCfg.playerBornRote = new Vector3(float.Parse(valArr[0]), float.Parse(valArr[1]),
                                float.Parse(valArr[2]));
                        }
                            break;
                    }
                }
                
                mapCfgDataDic.Add(id,mapCfg);
            }
        }
    }

    public MapCfg GetMapCfgData(int id) 
    {
        MapCfg data;
        mapCfgDataDic.TryGetValue(id, out data);
        return data;
    }

    #region 自动引导配置

    private Dictionary<int, AutoGuideCfg> guideTaskDic = new Dictionary<int, AutoGuideCfg>();
    private void InitGuideCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {
            PECommon.Log("xml file:" + path + "not exist", LogType.Error);
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);

            XmlNodeList nodeList = doc.SelectSingleNode("root").ChildNodes;
            for (int i = 0; i < nodeList.Count; i++)
            {
                XmlElement ele = nodeList[i] as XmlElement;
                if (string.IsNullOrEmpty(ele.GetAttributeNode("ID").InnerText))
                    continue;
                int id = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
                AutoGuideCfg guideCfg = new AutoGuideCfg
                {
                    ID = id
                };

                foreach (XmlElement e in nodeList[i].ChildNodes)
                {
                    switch (e.Name)
                    {
                        case "npcID":
                            guideCfg.npcID = int.Parse(e.InnerText);
                            break;
                        case "dilogArr":
                            guideCfg.dilogArr = e.InnerText;
                            break;
                        case "actID":
                            guideCfg.actID = int.Parse(e.InnerText);
                            break;
                        case "coin":
                            guideCfg.coin = int.Parse(e.InnerText);
                            break;
                        case "exp":
                            guideCfg.exp = int.Parse(e.InnerText);
                            break;
                    }
                }
                
                guideTaskDic.Add(id,guideCfg);
            }
        }
    }

    public AutoGuideCfg GetAutoGuideCfg(int id)
    {
        AutoGuideCfg autoGuideCfg;
        guideTaskDic.TryGetValue(id, out autoGuideCfg);
        return autoGuideCfg;
    }

    #endregion

    #region 强化升级配置
    private Dictionary<int,Dictionary<int,StrongCfg>> strongDic = new Dictionary<int, Dictionary<int,StrongCfg>>();

    private void InitStrongCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {
            PECommon.Log("xml file:" + path + "not exist", LogType.Error);
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);

            XmlNodeList nodeList = doc.SelectSingleNode("root").ChildNodes;
            for (int i = 0; i < nodeList.Count; i++)
            {
                XmlElement ele = nodeList[i] as XmlElement;
                if (string.IsNullOrEmpty(ele.GetAttributeNode("ID").InnerText))
                    continue;
                int id = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
                StrongCfg strongCfg = new StrongCfg
                {
                    ID = id
                };

                foreach (XmlElement e in nodeList[i].ChildNodes)
                {
                    int val = int.Parse(e.InnerText);
                    switch (e.Name)
                    {
                        case "pos":
                            strongCfg.pos = val;
                            break;
                        case "starlv":
                            strongCfg.startlv = val;
                            break;
                        case "addhp":
                            strongCfg.addhp = val;
                            break;
                        case "addhurt":
                            strongCfg.addhurt = val;
                            break;
                        case "adddef":
                            strongCfg.adddef = val;
                            break;
                        case "minlv":
                            strongCfg.minlv = val;
                            break;
                        case "coin":
                            strongCfg.coin = val;
                            break;
                        case "crytal":
                            strongCfg.crystal = val;
                            break;
                    }
                }

                Dictionary<int, StrongCfg> dic = null;
                if (strongDic.TryGetValue(strongCfg.pos, out dic))
                {
                    dic.Add(strongCfg.startlv,strongCfg);
                }
                else
                {
                    dic = new Dictionary<int, StrongCfg>();
                    dic.Add(strongCfg.startlv,strongCfg);
                    strongDic.Add(strongCfg.pos,dic);
                }
                
            }
        }
    }
    #endregion

    #endregion

    public int GetPropAddValPreLv(int currentIndex, int nextStartLv, int p2)
    {
        return 0;
    }

    public StrongCfg GetStrongData(int pos, int starlv)
    {
        StrongCfg sd = null;
        Dictionary<int, StrongCfg> dic = null;
        if (strongDic.TryGetValue(pos, out dic))
        {
            dic.TryGetValue(starlv, out sd);
        }
        return sd;
    }
}
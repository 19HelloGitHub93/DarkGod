using System;
using System.Collections;
using System.Collections.Generic;
using PEProtocol;
using UnityEngine;
using UnityEngine.AI;

public class MainCitySys : SystemRoot
{
    public static MainCitySys Instance = null;
    public MainCityWnd mainCityWnd;
    public InfoWnd infoWnd;
    public GuideWnd guideWnd;
    
    private PlayerController playerCtl;
    private Transform charCamTrans;
    private AutoGuideCfg curtTaskData;
    private Transform[] npcPosTrans;
    private NavMeshAgent nav;
    
    public override void InitSys()
    {
        base.InitSys();
        Instance = this;
    }

    public void EnterMainCity()
    {
        MapCfg mapData = resSvc.GetMapCfgData(Constants.MainCityMapID);
        resSvc.AsyncLoadScene(mapData.sceneName, () =>
        {
            PECommon.Log("enter maincity...");
            LoadPlayer(mapData);
            
            mainCityWnd.SetWndState();
            audioSvc.PlayBGMusic(Constants.BGMainCity);
            
            GameObject map = GameObject.FindGameObjectWithTag("MapRoot");
            MainCityMap mcm = map.GetComponent<MainCityMap>();
            npcPosTrans = mcm.NpcPosTrans;

            if (charCamTrans)
            {
                charCamTrans.gameObject.SetActive(false);
            }
        });
    }

    private void LoadPlayer(MapCfg mapData)
    {
        GameObject player = resSvc.LoadPrefab(PathDefine.AssissnCityPlayerPrefab,true);
        player.transform.position = mapData.playerBornPos;
        player.transform.localEulerAngles = mapData.playerBornRote;
        player.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        
        Camera.main.transform.position = mapData.mainCamPos;
        Camera.main.transform.localEulerAngles = mapData.mainCamRote;

        playerCtl = player.GetComponent<PlayerController>();
        playerCtl.Init();
        nav = player.GetComponent<NavMeshAgent>();

    }

    public void SetMoveDir(Vector2 dir)
    {
        StopNavTask();
        
        if (dir == Vector2.zero)
        {
            playerCtl.SetBlend(Constants.BlendIdle);
            playerCtl.isMove = false;
        }
        else
        {
            playerCtl.SetBlend(Constants.BlendWalk);
            playerCtl.isMove = true;
        }

        playerCtl.dir = dir;
    }

    public void OpenInfoWnd()
    {
        StopNavTask();
        
        if (!charCamTrans)
        {
            charCamTrans = GameObject.FindGameObjectWithTag("CharShowCam").transform;
        }

        charCamTrans.localPosition = playerCtl.transform.position + playerCtl.transform.forward * 3.8f+new Vector3(0,1.2f,0);
        charCamTrans.localEulerAngles = new Vector3(0, 180 + playerCtl.transform.localEulerAngles.y, 0);
        charCamTrans.localScale = Vector3.one;
        charCamTrans.gameObject.SetActive(true);
        
        infoWnd.SetWndState();
    }

    public void CloseInfoWnd()
    {
        if (charCamTrans)
        {
            charCamTrans.gameObject.SetActive(false);
            infoWnd.SetWndState(false);
        }
    }

    private float startRotate = 0;
    public void SetStartRotate()
    {
        startRotate = playerCtl.transform.localEulerAngles.y;
    }
    public void SetPlayerRotate(float rota)
    {
        playerCtl.transform.localEulerAngles = new Vector3(0, startRotate + rota, 0);
    }

    #region Guide Wnd

    private bool isNavGuide = false;
    public void RunTask(AutoGuideCfg agc)
    {
        if (agc != null)
        {
            curtTaskData = agc;
        }

        nav.enabled = true;
        if (curtTaskData.npcID != -1)
        {
            float dis = Vector3.Distance(playerCtl.transform.position, npcPosTrans[agc.npcID].position);
            if (dis < 0.5f)
            {
                isNavGuide = false;
                nav.isStopped = true;
                playerCtl.SetBlend(Constants.BlendIdle);
                nav.enabled = false;
                OpenGuideWnd();
            }
            else
            {
                isNavGuide = true;
                nav.enabled = true;
                nav.speed = Constants.PlayerMoveSpeed;
                nav.SetDestination(npcPosTrans[agc.npcID].position);
                playerCtl.SetBlend(Constants.BlendWalk);
            }
        }
        else
        {
            OpenGuideWnd();
        }
    }

    private void Update()
    {
        if (isNavGuide)
        {
            isArriveNavPos();
            playerCtl.SetCam();
        }
    }

    private void isArriveNavPos()
    {
        float dis = Vector3.Distance(playerCtl.transform.position, npcPosTrans[curtTaskData.npcID].position);
        if (dis < 0.5f)
        {
            isNavGuide = false;
            nav.isStopped = true;
            playerCtl.SetBlend(Constants.BlendIdle);
            nav.enabled = false;
            OpenGuideWnd();
        }
    }

    private void StopNavTask()
    {
        if (isNavGuide)
        {
            isNavGuide = false;
            nav.isStopped = true;
            nav.enabled = false;
            playerCtl.SetBlend(Constants.BlendIdle);
        }
    }

    private void OpenGuideWnd()
    {
        guideWnd.SetWndState();
    }

    public AutoGuideCfg getCurtTaskData()
    {
        return curtTaskData;
    }

    public void RspGuide(GameMsg msg)
    {
        RspGuide data = msg.rspGuide;
        GameRoot.AddTips(Constants.Color("任务奖励 金币+" + curtTaskData.coin + " 经验+" + curtTaskData.exp,TxtColor.Blue));
        switch (curtTaskData.actID)
        {
            case 0:
                
                break;
            case 1:
                
                break;
            case 2:
                
                break;
            case 3:
                
                break;
            case 4:
                
                break;
            case 5:
                
                break;
        }
        GameRoot.Instance.SetPlayerDataByGuide(data);
        mainCityWnd.RefreshUI();
    }

    #endregion
}

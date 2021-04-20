using System.Collections;
using System.Collections.Generic;
using PEProtocol;
using UnityEngine;

public class MainCitySys : SystemRoot
{
    public static MainCitySys Instance = null;
    public MainCityWnd mainCityWnd;
    private PlayerController playerCtl;
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
        
    }

    public void SetMoveDir(Vector2 dir)
    {
        if (dir == Vector2.zero)
        {
            playerCtl.SetBlend(Constants.BlendIdle);
        }
        else
        {
            playerCtl.SetBlend(Constants.BlendWalk);
        }

        playerCtl.dir = dir;
    }
}

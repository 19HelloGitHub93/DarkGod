﻿using System.Collections;
using System.Collections.Generic;
using PEProtocol;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainCityWnd : WindowRoot
{
    #region UIDefine

    public Image imgTouch;
    public Image imgDirBg;
    public Image imgDirPoint;
    
    public Animation menuAni;
    public Button btnMenu;
    
    public Text txtFight;
    public Text txtPower;
    public Image imgPowerPrg;
    public Text txtLevel;
    public Text txtName;
    public Text txtExpPrg;
    public Transform expPrgTrans;
    public Button btnGuide;
    #endregion

    private bool menuState = true;
    private float pointDis;
    private Vector2 startPos = Vector2.zero;
    private Vector2 defaultPos = Vector2.zero;
    private AutoGuideCfg curtTaskData;

    #region MainFunctions

    protected override void InitWnd()
    {
        base.InitWnd();
        pointDis = Screen.height * 1.0f / Constants.ScreenStandardHeight * Constants.ScreenOPDis;
        defaultPos = imgDirBg.transform.position;
        SetActive(imgDirPoint,false);
        RegisterTouchEvts();
        RefreshUI();
    }

    public void RefreshUI()
    {
        PlayerData pd = GameRoot.Instance.playerData;
        SetText(txtFight,PECommon.GetFightByProps(pd));
        SetText(txtPower,"体力:"+pd.power+"/"+PECommon.GetPowerLimit(pd.lv));
        imgPowerPrg.fillAmount = pd.power * 1.0f / PECommon.GetPowerLimit(pd.lv);
        SetText(txtLevel,pd.lv);
        SetText(txtName,pd.name);

        #region expprg

        int expPrgVal = (int)(pd.exp*1.0f/PECommon.GetExpUpValByLv(pd.lv)*100);
        SetText(txtExpPrg,expPrgVal+"%");
        int index = expPrgVal/10;

        GridLayoutGroup grid = expPrgTrans.GetComponent<GridLayoutGroup>();
        float globalRate = 1.0F * Constants.ScreenStandardHeight / Screen.height;
        float screenWidth = Screen.width * globalRate;
        float width = (screenWidth - 180) / 10;
        grid.cellSize = new Vector2(width, 7);

        for (int i = 0; i < expPrgTrans.childCount; i++)
        {
            Image img = expPrgTrans.GetChild(i).GetComponent<Image>();
            if (i < index)
            {
                img.fillAmount = 1;
            }
            else if(i==index)
            {
                img.fillAmount = expPrgVal % 10 * 1.0f / 10;
            }
            else
            {
                img.fillAmount = 0;
            }
        }

        #endregion

        curtTaskData = resSvc.GetAutoGuideCfg(pd.guideid);
        if (curtTaskData!=null)
        {
            SetGuideBtnIcon(curtTaskData.npcID);
        }else
            SetGuideBtnIcon(-1);
    }

    private void SetGuideBtnIcon(int npcId)
    {
        string spPath = "";
        Image img = btnGuide.GetComponent<Image>();
        switch (npcId)
        {
            case Constants.NPCWiseMan:
                spPath = PathDefine.WiseManHead;
                break;
            case Constants.NPCArtisan:
                spPath = PathDefine.ArtisanHead;
                break;
            case Constants.NPCGeneral:
                spPath = PathDefine.GeneralHead;
                break;
            case Constants.NPCTrader:
                spPath = PathDefine.TraderHead;
                break;
            default:
                spPath = PathDefine.TaskHead;
                break;
        }
        SetSprite(img,spPath);
    }

    #endregion

    #region ClickEvts

    public void ClickGuideBtn()
    {
        audioSvc.PlayUIAduio(Constants.UIClickBtn);
        if (curtTaskData!=null)
        {
            MainCitySys.Instance.RunTask(curtTaskData);
        }
        else
        {
            GameRoot.AddTips("更多引导任务,正在开发中...");
        }
    }
    public void ClickMenuBtn()
    {
        audioSvc.PlayUIAduio(Constants.UIExtenBtn);
        menuState = !menuState;
        AnimationClip clip = null;
        if (menuState)
            clip = menuAni.GetClip("OpenMCMenu");
        else
            clip = menuAni.GetClip("CloseMCMenu");

        menuAni.Play(clip.name);
    }
    
    public void ClickHeadBtn()
    {
       audioSvc.PlayUIAduio(Constants.UIOpenPage);
       MainCitySys.Instance.OpenInfoWnd();
    }

    public void RegisterTouchEvts()
    {
        OnClickDown(imgTouch.gameObject, (evt) =>
        {
            startPos = evt.position;
            SetActive(imgDirPoint);
            imgDirBg.transform.position = evt.position;
        });
        
        OnClickUp(imgTouch.gameObject, (evt) =>
        {
            imgDirBg.transform.position = defaultPos;
            SetActive(imgDirPoint,false);
            imgDirPoint.transform.localPosition = Vector2.zero;
            MainCitySys.Instance.SetMoveDir(Vector2.zero);
        });
        
        OnDrag(imgTouch.gameObject, (evt) =>
        {
            Vector2 dir = evt.position - startPos;
            float len = dir.magnitude;
            if (len>pointDis)
            {
                Vector2 clampDir = Vector2.ClampMagnitude(dir,pointDis);
                imgDirPoint.transform.position = startPos + clampDir;
            }
            else
            {
                imgDirPoint.transform.position = evt.position;
            }
            MainCitySys.Instance.SetMoveDir(dir.normalized);
        });
    }
    #endregion
    
}

using System.Collections;
using System.Collections.Generic;
using PEProtocol;
using UnityEngine;
using UnityEngine.UI;

public class InfoWnd : WindowRoot
{
   #region UI Define

   public RawImage imgChar;
   
   public Text txtInfo;
   public Text txtExp;
   public Image imgExpPrg;
   public Text txtPower;
   public Image imgPowerPrg;

   public Text txtJob;
   public Text txtFight;
   public Text txtHP;
   public Text txtHurt;
   public Text txtDef;

   public Transform transDetail;
   public Text dtxhp;
   public Text dtxad;
   public Text dtxap;
   public Text dtxaddef;
   public Text dtxapdef;
   public Text dtxdodge;
   public Text dtxpierce;
   public Text dtxcritical;
   
   #endregion

   private Vector2 startPos;
   protected override void InitWnd()
   {
      base.InitWnd();
      RefreshUI();
      SetActive(transDetail,false);
      RegTouchEvts();
   }

   private void RegTouchEvts()
   {
      OnClickDown(imgChar.gameObject, (evt) =>
      {
         startPos = evt.position;
         MainCitySys.Instance.SetStartRotate();
      });
      
      OnDrag(imgChar.gameObject, (evt) =>
      {
         float roate = -(evt.position.x - startPos.x) * 0.4f;
         MainCitySys.Instance.SetPlayerRotate(roate);
      });
   }

   private void RefreshUI()
   {
      PlayerData playerData = GameRoot.Instance.playerData;
      SetText(txtInfo,playerData.name+" LV."+playerData.lv);
      SetText(txtExp,playerData.exp+"/"+PECommon.GetExpUpValByLv(playerData.lv));
      imgExpPrg.fillAmount = playerData.exp * 1.0F / PECommon.GetExpUpValByLv(playerData.lv);
      SetText(txtPower,playerData.power+"/"+PECommon.GetPowerLimit(playerData.lv));
      imgExpPrg.fillAmount = playerData.power * 1.0F / PECommon.GetPowerLimit(playerData.lv);
      
      SetText(txtJob," 职业   暗夜刺客");
      SetText(txtFight," 战力   "+PECommon.GetFightByProps(playerData));
      SetText(txtHP," 血量   "+playerData.hp);
      SetText(txtHurt," 伤害   "+(playerData.ad+playerData.ap));
      SetText(txtDef," 防御   "+(playerData.addef+playerData.apdef));
      
      SetText(dtxhp,playerData.hp);
      SetText(dtxad,playerData.ad);
      SetText(dtxap,playerData.ap);
      SetText(dtxaddef,playerData.addef);
      SetText(dtxapdef,playerData.apdef);
      SetText(dtxdodge,playerData.dodge+"%");
      SetText(dtxpierce,playerData.pierce+"%");
      SetText(dtxcritical,playerData.critical+"%");
   }

   public void ClickCloseBtn()
   {
      audioSvc.PlayUIAduio(Constants.UIClickBtn);
      MainCitySys.Instance.CloseInfoWnd();
   }

   public void ClickDetailBtn()
   {
      audioSvc.PlayUIAduio(Constants.UIClickBtn);
      SetActive(transDetail);
   }

   public void ClickCloseDetailBtn()
   {
      audioSvc.PlayUIAduio(Constants.UIClickBtn);
      SetActive(transDetail,false);
   }
}

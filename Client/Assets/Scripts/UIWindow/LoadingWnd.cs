using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingWnd : WindowRoot
{
    public Text txtTips;
    public Image imgFg;
    public Image imgPoint;
    public Text txtPrg;

    private float fgWidth;

    protected override void InitWnd()
    {
        base.InitWnd();
        
        fgWidth = imgFg.rectTransform.rect.width;
        SetText(txtTips,"这是一条游戏tips");
        
        SetText(txtTips,"0%");
        imgFg.fillAmount = 0;
        imgPoint.rectTransform.anchoredPosition = new Vector2(fgWidth * -0.5f, 0);
    }

    public void SetProgress(float prg)
    {
        SetText(txtTips,(int) (prg * 100) + "%");
        imgFg.fillAmount = prg;

        float posX = prg * fgWidth - (fgWidth * 0.5f);
        imgPoint.rectTransform.anchoredPosition = new Vector2(posX, 0);
    }
}

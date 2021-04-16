using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicWnd : WindowRoot
{
    public Animation tipsAni;
    public Text txtTips;

    protected override void InitWnd()
    {
        base.InitWnd();
        SetActive(txtTips,false);
    }

    public void SetTips(string tips)
    {
        SetActive(txtTips);
        SetText(txtTips,tips);

        AnimationClip clip = tipsAni.GetClip("TipShowAni");
        tipsAni.Play();

        StartCoroutine(AniPlayDone(clip.length, () =>
        {
            SetActive(txtTips,false);
        }));
    }

    private IEnumerator AniPlayDone(float sec, Action cb)
    {
        yield return new WaitForSeconds(sec);
        if (cb != null)
        {
            cb();
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Transform camTrans;
    private Vector3 camOffset;
    private Animator ani;
    private CharacterController ctrl;
    
    public Vector2 dir = Vector2.zero;

    private float targetBlend;
    private float currentBlend;
    public bool isMove = false;

    public void Init()
    {
        camTrans = Camera.main.transform;
        camOffset = transform.position - camTrans.position;
        ani = GetComponent<Animator>();
        ctrl = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // float h = Input.GetAxis("Horizontal");
        // float v = Input.GetAxis("Vertical");
        //
        // dir = new Vector2(h, v).normalized;
        
        if(currentBlend!=targetBlend)
            UpdateMixBlend();
        if (isMove)
        {
            if (dir != Vector2.zero)
            {
                SetDir();
                SetMove();
                SetCam();
                SetBlend(Constants.BlendWalk);
            }
            else
            {
                SetBlend(Constants.BlendIdle);
            }
        }
    }

    private void SetDir()
    {
        float angle = Vector2.SignedAngle(dir,Vector2.up)+camTrans.eulerAngles.y;
        Vector3 eulerAngles = new Vector3(0,angle,0);
        transform.localEulerAngles = eulerAngles;
    }

    private void SetMove()
    {
        ctrl.Move(transform.forward * Time.deltaTime * Constants.PlayerMoveSpeed);
    }

    public void SetCam()
    {
        if (camTrans != null)
            camTrans.position = transform.position - camOffset;
    }

    private float cu;
    public void SetBlend(float blend)
    {
        targetBlend = blend;
    }

    private void UpdateMixBlend()
    {
        if (Mathf.Abs(currentBlend - targetBlend) < Constants.AccelerSpeed * Time.deltaTime)
        {
            currentBlend = targetBlend;
        }else if (currentBlend > targetBlend)
        {
            currentBlend -= Constants.AccelerSpeed * Time.deltaTime;
        }
        else
        {
            currentBlend += Constants.AccelerSpeed * Time.deltaTime;
        }
        ani.SetFloat("Blend",currentBlend);
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//在其他脚本前先执行
[DefaultExecutionOrder(-100)]
public class PlayerInput : MonoBehaviour
{
    #region 操作
    [HideInInspector] public float horizontal;
    [HideInInspector] public bool jumpPressed;
    [HideInInspector] public bool jumpHeld;
	[HideInInspector] public bool attackPressed;
    //准备重置输入
    bool readyToClearInput;

    void ProcessInput()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        jumpPressed = jumpPressed || Input.GetButtonDown("Jump");
        jumpHeld = jumpHeld || Input.GetButton("Jump");
		attackPressed = Input.GetButtonDown("Fire1");
    }

    void ClearInput()
    {
        horizontal = 0;
        jumpPressed = false;
        jumpHeld = false;
        attackPressed = false;
    }

    #endregion

    void Update()
    {
        if(readyToClearInput)
        {
            readyToClearInput = false;

            ClearInput();
        }

        ProcessInput();
    }

    void FixedUpdate()
    {
        readyToClearInput = true;
    }

}
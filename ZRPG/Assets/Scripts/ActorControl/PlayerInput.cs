using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//在其他脚本前先执行
[DefaultExecutionOrder(-100)]
public class PlayerInput : MonoBehaviour
{
    //使用手柄
    public bool isJoystick;

    void FixedUpdate()
    {
        readyToClearInput = true;
    }

    void Update()
    {
        if(readyToClearInput)
        {
            readyToClearInput = false;
            ClearInput();
        }

        ProcessInput();
    }

    #region 操作

    [HideInInspector] public float horizontal;
    [HideInInspector] public bool jumpPressed;
    [HideInInspector] public bool jumpReleased;
    [HideInInspector] public bool attackPressed;

    //准备重置输入
    bool readyToClearInput;

    void ProcessInput()
    {
        //按下手柄键
        if(Input.GetKeyDown(KeyCode.Joystick1Button0) ||
            Input.GetAxis("Joystick L Trigger") != 0)
        {
            isJoystick = true;
        }

        if (isJoystick)
            horizontal = Input.GetAxis("Joystick L Trigger");
        else
            horizontal = Input.GetAxisRaw("Horizontal");

        jumpPressed = Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Joystick1Button0);
        jumpReleased = Input.GetButtonUp("Jump") || Input.GetKeyUp(KeyCode.Joystick1Button0);
        attackPressed = Input.GetButtonDown("Fire1");

        ////手柄按下菜单键
        //if(Input.GetKeyUp(KeyCode.Joystick1Button11))
    }

    void ClearInput()
    {
        horizontal = 0;
        jumpPressed = false;
        jumpReleased= false;
        attackPressed = false;
    }

    #endregion
}

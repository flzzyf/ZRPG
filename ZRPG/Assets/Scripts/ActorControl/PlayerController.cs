using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	Actor actor;
    PlayerInput input;

	void Start()
    {
		actor = GetComponent<Actor>();
        input = GetComponent<PlayerInput>();
	}

	void FixedUpdate()
    {
        //移动
        actor.Move(input.horizontal);

        if(input.jumpPressed)
        {
            actor.Jump();
        }

        //松开空格，停止跳跃
        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Joystick1Button0))
		{
			actor.JumpCancel();
		}

		if(Input.GetButtonDown("Fire1"))
		{
			actor.Attack();
		}

        if (Input.GetKeyDown(KeyCode.Q))
        {
            actor.abils[0].Cast(actor);

            actor.animator.SetTrigger("Spell1");
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            actor.abils[1].Cast(actor);

            actor.animator.SetTrigger("Spell2");
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            actor.abils[2].Cast(actor);

            actor.animator.SetTrigger("Spell3");
        }

        //	inputH = Input.GetAxis("Joystick L Trigger");
        //if (inputH != 0)
        //	print(inputH);

        ////手柄按下菜单键
        //if(Input.GetKeyUp(KeyCode.Joystick1Button11))
        //{

        //}
    }
}

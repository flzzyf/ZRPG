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

		CameraFollow.instance.Follow(transform);
	}

	void FixedUpdate()
    {
		//移动
		actor.Move(input.horizontal);

        //按下跳跃
        if(input.jumpPressed)
        {
            actor.Jump();
        }

        //松开跳跃键
        if (input.jumpReleased)
		{
			actor.JumpCancel();
		}

        //按下攻击键
		if(input.attackPressed)
		{
			actor.Attack();
		}

        //按下QWE
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //actor.abils[0].Cast(actor);

            //actor.animator.SetTrigger("Spell1");
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            //actor.abils[1].Cast(actor);

            //actor.animator.SetTrigger("Spell2");
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            //actor.abils[2].Cast(actor);

            //actor.animator.SetTrigger("Spell3");
        }

    }

	void Attack()
	{
		print("Attack");
	}


}

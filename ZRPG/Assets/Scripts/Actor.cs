﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//游戏人物
public class Actor : MonoBehaviour
{
	Controller2D controller2D;

	public GameObject gfx;
	[HideInInspector] public Animator animator;

    public Panel_ActorInfo panel_ActorInfo;

	[Header("人物属性")]
	public float speed;

    public List<Abil> abils;

	void Start()
    {
		controller2D = GetComponent<Controller2D>();
		animator = gfx.GetComponent<Animator>();

        InitHp();

		InitJumpVelocity();
    }

	void Update()
	{
		//滑墙判定
		UpdateWallSlide();
	}

    public void Move(float inputH)
    {
        controller2D.SetHorizontalVelocity(inputH * speed);


    }

    #region 生命值

    public int hpMax = 5;
    [HideInInspector]
	public int hpCurrent;

	void InitHp()
	{
		SetHp(hpMax);
	}

	public void SetHp(int amount)
	{
		hpCurrent = amount;

        if (panel_ActorInfo != null)
            panel_ActorInfo.Set(this);
	}
	public void ModifyHp(int amount)
	{
        SetHp(hpCurrent += amount);

		if (hpCurrent <= 0)
		{
			Death();
		}
	}

	public virtual void TakeDamage(int amount)
	{
		//播放被击动画
		animator.SetTrigger("Hit");

		ModifyHp(-amount);
	}

	public virtual void Death()
	{
	}

	public void DieAnimEvent()
	{
		Destroy(gameObject);
	}
	#endregion

	#region 跳跃

	[Header("跳跃参数")]
	//跳跃高度和跳跃时间
	public float jumpHeight = 4;
	public float timeToJump = .4f;

	//松开空格后的跳跃高度
	public float jumpHeightMin = 2;

	float jumpVelocity;
	float jumpVelocityMin;


	//根据跳跃高度和时间换算出跳跃初速度
	void InitJumpVelocity()
	{
		jumpVelocity = (jumpHeight - 0.5f * Controller2D.gravity * timeToJump * timeToJump) / timeToJump;
		jumpVelocityMin = (jumpHeightMin - 0.5f * Controller2D.gravity * timeToJump * timeToJump) / timeToJump;
	}

	public void Jump()
	{
		if (wallSliding)
		{
			float inputH = Input.GetAxisRaw("Horizontal");
			int wallDir = (controller2D.collisions.left) ? -1 : 1;

			if (wallDir == inputH)  //按朝着墙的方向
			{
				controller2D.velocity.x = -wallDir * wallJumpClimb.x;
				controller2D.velocity.y = wallJumpClimb.y;
			}
			else if (inputH == 0)   //不按方向键
			{
				controller2D.velocity.x = -wallDir * wallJumpOff.x;
				controller2D.velocity.y = wallJumpOff.y;
			}
			else   //按和墙反方向键
			{
				controller2D.velocity.x = -wallDir * wallLeap.x;
				controller2D.velocity.y = wallLeap.y;
			}
		}
		else
		{
			//接触地面才能跳
			if(controller2D.collisions.below)
				controller2D.velocity.y += jumpVelocity;
		}
	}

	//松开空格，停止跳跃
	public void JumpCancel()
	{
		controller2D.velocity.y = Mathf.Min(controller2D.velocity.y, jumpVelocityMin);
	}

	#endregion

	#region 滑墙

	[Header("滑墙参数")]
	//滑墙相关
	public bool canSlideWall;

	public float wallSlideSpeedMax = 1;

	public Vector2 wallJumpClimb;
	public Vector2 wallJumpOff;
	public Vector2 wallLeap;

	public float wallStickTime = .25f;
	float timeToWallUnstick;

	public bool wallSliding;

	void UpdateWallSlide()
	{
		if (canSlideWall)
		{
			int wallDir = (controller2D.collisions.left) ? -1 : 1;

			wallSliding = false;
			if ((controller2D.collisions.left || controller2D.collisions.right) && !controller2D.collisions.below)
			{
				wallSliding = true;

				//jumpCount = unit.jumpCountMax;

				//滑墙时下落速度限制
				controller2D.velocity.y = Mathf.Max(controller2D.velocity.y, -wallSlideSpeedMax);

				if (timeToWallUnstick > 0)
				{
					controller2D.velocityXSmoothing = 0;
					controller2D.velocity.x = 0;

					if (Mathf.Sign(controller2D.velocity.x) != wallDir && controller2D.velocity.x != 0)
					{
						timeToWallUnstick -= Time.deltaTime;

					}
					else
					{
						timeToWallUnstick = wallStickTime;
					}
				}
				else
				{
					timeToWallUnstick = wallStickTime;
				}
			}
		}
	}
	#endregion

	#region 近身攻击

	[Header("攻击属性")]
	public Vector2 attackOffset;
	public float attackRadius;

	void ShowAttackRange()
	{
		Gizmos.DrawWireSphere((Vector2)transform.position + attackOffset, attackRadius);
	}

	//发动攻击
	public void Attack()
	{
		Vector2 pos = (Vector2)transform.position + attackOffset * new Vector2(controller2D.facingDir, 0);
		Collider2D[] colliders = Physics2D.OverlapCircleAll(pos, attackRadius);

		foreach (var item in colliders)
		{
			if(item.CompareTag("Enemy"))
			{
				print("Hit");
				item.GetComponent<Actor>().TakeDamage(1);
			}
		}
	}

	#endregion

	private void OnDrawGizmosSelected()
	{
		ShowAttackRange();
	}
}
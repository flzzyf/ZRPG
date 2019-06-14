using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//游戏人物
public class Actor : MonoBehaviour
{
    [HideInInspector] public RigidbodyBox rigidbodyBox;

	public GameObject gfx;
	[HideInInspector] public Animator animator;

    public Panel_ActorInfo panel_ActorInfo;

	[Header("人物属性")]
	public float speed;

    public List<Abil> abils;

    void Start()
    {
        rigidbodyBox = GetComponent<RigidbodyBox>();
		animator = gfx.GetComponent<Animator>();

        InitHp();

		InitJumpVelocity();

        //初始化动画事件
        animEvent = new UnityEvent();
    }

	void Update()
	{
		//滑墙判定
		UpdateWallSlide();
	}

    public void Move(float inputH)
    {
        if (isStuned)
        {
            rigidbodyBox.movingForce.x = 0;
            animator.SetFloat("Speed", 0);

            return;
        }

        //播放移动动画
        animator.SetFloat("Speed", Mathf.Abs(inputH));

        //设置移动
        rigidbodyBox.movingForce.x = inputH * speed;

        if (inputH != 0 &&
            facingDir != Mathf.Sign(inputH))
            Flip();

    }

    #region 朝向

    public bool facingRight;

    public int facingDir { get { return facingRight ? 1 : -1; } }

    public void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        facingRight = !facingRight;
    }
    #endregion

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

	public void Death()
	{
        //Destroy(gameObject);

        gameObject.SetActive(false);

        BattleManager.instance.StartCoroutine(BattleManager.instance.Respawn(this));
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
		jumpVelocity = (jumpHeight - 0.5f * rigidbodyBox.gravity * timeToJump * timeToJump) / timeToJump;

		jumpVelocityMin = (jumpHeightMin - 0.5f * rigidbodyBox.gravity * timeToJump * timeToJump) / timeToJump;
	}

	public void Jump()
	{
        if (isStuned)
            return;

        if (wallSliding)
		{
			float inputH = Input.GetAxisRaw("Horizontal");
			int wallDir = (rigidbodyBox.collisions.left) ? -1 : 1;

			if (wallDir == inputH)  //按朝着墙的方向
			{
				rigidbodyBox.velocity.x = -wallDir * wallJumpClimb.x;
				rigidbodyBox.velocity.y = wallJumpClimb.y;
			}
			else if (inputH == 0)   //不按方向键
			{
				rigidbodyBox.velocity.x = -wallDir * wallJumpOff.x;
				rigidbodyBox.velocity.y = wallJumpOff.y;
			}
			else   //按和墙反方向键
			{
				rigidbodyBox.velocity.x = -wallDir * wallLeap.x;
				rigidbodyBox.velocity.y = wallLeap.y;
			}
		}
		else
		{
            //接触地面才能跳
            if(rigidbodyBox.isOnGround)
            {
                rigidbodyBox.AddForce(Vector2.up * jumpVelocity);
            }
        }
	}

	//松开空格，停止跳跃
	public void JumpCancel()
	{
		rigidbodyBox.velocity.y = Mathf.Min(rigidbodyBox.velocity.y, jumpVelocityMin);
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
			int wallDir = (rigidbodyBox.collisions.left) ? -1 : 1;

			wallSliding = false;
			if ((rigidbodyBox.collisions.left || rigidbodyBox.collisions.right) && !rigidbodyBox.collisions.below)
			{
				wallSliding = true;

				//jumpCount = unit.jumpCountMax;

				//滑墙时下落速度限制
				rigidbodyBox.velocity.y = Mathf.Max(rigidbodyBox.velocity.y, -wallSlideSpeedMax);

				if (timeToWallUnstick > 0)
				{
					//rigidbodyBox.velocityXSmoothing = 0;
					rigidbodyBox.velocity.x = 0;

					if (Mathf.Sign(rigidbodyBox.velocity.x) != wallDir && rigidbodyBox.velocity.x != 0)
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
        animator.SetTrigger("Attack");

        animEvent.RemoveAllListeners();
        animEvent.AddListener(AttackAnimEvent);
	}

    public void AttackAnimEvent()
    {
        Vector2 pos = (Vector2)transform.position + attackOffset * new Vector2(facingDir, 0);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(pos, attackRadius);

        foreach (var item in colliders)
        {
            if (item.CompareTag("Enemy"))
            {
                item.GetComponent<Actor>().TakeDamage(1);

                //施力
                Vector2 force = new Vector2(facingDir, .5f) * 5;
                item.GetComponent<RigidbodyBox>().AddForce(force);

                item.GetComponent<Actor>().Stun(.7f);

            }
        }
    }


    #endregion

    #region 昏迷状态

    //处于无法控制状态
    public bool isStuned;

    float currentStunTime;

    //昏迷几秒
    public void Stun(float duration)
    {
        currentStunTime = Mathf.Max(currentStunTime, duration);

        StopCoroutine(StunTimeCounter(0));
        StartCoroutine(StunTimeCounter(duration));
    }

    //昏迷时间计时器
    IEnumerator StunTimeCounter(float duration)
    {
        isStuned = true;

        yield return new WaitForSeconds(duration);

        isStuned = false;
    }

    #endregion

    #region 动画事件

    //动画触发事件
    UnityEvent animEvent;

    //触发动画事件
    public void TriggerAnimEvent()
    {
        animEvent.Invoke();
    }

    #endregion

    private void OnDrawGizmosSelected()
	{
		ShowAttackRange();
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Rigidbody2D的精简版，所有物体都用到
[RequireComponent(typeof(BoxCollider2D))]
public class Controller2D : MonoBehaviour
{
	[HideInInspector]
    public Vector3 velocity;

    //空中地上加速度
    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;

	[HideInInspector]
    public float velocityXSmoothing;

	//人物朝向，-1左1右
	public bool facingRight;
	public int facingDir { get { return facingRight ? 1 : -1; } set { facingRight = value > 0; } }

	void Start()
    {
		boxCollider = GetComponent<BoxCollider2D>();

		//计算光束间距
		CalculateRaySpacing();

        //InitGravity();
    }

	void Update()
	{
		//计算重力速度
		UpdateGravity(ref velocity);

		//移动
		Move(velocity * Time.deltaTime);
	}

	//设置水平速度
	public void SetHorizontalVelocity(float amount)
	{
		velocity.x = Mathf.SmoothDamp(velocity.x, amount, ref velocityXSmoothing,
			(collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
	}

	//每帧自动移动
	public void Move(Vector3 _velocity, bool _standingOnPlatform = false)
    {
        //更新四个方向的光束源点
        UpdateRaycastOrigins();
        //重置碰撞
        collisions.Reset();
        //之前速度
        collisions.velocityOld = _velocity;

		//设置移动方向
        if(_velocity.x != 0)
        {
            facingDir = (int)Mathf.Sign(_velocity.x);
        }

        //下坡
        if (_velocity.y < 0)
            DescendSlope(ref _velocity);

		//水平碰撞判定（为了爬墙，即便是0也判断）
		HorizontalCollisions(ref _velocity);

		//垂直碰撞判定
		if (_velocity.y != 0)
		{
			VerticalCollisions(ref _velocity);
		}

		//上下方有物体时重置y速度
		if (collisions.above || collisions.below)
		{
			velocity.y = 0;
		}

		//print("当前速度：" + velocity);

		//移动
		transform.Translate(_velocity);

        if (_standingOnPlatform)
            collisions.below = _standingOnPlatform;

    }


	#region 水平和竖直碰撞判定
	//水平碰撞判定
	void HorizontalCollisions(ref Vector3 _velocity)
    {
        //速度正负方向
        float directionX = facingDir;
        //光束长度
        float rayLength = Mathf.Abs(_velocity.x) + skinWidth;

        if(Mathf.Abs(_velocity.x) < skinWidth)
        {
            rayLength = 2 * skinWidth;  //刚好能检测到相邻物体的距离
        }

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigin.bottomLeft : raycastOrigin.bottomRight;

            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Utils.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            //碰到物体
            if (hit)
            {
                if (hit.distance == 0)
                    continue;

                //坡角度
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (i == 0 && slopeAngle <= maxClambAngle)
                {
                    if (collisions.descendingSlope)
                    {
                        collisions.descendingSlope = false;
                        _velocity = collisions.velocityOld;
                    }
                    float distanceToSlopeStart = 0;
                    //进入新的坡
                    if (slopeAngle != collisions.slopeAngleOld)
                    {
                        distanceToSlopeStart = hit.distance - skinWidth;
                        _velocity.x -= distanceToSlopeStart * directionX;
                    }
                    //爬坡

                    ClampSlope(ref _velocity, slopeAngle);
                    _velocity.x += distanceToSlopeStart * directionX;
                }
                //爬不动坡
                if(!collisions.clambingSlope || slopeAngle > maxClambAngle)
                {
                    //根据距离确定下一步移动距离
                    _velocity.x = (hit.distance - skinWidth) * directionX;
                    rayLength = hit.distance;
                    //在爬坡
                    if (collisions.clambingSlope)
                    {
                        _velocity.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(_velocity.x);
                    }

                    //若方向向左则为真
                    collisions.left = directionX == -1;
                    //向右
                    collisions.right = directionX == 1;
                }
            }

        }
    }

    //垂直碰撞判定
    void VerticalCollisions(ref Vector3 _velocity)
    {
        //速度正负方向
        float directionY = Mathf.Sign(_velocity.y);
        //光束长度
        float rayLength = Mathf.Abs(_velocity.y) + skinWidth;
        //遍历每道光束
        for (int i = 0; i < verticalRayCount; i++)
        {
            //根据速度方向确定光束方向
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigin.bottomLeft : raycastOrigin.topLeft;
            //光束源点偏移
            rayOrigin += Vector2.right * (verticalRaySpacing * i + _velocity.x);
            //发出光束
            RaycastHit2D hit = Utils.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            //有障碍物
            if (hit)
            {
				//如果是可穿越的平台，且人物朝上移动则无视该平台
				if (hit.collider.CompareTag("Platform") && _velocity.y > 0)
					continue;

                //当距离为0时速度也为0
                _velocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;
                //在爬墙
                if (collisions.clambingSlope)
                {
                    //上方有障碍限制x方向移动
                    _velocity.x = _velocity.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(_velocity.x);
                }

                //若方向向下则为真
                collisions.below = directionY == -1;
                //向右
                collisions.above = directionY == 1;
            }
        }
    }

	#endregion

	#region 上下坡

	//最大可攀爬角度
	public float maxClambAngle = 60;
	//最大下坡角度（超过会直接掉下来
	public float maxDescendAngle = 55;

	void ClampSlope(ref Vector3 velocity, float slopeAngle)
    {
        //在平地上应该移动的水平距离，若有障碍物则为0
        float moveDistance = Mathf.Abs(velocity.x);
        float climbVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

        if(velocity.y > climbVelocityY)
        {
            //print("Jumping");
        }
        else
        {
            //计算在斜坡上的xy轴移动距离
            velocity.y = climbVelocityY;
            velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
            //设置为接触地面
            collisions.below = true;
            collisions.clambingSlope = true;
            collisions.slopeAngle = slopeAngle;
        }
    }

    void DescendSlope(ref Vector3 _velocity)
    {
        //速度正负方向
        float directionX = Mathf.Sign(_velocity.x);

        Vector2 rayOrigin = (directionX == -1) ? raycastOrigin.bottomRight : raycastOrigin.bottomLeft;

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, Mathf.Infinity, collisionMask);

        if (hit)
        {
            //坡角度
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            //坡角在合适范围内
            if (slopeAngle != 0 && slopeAngle <= maxClambAngle)
            {
                //在下坡
                if(Mathf.Sign(hit.normal.x) == directionX)
                {
                    //
                    if(hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(_velocity.x))
                    {
                        float moveDistance = Mathf.Abs(_velocity.x);
                        float descendVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                        _velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(_velocity.x);
                        _velocity.y -= descendVelocityY;

                        collisions.slopeAngle = slopeAngle;
                        collisions.descendingSlope = true;
                        collisions.below = true;
                    }
                }

            }
        }
    }

	#endregion

	public void AddForce(Vector3 _dir, float _amount, float _time = 1)
    {
        Vector3 force = _dir.normalized;
        force *= _amount;

        //force *= transform.localScale.x;
        //forceVelocity = force;

    }

	#region 碰撞信息
	public CollisionInfo collisions;
    public struct CollisionInfo
	{
		//上下左右接触物体
        public bool above, below;
        public bool left, right;
		//正在上下坡
        public bool clambingSlope;
        public bool descendingSlope;
		//坡度
        public float slopeAngle, slopeAngleOld;

        public Vector3 velocityOld;

        public void Reset()
        {
            above = below = false;
            left = right = false;
            clambingSlope = false;
            descendingSlope = false;

            slopeAngleOld = slopeAngle;
            slopeAngle = 0;
        }
    }

	#endregion

	#region 射线碰撞判定

	//边缘厚度，一个很小的值
	public const float skinWidth = .015f;

	//水平和竖直的射线数量
	public int horizontalRayCount = 4;
	public int verticalRayCount = 4;
	//水平和竖直的射线间距
	float horizontalRaySpacing;
	float verticalRaySpacing;

	BoxCollider2D boxCollider;

	//碰撞层
	public LayerMask collisionMask;

	//四个角落的碰撞源点
	struct RaycastOrigin
	{
		public Vector2 topLeft, topRight;
		public Vector2 bottomLeft, bottomRight;
	}
	RaycastOrigin raycastOrigin;

	//更新四个方向的光束源点
	void UpdateRaycastOrigins()
	{
		//边缘扩大一定的厚度，这样射线不会搜索到自己
		Bounds bounds = boxCollider.bounds;
		bounds.Expand(skinWidth * -2);

		raycastOrigin.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
		raycastOrigin.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
		raycastOrigin.topLeft = new Vector2(bounds.min.x, bounds.max.y);
		raycastOrigin.topRight = new Vector2(bounds.max.x, bounds.max.y);
	}

	//计算光束间距
	void CalculateRaySpacing()
	{
		Bounds bounds = boxCollider.bounds;
		//边缘缩进一定的厚度
		bounds.Expand(skinWidth * -2);
		//至少两道光
		horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
		verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);
		//计算水平和垂直方向上光束的间距
		horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
		verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
	}
	#endregion

	#region 重力

	public static float gravity = -20;

	//计算重力
	void UpdateGravity(ref Vector3 velocity)
	{
		velocity.y += gravity * Time.deltaTime;
	}
	#endregion

}

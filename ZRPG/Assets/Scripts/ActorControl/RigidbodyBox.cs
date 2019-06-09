using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//最后执行
[DefaultExecutionOrder(100)]
public class RigidbodyBox : MonoBehaviour
{
    //该物体每帧的移动量
    [SerializeField]
    Vector2 velocity;

    BoxCollider2D boxCollider;

    public Vector2 movingForce;

    public float gravity = -1;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();

        //更新四个方向的光束源点
        CalculateRaySpacing();
    }

    void FixedUpdate()
    {
        //计算施加在该物体上的力
        CalculateForces(ref velocity);

        //对该物体施加它身上的（玩家移动力）
        ApplyForces((velocity + movingForce) * Time.fixedDeltaTime);

    }

    //计算施加在该物体上的力
    void CalculateForces(ref Vector2 v)
    {
        UpdateRaycastOrigins();
        collisions.Reset();

        //竖直射线判定，判定当前移动方向上有没有被阻挡
        VerticalRaycast(v);

        //如果上下有接触物体则设置y速度为0
        if (collisions.below || collisions.above)
            velocity.y = 0;

        //施加重力
        v.y += gravity * Time.fixedDeltaTime;

        //摩擦力



    }

    //对该物体施加它身上的力
    void ApplyForces(Vector2 v)
    {
        if(collisions.below && distanceToBelow != 0)
        {
            v.y = -distanceToBelow;
        }

        transform.Translate(v);
    }


    //竖直射线判定，输入物体移动量参数
    void VerticalRaycast(Vector2 v)
    {
        //速度正负方向，默认是负的
        int dir = v.y > 0 ? 1 : -1;
        //光束长度
        float rayLength = Mathf.Abs(v.y) + skinWidth;

        //遍历每道光束
        for (int i = 0; i < verticalRayCount; i++)
        {
            //根据速度方向确定光束方向
            Vector2 rayOrigin = (dir == -1) ? raycastOrigin.bottomLeft : raycastOrigin.topLeft;

            //光束源点偏移
            rayOrigin += Vector2.right * (verticalRaySpacing * i + v.x);

            //发出光束
            RaycastHit2D hit = Utils.Raycast(rayOrigin, Vector2.up * dir, rayLength, collisionMask);

            //有障碍物
            if (hit)
            {
                //print(hit.distance);
                //v.y = (hit.distance - skinWidth) * dir;

                //与底部的距离，如果不是0稍后会将y速度设为该距离
                distanceToBelow = hit.distance - skinWidth;

                rayLength = hit.distance;

                //若方向向下则为真
                collisions.below = dir == -1;
                //向右
                collisions.above = dir == 1;

                //return;
            }
        }
    }

    #region 射线碰撞判定

    //边缘厚度，一个很小的值
    public const float skinWidth = .015f;

    //水平和竖直的射线数量
    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;
    //水平和竖直的射线间距
    float horizontalRaySpacing;
    float verticalRaySpacing;

    //与底部的距离
    float distanceToBelow;

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
        //边缘缩小一定的厚度
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

    #region 碰撞信息
    public CollisionInfo collisions;
    public struct CollisionInfo
    {
        //上下左右接触物体
        public bool above, below;
        public bool left, right;

        public void Reset()
        {
            above = below = false;
            left = right = false;
        }
    }

    #endregion

}

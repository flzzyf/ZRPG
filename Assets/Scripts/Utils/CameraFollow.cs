using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : Singleton<CameraFollow>
{
    public Transform target;

    public float smoothSpeed = 0.125f;

    public Vector3 offset;

	void Start()
	{
		//如果有目标则立即移动到目标点
		if (target != null)
			MoveToTarget(target);
	}

	void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        transform.position = smoothedPosition;
    }

	//立即移动到目标点
	void MoveToTarget(Transform target)
	{
		Vector3 desiredPosition = target.position + offset;
		transform.position = desiredPosition;
	}

	//跟随目标
	public void Follow(Transform target)
	{
		this.target = target;

		MoveToTarget(target);
	}

}

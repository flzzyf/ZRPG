using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
	//判定射线是否射中物体
	public static RaycastHit2D Raycast(Vector2 origin, Vector2 dir, float distance, LayerMask layer)
	{
		RaycastHit2D hit = Physics2D.Raycast(origin, dir, distance, layer);

		Debug.DrawLine(origin, origin + dir * distance, Color.green);

        if(hit)
            Debug.DrawLine(origin, origin + dir * hit.distance, Color.red);

        return hit;
	}
}

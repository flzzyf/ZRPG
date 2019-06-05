using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
	//判定射线是否射中物体
	public static RaycastHit2D Raycast(Vector2 origin, Vector2 dir, float distance, LayerMask layer)
	{
		RaycastHit2D hit = Physics2D.Raycast(origin, dir, distance, layer);

		Color color = hit ? Color.red : Color.green;
		Debug.DrawLine(origin, origin + dir * distance, color);

		return hit;
	}

	public static void Push(Rigidbody2D rb, Vector2 dir, float amount)
	{
		rb.AddForce(dir * amount, ForceMode2D.Impulse);
	}
}

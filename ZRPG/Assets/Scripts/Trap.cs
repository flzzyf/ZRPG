using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
	public GameObject gfx;
	Animator animator;

	bool isTriggered;

	public int damage = 1;

	BoxCollider2D boxCollider2D;

	public LayerMask collisionMask;

	void Start()
	{
		animator = gfx.GetComponent<Animator>();
		boxCollider2D = GetComponent<BoxCollider2D>();
	}

	void Update()
	{
		if (isTriggered)
			return;

		Collider2D collider = Physics2D.OverlapBox((Vector2)transform.position + boxCollider2D.offset, boxCollider2D.size, 0, collisionMask);

		if(collider != null)
		{
			Trigger(collider.GetComponent<Actor>());

		}
	}

	//触发陷阱
	void Trigger(Actor actor)
	{
		isTriggered = true;

		animator.SetTrigger("Trigger");

		actor.TakeDamage(damage);
	}
}

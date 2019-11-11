using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CharacterMovement : MonoBehaviour
{
	[Header("Input")]
	public float horizontal;
	public bool wantsToJump;

	[Header("Settings")]
	public float speed;
	public float groundAcceleration;
	public float groundDeceleration;
	public float airAcceleration;
	public float airDeceleration;
	public float jumpHeight;

	[Header("Status")]
	public Vector2 velocity;
	public bool grounded;

	[Header("Collision")]
	public Collider2D[] hits;
	private BoxCollider2D boxCollider;

    // Start is called before the first frame update
    void Start()
    {
		boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
		PlayerInput();

		ModifyVelocity();

		Move();

		CollisionDetection();
		CollisionResolution();
    }

	void PlayerInput()
	{
		horizontal = Input.GetAxis("Horizontal");
		wantsToJump = Input.GetButtonDown("Jump");
	}

	void ModifyVelocity()
	{
		float acceleration = grounded ? groundAcceleration : airAcceleration;
		float deceleration = grounded ? groundDeceleration : airDeceleration;

		if (horizontal > 0 && velocity.x >= 0 || horizontal < 0 && velocity.x <= 0)
		{
			velocity.x = Mathf.MoveTowards(velocity.x, speed * horizontal, acceleration * Time.deltaTime);
		} else
		{
			velocity.x = Mathf.MoveTowards(velocity.x, 0, deceleration * Time.deltaTime);
		}

		if (grounded)
		{
			velocity.y = 0;

			if (wantsToJump)
			{
				velocity.y = Mathf.Sqrt(2 * jumpHeight * Mathf.Abs(Physics2D.gravity.y));
			}
		}

		velocity.y += Physics2D.gravity.y * Time.deltaTime;
	}

	void Move()
	{
		transform.Translate(velocity * Time.deltaTime);
	}

	void CollisionDetection()
	{
		hits = Physics2D.OverlapBoxAll(transform.position, boxCollider.size, 0);
	}

	void CollisionResolution()
	{
		grounded = false;

		foreach (Collider2D hit in hits)
		{
			if (hit == boxCollider)
				continue;

			ColliderDistance2D colliderDistance = hit.Distance(boxCollider);

			if (Vector2.Angle(colliderDistance.normal, Vector2.up) < 90 && velocity.y < 0)
			{
				grounded = true;
			}

			if (colliderDistance.isOverlapped)
			{
				Vector2 correction = colliderDistance.pointA - colliderDistance.pointB;
				transform.Translate(correction);
				velocity += correction / Time.deltaTime;
			}
		}
	}
}

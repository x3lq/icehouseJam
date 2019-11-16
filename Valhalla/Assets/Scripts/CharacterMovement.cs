using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CharacterMovement : MonoBehaviour
{
	[Header("Input")]
	public float horizontal;
	public float vertical;
	public bool wantsToJump;
	public bool wantsToDash;
	public bool wantsToBlink;
	private Vector2 dashDirection;

	[Header("Settings")]
	public float speed;
	public float groundAcceleration;
	public float groundDeceleration;
	public float airAcceleration;
	public float airDeceleration;
	public float jumpHeight;
	public float dashSpeed;
	public float dashDistance;
	public float blinkDelay;
	public LayerMask blinkCollisionLayer;
	public float blinkDistance;

	[Header("Status")]
	public Vector2 velocity;
	public bool grounded;
	public bool dashing;
	private float dashTimer;
	private float blinkTimer;

	[Header("Collision")] 
	public LayerMask collisionLayers;
	public Collider2D[] hits;
	private BoxCollider2D boxCollider;

	[Header("Weapon Status")] 
	private Axt axt;

	public Boolean wantsAxtJump;

	// Start is called before the first frame update
    void Start()
    {
		boxCollider = GetComponent<BoxCollider2D>();
		axt = GetComponent<Axt>();
    }

	// Update is called once per frame
	void Update()
	{
		if (axt.pullToAxt)
		{
			return;
		}
		
		PlayerInput();

		if (blinkTimer <= 0)
		{
			if (dashTimer <= 0)
			{
				dashing = false;
				ModifyVelocity();
			}
			else
			{
				dashing = true;
				DashModifyVelocity();
			}

			Move();

			if (wantsToBlink)
			{
				blinkTimer = blinkDelay;
			}
		}
		else
		{
			blinkTimer -= Time.deltaTime;

			if (blinkTimer <= 0)
			{
				Blink();
			}
		}

		CollisionDetection();
		CollisionResolution();
	}

	void PlayerInput()
	{
		horizontal = Input.GetAxis("Horizontal");
		vertical = Input.GetAxis("Vertical");
		wantsToJump = Input.GetButtonDown("Jump");
		wantsToDash = Input.GetMouseButtonDown(0);
		wantsToBlink = Input.GetMouseButtonDown(1);

		if (grounded && wantsToDash && horizontal != 0)
		{
			dashTimer = dashDistance / dashSpeed;
			dashDirection = new Vector2(horizontal, 0).normalized;
		}
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

			if (wantsToJump && !wantsToDash)
			{
				velocity.y = Mathf.Sqrt(2 * jumpHeight * Mathf.Abs(Physics2D.gravity.y));
			}
		}
		
		if (wantsAxtJump)
		{
			Debug.Log("AxtJump");
			wantsAxtJump = false;
			velocity.y = 0;
			velocity.y = Mathf.Sqrt(2 * jumpHeight * Mathf.Abs(Physics2D.gravity.y));
		}

		velocity.y += Physics2D.gravity.y * Time.deltaTime;
	}

	void DashModifyVelocity()
	{
		dashTimer -= Time.deltaTime;

		if (dashTimer > 0)
		{
			velocity.x = dashDirection.x * dashSpeed;
		} else
		{
			velocity.x = velocity.x/Mathf.Abs(velocity.x)*speed;
		}
	}

	void Move()
	{
		transform.Translate(velocity * Time.deltaTime);
	}

	void Blink()
	{
		Vector2 direction = new Vector2(horizontal, vertical).normalized;
		Vector2 blinkTargetPosition = transform.position + (Vector3)direction * blinkDistance;

		RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, blinkDistance, blinkCollisionLayer);
		
		if (hit)
		{
			Debug.Log("Hit");
			blinkTargetPosition = hit.point - direction;
		}

		transform.position = blinkTargetPosition;
		velocity = direction * speed;
	}

	void CollisionDetection()
	{
		hits = Physics2D.OverlapBoxAll(transform.position, boxCollider.size, 0, collisionLayers);
	}

	void CollisionResolution()
	{
		grounded = false;

		float x = velocity.x;

		foreach (Collider2D hit in hits)
		{
			if (hit == boxCollider)
				continue;
			
			ColliderDistance2D colliderDistance = hit.Distance(boxCollider);

			if (Vector2.Angle(colliderDistance.normal, Vector2.up) < 45 && velocity.y < 0)
			{
				grounded = true;
			}

			if (colliderDistance.isOverlapped)
			{
				Vector2 correction = colliderDistance.pointA - colliderDistance.pointB;
				transform.Translate(correction);
				velocity += correction / Time.deltaTime / 6;
			}
		}
	}
}

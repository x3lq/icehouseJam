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

	[Header("Status")]
	public Vector2 velocity;
	public bool grounded;
	public bool dashing;
	private float dashTimer;

	[Header("Collision")] 
	public LayerMask collisionLayers;
	public Collider2D[] hits;
	private BoxCollider2D boxCollider;

	[Header("Attack Status")] 
	public Boolean wantsToAttack;
	public Boolean wantsToHammer;
	public Boolean wantsToThrowSpeer;

	public Boolean blockedSpeer;
	public float speerTimer;

	public HandAttack handAttack;
	public Hammer hammer;
	public GameObject speerPrefab;
	
	[Header("Weapon Status")] 
	private Axt axt;

	public Boolean wantsAxtJump;

	// Start is called before the first frame update
    void Start()
    {
	    hammer = GetComponent<Hammer>();
	    handAttack = GetComponent<HandAttack>();
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

		if (wantsToAttack)
		{
			handAttack.attack = true;
		}

		if (wantsToHammer)
		{
			hammer.attack = true;
		}

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

		if (wantsToThrowSpeer && !blockedSpeer)
		{
			Speer speer = Instantiate(speerPrefab, transform.position, Quaternion.identity).GetComponent<Speer>();
			speer.throwSpeer(new Vector2(horizontal, vertical));
			blockedSpeer = true;
			StartCoroutine(unblockSpeerAfterTime());
		}
		wantsToThrowSpeer = false;

		Move();

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
		wantsToAttack = Input.GetButtonDown("Attack");
		wantsToHammer = Input.GetButtonDown("HammerAttack");
		wantsToThrowSpeer = Input.GetButtonDown("Speer");

		if (grounded && wantsToDash && horizontal != 0)
		{
			dashTimer = dashDistance / dashSpeed;
			dashDirection = new Vector2(horizontal, 0).normalized;
		}
	}

	void ModifyVelocity()
	{
		if (axt.active)
		{
			return;
		}
		
		float acceleration = grounded ? groundAcceleration : airAcceleration;
		float deceleration = grounded ? groundDeceleration : airDeceleration;

		if (horizontal > 0 && velocity.x >= 0 || horizontal < 0 && velocity.x <= 0)
		{
			velocity.x = Mathf.MoveTowards(velocity.x, speed * horizontal, acceleration * Time.deltaTime);
		} else
		{
			velocity.x = Mathf.MoveTowards(velocity.x, 0, deceleration * Time.deltaTime);
		}

		if (wantsToThrowSpeer)
		{
			velocity.x = 0;
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

	IEnumerator unblockSpeerAfterTime()
	{
		yield return new WaitForSeconds(speerTimer);
		blockedSpeer = false;
	}
}

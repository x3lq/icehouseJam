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
	public bool holdJump;
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
	private float lastGroundedY;
	public bool dashing;
	private float dashTimer;
	public float fallDistance;
	public bool landing;
	public bool lookingRight;

	[Header("Collision")] 
	public LayerMask collisionLayers;
	public Collider2D[] hits;
	private BoxCollider2D boxCollider;

	[Header("Attack Status")] 
	public Boolean wantsToAttack;
	public Boolean wantsToHammer;
	public bool isHammering;
	public Boolean wantsToThrowSpeer;
	public bool throwSpeer;

	public Boolean blockedSpeer;
	public float speerTimer;

	public HandAttack handAttack;
	public Hammer hammer;
	public GameObject speerPrefab;
	
	[Header("Weapon Status")] 
	private Axt axt;

	public Boolean wantsAxtJump;

	private CharacterHealth characterHealth;

	// Start is called before the first frame update
    void Start()
    {
	    hammer = GetComponent<Hammer>();
	    handAttack = GetComponent<HandAttack>();
		boxCollider = GetComponent<BoxCollider2D>();
		axt = GetComponent<Axt>();
		characterHealth = GetComponent<CharacterHealth>();
    }

	// Update is called once per frame
	void Update()
	{
		if (characterHealth.hasWon)
		{
			return;
		}
		
		if (axt.pullToAxt)
		{
			return;
		}

		if (landing)
		{
			velocity = Vector2.zero;
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
			isHammering = true;
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
			throwSpeer = true;
		}

		Move();

		CollisionDetection();
		CollisionResolution();

		CalculateFallDistance();

		if (velocity.x > 0)
		{
			lookingRight = true;
		}

		if (velocity.x < 0)
		{
			lookingRight = false;
		}
	}

	void PlayerInput()
	{
		horizontal = Input.GetAxis("Horizontal");
		vertical = Input.GetAxis("Vertical");
		wantsToJump = Input.GetButtonDown("Jump" + ControllerSelector.type);
		holdJump = Input.GetButton("Jump" + ControllerSelector.type);
		wantsToDash = Input.GetButtonDown("Dash" + ControllerSelector.type);
		wantsToAttack = Input.GetButtonDown("Attack" + ControllerSelector.type);
		wantsToHammer = Input.GetButtonDown("HammerAttack"+ ControllerSelector.type);

		if (ControllerSelector.type == "XBox")
		{
			wantsToThrowSpeer = Input.GetAxis("Speer"+ ControllerSelector.type) == 1;
		}
		
		if(ControllerSelector.type == "PS4")
		{
			wantsToThrowSpeer = Input.GetButtonDown("SpeerPS4");
		}

		if (wantsToDash && horizontal != 0)
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

		if (throwSpeer)
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

		if (velocity.y > 0 && !holdJump)
		{
			velocity.y += Physics2D.gravity.y * Time.deltaTime;
		}
	}

	void DashModifyVelocity()
	{
		dashTimer -= Time.deltaTime;

		if (dashTimer > 0)
		{
			velocity.x = dashDirection.x * dashSpeed;
		} else if (velocity.x != 0)
		{
			velocity.x = velocity.x / Mathf.Abs(velocity.x) * speed;
		}

		velocity.y = 0;
	}

	void Move()
	{
		if (isHammering && grounded)
		{
			velocity.x = 0;
		}

		velocity = Vector2.ClampMagnitude(velocity, 50);

		transform.Translate(velocity * Time.deltaTime);
	}

	void CollisionDetection()
	{
		if (dashTimer > 0)
		{
			boxCollider.size = new Vector2(1, boxCollider.size.y);
		} else
		{
			boxCollider.size = new Vector2(0.5f, boxCollider.size.y);
		}
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
				velocity.y = 0;
				lastGroundedY = transform.position.y;
			}
			else if (Vector2.Angle(colliderDistance.normal, Vector2.left) < 45 && velocity.x > 0 || Vector2.Angle(colliderDistance.normal, Vector2.right) < 45 && velocity.x < 0)
			{
				velocity.x = 0;
				dashTimer = 0;
			} else if (Vector2.Angle(colliderDistance.normal, Vector2.down) < 45 && velocity.y > 0)
			{
				velocity.y = 0;
			}

			if (colliderDistance.isOverlapped)
			{
				Vector2 correction = colliderDistance.pointA - colliderDistance.pointB;
				transform.Translate(correction);
				// velocity += correction / Time.deltaTime / 8;
			}
		}
	}

	void CalculateFallDistance()
	{
		if (!grounded)
		{
			fallDistance = lastGroundedY - transform.position.y;
		}
	}

	IEnumerator unblockSpeerAfterTime()
	{
		yield return new WaitForSeconds(speerTimer);

		if (ControllerSelector.type == "XBox")
		{
			blockedSpeer = Input.GetAxis("Speer" + ControllerSelector.type) == 1;

			if (blockedSpeer)
			{
				StartCoroutine(unblockSpeerAfterTime());
			}
		}
		else
		{
			blockedSpeer = false;
		}

	}

	void UnblockHammer()
	{
		isHammering = false;
	}

	public void onWin()
	{
		StartCoroutine(onWinTimer());
	}

	IEnumerator onWinTimer()
	{
		yield return new WaitForSeconds(0.8f);
		GetComponent<Animator>().SetTrigger("Death");
	}

	public void OnDeathFinished()
	{
		if (characterHealth.hasWon)
		{
			GameManager.instance.playerWon();
		}
	}

	public void ThrowSpeer()
	{
		if (horizontal == 0 && vertical == 0)
		{
			return;
		}

		Speer speer = Instantiate(speerPrefab, transform.position, Quaternion.identity).GetComponent<Speer>();
		speer.throwSpeer(new Vector2(horizontal, vertical).normalized);
		blockedSpeer = true;
		StartCoroutine(unblockSpeerAfterTime());
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GoblinBoss : MonoBehaviour
{
	private CharacterMovement character;
    public Vector3 originalPos;
    public GameObject leftHand;
    public GameObject rightHand;
	public bool active;

	private bool alive = true;

	[Header("Boss Properties")]
    public float health;
    public float rageHealth;
	public float wakeUpDistance;
    
    public Boolean rage;

    public String animationState;
    private Animator animator;

    [Header("Movement")]
	public Vector2 velocity;
	public float speed;
	public bool grounded;
    
    public float jumpHeight;
    public float jumpTimer;
    public float timeTillNextJump;


    // Start is called before the first frame update
    void Start()
    {
        originalPos = transform.position;
        
        animator = GetComponent<Animator>();
		character = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (health < rageHealth)
        {
            rage = true;
        }

        if (health < 0 && alive)
        {
	        alive = false;
	        animator.SetTrigger("Death");
        }

		Move();

		CheckPlayerDistance();
    }

	void CheckPlayerDistance()
	{
		if (!active && (character.transform.position - transform.position).magnitude < wakeUpDistance)
		{
			WakeUp();
		}
	}

	void WakeUp()
	{
		active = true;
		animator.SetBool("Active", true);

	}

	void Move()
	{
		if (animationState == "Idle")
        {

			if (timeTillNextJump <= 0)
			{
				animator.SetTrigger("Jump");
			}
        }

		// Apply Gravity
		velocity += Physics2D.gravity * Time.deltaTime / 2f;

		grounded = transform.position.y <= originalPos.y;

		if (grounded)
		{
			transform.position = new Vector3(transform.position.x, originalPos.y, originalPos.z);
			velocity = Vector2.zero;

			timeTillNextJump -= Time.deltaTime;
		}

		transform.position += (Vector3)velocity * Time.deltaTime;
    }

	void Jump()
	{
		velocity.y = Mathf.Sqrt(2 * jumpHeight * Mathf.Abs(Physics2D.gravity.y) / 2f);
		transform.position += Vector3.up * 0.001f;
		timeTillNextJump = jumpTimer;
	}

    public void applyDamageToGoblin(float damage)
    {
        health -= damage;
		animator.SetTrigger("Hit");
    }

	public void SetVelocity(Vector2 velocity)
    {
        velocity.y = this.velocity.y;
		this.velocity = velocity;
	}

	public void onDeath()
	{
		GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterHealth>().hasWon = true;
		GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMovement>().onWin();
	}

	public void ScreenShake(float stress)
	{
		CameraShake.current.shakeCamera(0.5f, 5, 1);
	}
}

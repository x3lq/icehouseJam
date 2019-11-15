using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterMovement))]
public class CharacterAnimator : MonoBehaviour
{

	private SpriteRenderer spriteRenderer;
	private Animator animator;
	private CharacterMovement movement;

	public ParticleSystem snowJumpParticles;
	public ParticleSystem snowLandParticles;
	public float snowTime;
	private float particleTimer;
	private float landTimer;


	private bool lastGrounded = true;

    // Start is called before the first frame update
    void Start()
    {
		spriteRenderer = GetComponent<SpriteRenderer>();
		animator = GetComponent<Animator>();
		movement = GetComponent<CharacterMovement>();
    }

    // Update is called once per frame
    void Update()
    {
		Flip();

		SetParameters();

		AdjustSpeed();

		Particles();

		lastGrounded = movement.grounded;
    }

	void Flip()
	{
		if (movement.velocity.x < 0)
		{
			spriteRenderer.flipX = false;
		}
		if (movement.velocity.x > 0)
		{
			spriteRenderer.flipX = true;
		}
	}

	void SetParameters()
	{
		animator.SetFloat("Horizontal", movement.velocity.x);
		animator.SetFloat("Vertical", movement.velocity.y);
		animator.SetBool("Grounded", movement.grounded);
	}

	void AdjustSpeed()
	{
		if (!movement.grounded)
		{
			animator.speed = 1;
			return;
		}

		if (movement.velocity.x != 0)
		{
			animator.speed = Mathf.Abs(movement.velocity.x) / movement.speed;
		}
		else
		{
			animator.speed = 1;
		}
	}

	void Particles()
	{
		if (!movement.grounded)
		{
			particleTimer -= Time.deltaTime;
		}
		else
		{
			particleTimer = snowTime;
		}

		ParticleSystem.EmissionModule emission = snowJumpParticles.emission;

		if (!movement.grounded && particleTimer > 0)
		{
			emission.enabled = true;
		}
		else
		{
			emission.enabled = false;
		}

		LandingParticleBurst();
	}

	void LandingParticleBurst()
	{
		ParticleSystem.EmissionModule emission = snowLandParticles.emission;

		landTimer -= Time.deltaTime;

		if (!lastGrounded && movement.grounded)
		{
			landTimer = 0.2f;
			snowLandParticles.time = 0;
			emission.enabled = true;
		}

		if (landTimer < 0)
		{
			emission.enabled = false;
		}
	}
}

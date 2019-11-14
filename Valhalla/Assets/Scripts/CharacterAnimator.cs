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
		if (movement.velocity.x != 0)
		{
			animator.speed = Mathf.Abs(movement.velocity.x) / movement.speed;
		}
		else
		{
			animator.speed = 1;
		}
	}
}

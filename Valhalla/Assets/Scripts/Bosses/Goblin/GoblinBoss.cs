using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GoblinBoss : MonoBehaviour
{

    private Vector3 originalPos;
    public GameObject leftHand;
    public GameObject rightHand;

    [Header("Boss Properties")]
    public float health;
    public float rageHealth;
    
    public Boolean rage;

    public String animationState;
    private Animator animator;

    [Header("Movement")]
	public Vector2 velocity;
	public float speed;
    
    public float jumpHeight;
    public float jumpTimer;
    public float timeTillNextJump;
    
    

    // Start is called before the first frame update
    void Start()
    {
        originalPos = transform.position;
        
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (health < rageHealth)
        {
            rage = true;
        }

		Move();

    }

	void Move()
    {
        if (animationState == "Idle")
        {
            if (timeTillNextJump <= 0)
            {
                timeTillNextJump = jumpTimer;
            }

            float elapsedTimePercentage = (jumpTimer - timeTillNextJump) / jumpTimer;
            velocity.y = Mathf.Lerp(4f, -10f, elapsedTimePercentage);
        
            if(transform.position.y < originalPos.y)
            {
                velocity.y = 0;
                transform.position = new Vector3(transform.position.x, originalPos.y, originalPos.z);
            }
            timeTillNextJump -= Time.deltaTime;
        }
        
        if (animationState == "LeftSmash" || animationState == "RightSmash")
        {
            if (transform.position.y > originalPos.y)
            {
                velocity.y += Physics2D.gravity.y * Time.deltaTime;
            }
            else
            {
                velocity.y = 0;
                transform.position = new Vector3(transform.position.x, originalPos.y, originalPos.z);
            }
        }

		transform.position += (Vector3)velocity * Time.deltaTime;
    }

    public void applyDamageToGoblin(float damage)
    {
        health -= damage;
    }

	public void SetVelocity(Vector2 velocity)
    {
        velocity.y = this.velocity.y;
		this.velocity = velocity;
	}
}

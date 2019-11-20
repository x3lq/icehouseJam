using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GoblinBoss : MonoBehaviour
{
    public CharacterHealth characterHealth;

    public GameObject leftHand;
    public GameObject rightHand;
    private UnityEvent<float> leftHandDamage;
    private UnityEvent<float> rightHandDamage;

    [Header("Boss Properties")]
    public float health;
    public float rageHealth;

    public Boolean rage;

    private String animationState;
    private Animator animator;

    [Header("Damage Properties")] 
    public float[] damageZonesSmash;

    public float maxSmashDistance;

	[Header("Movement")]
	public Vector2 velocity;
	public float speed;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        leftHandDamage = leftHand.GetComponent<Hand>().triggerDamageWithDistance;
        rightHandDamage = rightHand.GetComponent<Hand>().triggerDamageWithDistance;
        
        leftHandDamage?.AddListener(calculateDamage);
        rightHandDamage?.AddListener(calculateDamage);
    }

    // Update is called once per frame
    void Update()
    {
        animationState = animator.GetBehaviour<StateMachineBehaviour>().ToString().Substring(9);
        animationState = animationState.Remove(animationState.Length - 1);
        
        if (health < rageHealth)
        {
            rage = true;
        }

		Move();
        
    }

	void Move()
	{
		transform.position += (Vector3)velocity * Time.deltaTime;
	}

    void calculateDamage(float distance)
    {
        if (animationState == "Smash")
        {
            applySmashDamageToPlayer(distance);
        }
    }

    void applySmashDamageToPlayer(float distance)
    {
        Vector2 handSize = leftHand.GetComponent<BoxCollider2D>().size;
        if (distance < handSize.x / 2)
        {
            characterHealth.applyDamage(characterHealth.maxHealth);
        } else
        {
            float damge = 80;
            foreach(float damageAreaSize in damageZonesSmash)
            {
                if (distance < handSize.x + damageAreaSize)
                {
                    characterHealth.applyDamage(damge);
                    break;
                }

                damge -= 20;
            }
        }
    }

    public void applyDamageToGoblin(float damage)
    {
        
    }

	public void SetVelocity(Vector2 velocity)
	{
		this.velocity = velocity;
	}
}

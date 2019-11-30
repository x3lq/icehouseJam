using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class IdleBehaviour : StateMachineBehaviour
{

    public GoblinBoss goblin;
    public Boolean playedRage;
	public Transform playerTransform;
    
    public List<String> attacks = new List<string>();

	public float timeTillNextJump;
	public float jumpTimer;
    public float timer, minTime, maxTime;

    public Boolean attacking;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        goblin = animator.GetComponent<GoblinBoss>();

		if (!goblin.animationState.Equals("Jump"))
		{

			playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

			if (goblin.rage && !playedRage)
			{
				animator.SetTrigger("Rage");
				playedRage = true;
			}

			timer = Random.Range(minTime, maxTime);
		}

		goblin.animationState = "Idle";
		attacking = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
	    /*
        if (timer <= 0)
        {
            int random = Random.Range(0, attacks.Count);

			String attack = attacks[random];

			if (attack.Equals("Smash"))
			{
				attack = playerTransform.transform.position.x > goblin.transform.position.x ? "RightSmash" : "LeftSmash";
			}
            animator.SetTrigger(attack);

			timer = Random.Range(minTime, maxTime);
        }
        else
        {
            timer -= Time.deltaTime;
        } */
	    //Roar Smash JumpSmash
	    if(attacking)
		    return;

	    attacking = true;
	    float[] attackProbability;
	    float distanceToPlayer = Math.Abs((playerTransform.position - goblin.transform.position).magnitude);
	    if (distanceToPlayer < 3)
	    {
		    attackProbability = new[] {0.6f, 0.8f, 1};
	    } else if (distanceToPlayer < 7)
	    {
		    attackProbability = new[] {0.1f, 0.2f, 1};
	    } else if (distanceToPlayer < 10)
	    {
		    attackProbability = new[] {0.2f, 0.6f, 1};
	    }
	    else
	    {
		    attackProbability = null;
	    }

	    float randomValue = Random.value;

	    if (attackProbability != null)
	    {
		    int i = 0;
		    foreach (var probability in attackProbability)
		    {
			    if (randomValue <= probability)
			    {
				    break;
			    }

			    i++;
		    }
	    
		    String attack = attacks[i];

		    if (attack.Equals("Smash"))
		    {
			    attack = playerTransform.transform.position.x > goblin.transform.position.x ? "RightSmash" : "LeftSmash";
		    }
		    animator.SetTrigger(attack);
	    }
	    else
	    {
		    animator.SetTrigger("Idle");
	    }
	    
	    

		CalculateVelocity();
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
		goblin.SetVelocity(Vector2.zero);
    }

	private void CalculateVelocity()
	{
		Vector2 direction = playerTransform.transform.position - goblin.transform.position;
		direction.y = 0;
		goblin.SetVelocity(direction * goblin.speed);
		
	}
}

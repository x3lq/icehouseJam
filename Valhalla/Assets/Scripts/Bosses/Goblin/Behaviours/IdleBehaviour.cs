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

    public float timer, minTime, maxTime;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        goblin = animator.GetComponent<GoblinBoss>();
		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        if (goblin.rage && !playedRage)
        {
            animator.SetTrigger("Rage");
            playedRage = true;
        }
        
        timer = Random.Range(minTime, maxTime);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (timer <= 0)
        {
            int random = Random.Range(0, attacks.Count);
            //animator.SetTrigger(attacks[random]);
        }
        else
        {
            timer -= Time.deltaTime;
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

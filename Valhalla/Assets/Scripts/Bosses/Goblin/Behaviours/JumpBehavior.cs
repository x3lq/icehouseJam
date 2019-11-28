using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBehavior : StateMachineBehaviour
{

	public GoblinBoss goblin;
	public Transform playerTransform;

	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		goblin = animator.GetComponent<GoblinBoss>();

		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

		goblin.animationState = "Jump";
	}

	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		CalculateVelocity();
	}

	private void CalculateVelocity()
	{
		Vector2 direction = playerTransform.transform.position - goblin.transform.position;
		direction.y = 0;
		goblin.SetVelocity(direction * goblin.speed);
	}
}

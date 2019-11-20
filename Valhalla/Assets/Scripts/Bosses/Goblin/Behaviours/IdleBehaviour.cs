using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class IdleBehaviour : StateMachineBehaviour
{

    public GoblinBoss goblin;
    public Boolean playedRage;
    
    public List<String> attacks = new List<string>();
    private int ran;

    public float timer, minTime, maxTime;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        goblin = animator.GetComponentInParent<GoblinBoss>();
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
            int ran = Random.Range(0, attacks.Count);
            animator.SetTrigger(attacks[ran]);
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}

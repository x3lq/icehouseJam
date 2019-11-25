using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinAnimator : MonoBehaviour
{

    public Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void onAttackEnd()
    {
        animator.SetTrigger("Idle");
    }
}

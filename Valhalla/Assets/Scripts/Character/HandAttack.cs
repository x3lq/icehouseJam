using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAttack : MonoBehaviour
{
    private CharacterMovement player;
    public GoblinBoss goblinBoss;

    public float damage;
    public float attackRange;

    public Boolean attack;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (attack)
        {
            attack = false;

            float leftHandDistance = (goblinBoss.leftHand.transform.position - transform.position).magnitude;
            float rightHandDistance = (goblinBoss.rightHand.transform.position - transform.position).magnitude;
            
            if (leftHandDistance < attackRange || rightHandDistance < attackRange)
            {
                goblinBoss.applyDamageToGoblin(damage);
            }
        }
    }
}
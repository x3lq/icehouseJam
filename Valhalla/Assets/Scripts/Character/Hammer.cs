using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    private CharacterMovement player;
    public GoblinBoss goblinBoss;
    public float hammerLength; 

    public float damage;
    public float attackRange;

    public Boolean attack;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<CharacterMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (attack)
        {
            attack = false;

            float leftHandDistance = (goblinBoss.leftHand.transform.position - transform.position).magnitude;
            float rightHandDistance = (goblinBoss.rightHand.transform.position - transform.position).magnitude;
            
            if (leftHandDistance + hammerLength < attackRange || rightHandDistance + hammerLength < attackRange)
            {
                goblinBoss.applyDamageToGoblin(damage);
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Goblin : MonoBehaviour
{
    [Header("Body Parts")]
    public GameObject head;
    public GameObject body;
    public GameObject leftHand;
    public GameObject rightHand;
    public GameObject leftShoulder;
    public GameObject rightShoulder;
    // Start is called before the first frame update

    [Header("Attack Properties")] 
    public List<GoblinAttack> attacks;

    public float globalAttackCooldown;
    public Boolean attackInProgress;
    void Start()
    {
        attacks = new List<GoblinAttack>();
        attacks.Add(GetComponent<HandDrop>());
        //attacks.Add(GetComponent<Roar>());
        //attacks.Add(GetComponent<JumpSmash>());
    }

    // Update is called once per frame
    void Update()
    {
        if (globalAttackCooldown > 0)
        {
            globalAttackCooldown -= Time.deltaTime;
            return;
        }
        
        if(attackInProgress)
            return;

        attacks[0].startAttack();
        attackInProgress = true;

        //randomly select attack
    }

    public void setCooldownTimer(float cooldown)
    {
        attackInProgress = false;
        globalAttackCooldown = cooldown;
    }
}

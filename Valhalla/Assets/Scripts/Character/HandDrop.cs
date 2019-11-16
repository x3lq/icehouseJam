using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class HandDrop : GoblinAttack
{
    [Header("Goblin Body Parts")] 
    public GameObject leftHand;
    public GameObject rightHand;
    private Vector3 defaultPosition;

    [Header("Probability of Attacking Hand")]
    private GameObject attackingHand;
    public float probability = 0.5f;
    public float probabilityDecreaseFactor;

    [Header("Attack State")] 
    public Boolean rising;
    public Boolean smashing;
    public Boolean smashingHold;
    public Boolean pulling;
    public Boolean reset;

    [Header("Attack Properties")] 
    public float risingSpeed;
    public float risingDuration;
    private float fallingSpeed;
    public float fallingTime;
    private float fallingTimeElapsed;
    private Boolean calculatedFallingSpeed;
    public float smashHoldTimeStart;
    private float smashHoldTimeStartTimer;
    public float risingTimeElapsed;
    private float xSpeed;
    public float smashingMaxXOffest;
    private float smashingXOffset;

    [Header("Pulling Hand")] 
    public float pullDuration;
    public float pullDistance;
    
    private float pullTimeElapsed  = 0;
    private float pullSpeed;

    [Header("Reset Hand")] 
    public float resetDuration;
    public float resetDurationElapsed;
    private Boolean resetStart;
    public float resetSpeed;

    void Start()
    {
        goblinManager = GetComponent<Goblin>();
        pullSpeed = pullDistance / pullDuration;
    }
    
    // Update is called once per frame
    void Update()
    {
        if(!attack)
            return;

        if (!attackingHand)
            return;

        if (rising)
        {
            riseHand();
        }

        if (smashing)
        {
            smashHand();
        }

        if (pulling)
        {
            pullHand();
        }

        if (reset)
        {
            resetHand();
        }
    }

    private void riseHand()
    {
        if (risingTimeElapsed > risingDuration)
        {
            risingTimeElapsed = 0;
            rising = false;
            smashing = true;
            calculatedFallingSpeed = false;
            smashingHold = true;
        }

        risingTimeElapsed += Time.deltaTime;
        attackingHand.transform.position += Vector3.up * risingSpeed * Time.deltaTime;
    }

    private void smashHand()
    {
        if (smashingHold)
        {
            smashHoldTimeStartTimer += Time.deltaTime;

            if (smashHoldTimeStartTimer > smashHoldTimeStart)
            {
                smashingHold = false;
                smashHoldTimeStartTimer = 0;
            }
        }
        else
        {
            // s = vt => t = s / v
            if (!calculatedFallingSpeed)
            {
                float handHeight = attackingHand.transform.position.y - ground.transform.position.y
                                                                      - (attackingHand.transform.localScale.y / 2)
                                                                      - (ground.transform.localScale.y / 2);

                fallingSpeed = handHeight / fallingTime;
                smashingXOffset = Random.Range(-smashingMaxXOffest, smashingMaxXOffest);
                xSpeed = smashingXOffset / fallingTime;

                fallingTimeElapsed = fallingTime;
                calculatedFallingSpeed = true;
            }

            if (fallingTimeElapsed <= 0)
            {
                smashing = false;
                pulling = true;
            }
            
            Vector3 frameOffset = new Vector3(xSpeed * Time.deltaTime,
                (-1) * fallingSpeed * Time.deltaTime,
                0);
            
            attackingHand.transform.position += frameOffset;
            fallingTimeElapsed -= Time.deltaTime;   
        }
    }

    private void pullHand()
    {
        if (pullTimeElapsed > pullDuration)
        {
            pullTimeElapsed = 0;
            pulling = false;
            reset = true;
            resetStart = true;
        }

        attackingHand.transform.position += Vector3.forward * (pullSpeed * Time.deltaTime);
        pullTimeElapsed += Time.deltaTime;
    }

    private void resetHand()
    {
        if (resetStart)
        {
            resetSpeed = (defaultPosition - attackingHand.transform.position).magnitude / resetDuration;
            resetStart = false;
        }

        if (resetDurationElapsed > resetDuration)
        {
            resetDurationElapsed = 0;
            reset = false;
            goblinManager.setCooldownTimer(3);
        }

        Vector3 direction = (defaultPosition - attackingHand.transform.position).normalized;
        attackingHand.transform.position += direction * (resetSpeed * Time.deltaTime);
        resetDurationElapsed += Time.deltaTime;
    }

    public override void startAttack()
    {
        float random = Random.value;
        
        if (random < probability)
        {
            attackingHand = leftHand;
            probability -= probabilityDecreaseFactor;
        }
        else
        {
            attackingHand = rightHand;
            probability += probabilityDecreaseFactor;
        }

        defaultPosition = new Vector3(attackingHand.transform.position.x,
            attackingHand.transform.position.y,
            attackingHand.transform.position.z);
        attack = true;
        rising = true;
    }
}

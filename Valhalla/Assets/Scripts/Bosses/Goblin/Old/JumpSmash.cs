using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpSmash : GoblinAttack
{
    [Header("Body Parts")]
    public GameObject head;
    public GameObject body;
    public GameObject leftHand;
    public GameObject rightHand;
    public GameObject leftFoot;
    public GameObject rightFoot;

    private Vector3 headOriginalPosition;
    private Vector3 bodyOriginalPosition;
    private Vector3 leftHandOriginalPosition;
    private Vector3 rightHandOriginalPosition;
    private Vector3 leftFootOriginalPosition;
    private Vector3 rightFootOriginalPosition;

    [Header("Attack Properties")] 

    [Header("Jump")] 
    public float squatDuration;
    public Boolean squat;
    public Boolean kneesBend;
    public Boolean armsBack;

    public float riseDuration;
    public Boolean jumping;
    public Boolean armsUp;
    public Boolean bodyStretched;

    public float smashDuration;
    public Boolean smashing;

    public float resetDuration;
    public Boolean reset;
    
    
    

    // Start is called before the first frame update
    void Start()
    {
        goblinManager = GetComponent<Goblin>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!attack)
            return;

        if (squat)
        {
            prepareJump();
        }

        if (jumping)
        {
            jump();
        }

        if (smashing)
        {
            smash();
        }

        if (reset)
        {
            
        }
    }

    private void prepareJump()
    {
        
    }

    private void jump()
    {
        
    }

    private void smash()
    {
        
    }

    public override void startAttack()
    {
        saveOriginalPositions();
        
        squat = true;
        attack = true;
    }

    private void saveOriginalPositions()
    {
        headOriginalPosition = new Vector3(head.transform.position.x,
                    head.transform.position.y,
                    head.transform.position.z);
                
        bodyOriginalPosition = new Vector3(body.transform.position.x,
            body.transform.position.y,
            body.transform.position.z);
        
        leftHandOriginalPosition = new Vector3(leftHand.transform.position.x,
            leftHand.transform.position.y,
            leftHand.transform.position.z);
        
        rightHandOriginalPosition = new Vector3(rightHand.transform.position.x,
            rightHand.transform.position.y,
            rightHand.transform.position.z);
        
        leftFootOriginalPosition = new Vector3(leftFoot.transform.position.x,
            leftFoot.transform.position.y,
            leftFoot.transform.position.z);
        
        rightFootOriginalPosition = new Vector3(rightFoot.transform.position.x,
            rightFoot.transform.position.y,
            rightFoot.transform.position.z);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmashAttack : MonoBehaviour
{
    private GoblinBoss goblinBoss;
    
    public GameObject leftHand;
    public GameObject rightHand;
    public GameObject smashingHand;
    
    [Header("Smash Damage")]
    public CharacterHealth characterHealth;

    public float maxSmashDistance;
    public float maxDamage;

    private void Start()
    {
        goblinBoss = GetComponent<GoblinBoss>();
    }

    public void checkSmashHit()
    {
        if (goblinBoss.animationState == "LeftSmash")
        {
            smashingHand = leftHand;
        }
        else
        {
            smashingHand = rightHand;
        }
        
        applyDamageToPlayer();
    }
    
    private void applyDamageToPlayer()
    {
        float distance = (characterHealth.transform.position - smashingHand.transform.position).magnitude;
        Vector2 handSize = smashingHand.transform.GetComponent<BoxCollider2D>().size;
        if (distance < handSize.x / 2)
        {
            characterHealth.applyDamage(characterHealth.maxHealth);
        }
        else
        {
            distance -= handSize.x / 2;

            if (distance < maxSmashDistance)
            {
                float damagePercent = distance / maxSmashDistance;
                characterHealth.applyDamage(damagePercent * maxDamage);
            }
        }
    }
}

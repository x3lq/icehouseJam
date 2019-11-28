using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpSmashAttack : MonoBehaviour
{
    private GoblinBoss goblinBoss;
    private Vector2 handSize;

    public GameObject leftHand;
    public GameObject rightHand;

    [Header("Smash Damage")] public CharacterHealth characterHealth;

    public float maxSmashDistance;
    public float maxDamage;

    private void Start()
    {
        goblinBoss = GetComponent<GoblinBoss>();
        characterHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterHealth>();

    }

    public void onJumpSmash()
    {
        Vector3 leftHandDistance = leftHand.transform.position - characterHealth.transform.position;
        Vector3 rightHandDistance = rightHand.transform.position - characterHealth.transform.position;

        //left of doubleHand
        if (leftHandDistance.x < 0)
        {
            handSize = leftHand.GetComponent<BoxCollider2D>().size;
            applyDamageToPlayer(leftHandDistance.magnitude);
        }
        //tight hand of doubleside
        else if (rightHandDistance.x > 0)
        {
            handSize = rightHand.GetComponent<BoxCollider2D>().size;
            applyDamageToPlayer(rightHandDistance.magnitude);
        }
        //fucking moron stood in the middle
        else
        {
            characterHealth.applyDamage(characterHealth.maxHealth);
        }
    }

    private void applyDamageToPlayer(float distance)
    {
        distance -= handSize.x / 2;

        if (distance < maxSmashDistance)
        {
            float damagePercent = (maxSmashDistance - distance) / maxSmashDistance;
            characterHealth.applyDamage(damagePercent * maxDamage);
        }
    }
}
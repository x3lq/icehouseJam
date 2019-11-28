using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAttack : MonoBehaviour
{
	public GameObject attackPrefab;

    private CharacterMovement player;
    public GoblinBoss goblinBoss;

    public float damage;
    public float attackRange;
	public float attackSpeed;
	public float attackScale;
	public float attackScaleSpeed;

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

			Vector3 position = transform.position;
			position += player.lookingRight ? Vector3.right : Vector3.left;

			AttackRune rune = Instantiate(attackPrefab, position, Quaternion.identity).GetComponent<AttackRune>();
			rune.goToRight = player.lookingRight;
			rune.damage = damage;
			rune.speed = attackSpeed;
			rune.range = attackRange;
			rune.scale = attackScale;
			rune.scaleSpeed = attackScaleSpeed;

			/*
            float leftHandDistance = (goblinBoss.leftHand.transform.position - transform.position).magnitude;
            float rightHandDistance = (goblinBoss.rightHand.transform.position - transform.position).magnitude;
            
            if (leftHandDistance < attackRange || rightHandDistance < attackRange)
            {
                goblinBoss.applyDamageToGoblin(damage);
            }
			*/
        }
    }
}
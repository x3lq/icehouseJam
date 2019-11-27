using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speer : MonoBehaviour
{
    public GoblinBoss goblinBoss;
    
    public float alifeFor;
    public Boolean hitTarget;
    
    public float speed;
    public Vector2 direction;
    public float damage;

    public LayerMask collisionLayers;
    public Collider2D[] hits;
    private BoxCollider2D boxCollider;

    // Start is called before the first frame update
    void Start()
    {
        //goblinBoss = GameObject.FindGameObjectWithTag("Boss").GetComponent<GoblinBoss>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hitTarget)
        {
            return;
        }
        
        if (alifeFor < 0)
        {
            Destroy(gameObject);
        }

        hits = Physics2D.OverlapBoxAll(transform.position, boxCollider.size, 0, collisionLayers);
        
        foreach (Collider2D hit in hits)
        {
            if (hit == boxCollider)
                continue;
			
            ColliderDistance2D colliderDistance = hit.Distance(boxCollider);

            if (colliderDistance.distance < 0.1)
            {
                direction = Vector2.zero;
                hitTarget = true;
                // goblinBoss.applyDamageToGoblin(damage);
                Destroy(gameObject, 0.1f);
            }
        }
        
        alifeFor -= Time.deltaTime;
        
        transform.position += (Vector3)direction * (speed * Time.deltaTime);
    }

    public void throwSpeer(Vector2 direction)
    {
        this.direction = direction;
    }
}

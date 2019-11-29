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
	public Vector3 velocity;
    public float damage;

    public LayerMask collisionLayers;
    public Collider2D[] hits;
    private BoxCollider2D boxCollider;

    // Start is called before the first frame update
    void Start()
    {
        //goblinBoss = GameObject.FindGameObjectWithTag("Boss").GetComponent<GoblinBoss>();
        boxCollider = GetComponent<BoxCollider2D>();

		velocity = direction * speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (hitTarget)
        {
            return;
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
				transform.parent = hit.transform;
                Destroy(gameObject, 5f);

				if (hit.tag.Equals("Boss"))
				{
					hit.GetComponentInParent<GoblinBoss>().applyDamageToGoblin(damage);
				}

				return;
			}
        }

		velocity += (Vector3)Physics2D.gravity * Time.deltaTime;

        transform.position += velocity * Time.deltaTime;

		Rotate();
    }

	void Rotate()
	{
		transform.rotation = Quaternion.identity;

		float angle = Vector3.Angle(Vector3.up, velocity);

		angle *= direction.x > 0 ? -1 : 1;

		transform.Rotate(new Vector3(0, 0, angle));
	}

    public void throwSpeer(Vector2 direction)
    {
        this.direction = direction;
    }
}

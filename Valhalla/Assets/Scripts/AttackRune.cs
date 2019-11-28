using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRune : MonoBehaviour
{

	public float damage;

	new CircleCollider2D collider;

	private Collider2D[] hits;

    // Start is called before the first frame update
    void Start()
    {
		collider = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
		CheckHit();
    }

	void CheckHit()
	{

		hits = Physics2D.OverlapCircleAll(transform.position + (Vector3)collider.offset, collider.radius);

		foreach (Collider2D hit in hits)
		{
			if (hit.tag.Equals("Boss"))
			{
				hit.GetComponentInParent<GoblinBoss>().applyDamageToGoblin(damage);
				Destroy();
			}
		}
	}

	void Destroy()
	{
		DestroyImmediate(gameObject);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRune : MonoBehaviour
{

	public float damage;
	public float speed;
	public float range;
	public float scale;
	public float scaleSpeed;
	float currentScale;
	private Vector3 startPosition;
	private Vector3 targetPosition;

	new CircleCollider2D collider;

	private Collider2D[] hits;

	public bool goToRight;

	public bool destroy;

    // Start is called before the first frame update
    void Start()
    {
		collider = GetComponent<CircleCollider2D>();

		startPosition = transform.position;

		if (goToRight)
		{
			targetPosition = startPosition + Vector3.right * range;
			transform.Rotate(new Vector3(0, 0, -90));
		}
		else
		{
			targetPosition = startPosition + Vector3.left * range;
			transform.Rotate(new Vector3(0, 0, 90));
		}

		currentScale = 1;
	}

    // Update is called once per frame
    void Update()
    {
		Move();
		CheckHit();

		if (destroy)
		{
			Destroy();
		}
    }

	void CheckHit()
	{
		hits = Physics2D.OverlapCircleAll(transform.position + (Vector3)collider.offset, collider.radius);

		foreach (Collider2D hit in hits)
		{
			if (hit.tag.Equals("Boss"))
			{
				hit.GetComponentInParent<GoblinBoss>().applyDamageToGoblin(damage);
				destroy = true;
			}
		}
	}

	void Move()
	{
		transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);

		if((transform.position - targetPosition).magnitude < 0.5f)
		{
			destroy = true;
		}

		currentScale = Mathf.Lerp(currentScale, scale, Time.deltaTime * scaleSpeed);

		transform.localScale = new Vector3(currentScale, currentScale);
	}

	void Destroy()
	{
		DestroyImmediate(gameObject);
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraFollow : MonoBehaviour
{
    public float speed;
	public float distance;
	public float lookAheadDistance;
    public Vector2 offset;

	public CharacterMovement target;
	private bool lookingRight = true;

	public float verticalMaxDistance;

	public float distanceToTarget;

	public Vector3 position;
	

    private void Update()
    {
		Follow();
		VerticalLock();
    }

	private void Follow()
	{
		if (target.velocity.x > 0)
		{
			lookingRight = true;
		}
		if (target.velocity.x < 0)
		{
			lookingRight = false;
		}

		Vector3 targetPosition = target.transform.position + Vector3.back * distance + (Vector3)offset;

		targetPosition += lookingRight ? Vector3.right * lookAheadDistance : Vector3.left * lookAheadDistance;

		position = Vector3.Lerp(position, targetPosition, speed * Time.deltaTime);
	}

	void VerticalLock()
	{
		distanceToTarget = target.transform.position.y - transform.position.y;

		if (Mathf.Abs(distanceToTarget) > verticalMaxDistance)
		{
			position += Vector3.up * (Mathf.Abs(distanceToTarget) - verticalMaxDistance) * Mathf.Sign(distanceToTarget);
		}

		distanceToTarget = target.transform.position.y - transform.position.y;
	}
}

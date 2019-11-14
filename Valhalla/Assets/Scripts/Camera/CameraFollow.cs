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
	private bool lookingRight;

    // Use this for initialization
    void Start () {

    }

    private void Update()
    {
		Follow();
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

		Vector3 targetPosition = target.transform.position + (Vector3)offset + Vector3.back * distance;

		targetPosition += lookingRight ? Vector3.right * lookAheadDistance : Vector3.left * lookAheadDistance;

		transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
	}
}

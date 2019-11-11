using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[Header("References")]
	public CharacterMovement character;

	[Header("Settings")]
	public float distance;
	public float speed;

	[Header("Status")]
	public Vector3 targetPosition;

    // Start is called before the first frame update
    void Start()
    {
		transform.position = character.transform.position + Vector3.back * distance;
    }

    // Update is called once per frame
    void Update()
    {
		Follow();
    }

	void Follow()
	{
		targetPosition = character.transform.position + Vector3.back * distance;
		transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
	}
}

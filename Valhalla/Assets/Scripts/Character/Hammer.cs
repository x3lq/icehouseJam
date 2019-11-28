using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    private CharacterMovement player;
    public GoblinBoss goblinBoss;
	public GameObject splashPrefab;

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
		}
    }

	void SpawnSplash()
	{
		Vector3 position = transform.position;
		position += player.lookingRight ? Vector3.right * 1.5f : Vector3.left * 1.5f;

		GameObject splash = Instantiate(splashPrefab, position, Quaternion.identity);
	}
}

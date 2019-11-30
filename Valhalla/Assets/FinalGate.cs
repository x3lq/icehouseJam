using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalGate : MonoBehaviour
{
    public CharacterMovement player;

    private void Update()
    {
        if (player)
        {
            player.velocity = Vector2.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = other.GetComponent<CharacterMovement>();
            GameManager.instance.finalScreen();
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spike : MonoBehaviour
{
    public float damageToPlayer;
    public CharacterHealth player;
    public float width;
    public float height;
    public float playerHeight;
    public float playerWidth;
    
    public float maxHeight;
    private float prevHeight = 0;

    public float riseSpeed;
    private float riseDuration;

    public float shakeDuration;
    private float shakeDurationTimer;
    public float maxAngle;

    public float fallSpeed;
    private float fallDuration;

    public Boolean shake;
    public Boolean fall;
    public Boolean hit;

    private void Start()
    {
        riseDuration = maxHeight / riseSpeed;
        shakeDurationTimer = shakeDuration;
        fallDuration = maxHeight / fallSpeed;
        width = GetComponent<BoxCollider2D>().size.x;
        height = GetComponent<BoxCollider2D>().size.y;

        player = GameObject.FindWithTag("Player").GetComponent<CharacterHealth>();
        playerHeight = player.GetComponent<BoxCollider2D>().size.y / 2;
        playerWidth = player.GetComponent<BoxCollider2D>().size.x / 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (riseDuration > 0)
        {
            transform.position += Vector3.up * (riseSpeed * Time.deltaTime);
            riseDuration -= Time.deltaTime;

            if (riseDuration <= 0)
            {
                shake = true;
            }
        }

        if (shake)
        {
            shakeDurationTimer -= Time.deltaTime;

            if (shakeDurationTimer < shakeDuration / 2)
            {
                transform.Rotate(0, Random.Range(-maxAngle, maxAngle), 0);
            }

            if (shakeDurationTimer < 0)
            {
                shake = false;
                fall = true;
            }
            
            collisionDetection();
        }

        if (fall)
        {
            fallDuration -= Time.deltaTime;

            if (fallDuration < 0)
            {
                Destroy(transform.gameObject);
            }
            
            transform.position -= Vector3.up * (fallSpeed * Time.deltaTime); 
        }
    }

    private void collisionDetection()
    {
        if (hit)
        {
            return;
        }
        if (Math.Abs(transform.position.x - player.transform.position.x) < width + playerWidth)
        {
            if (Math.Abs(transform.position.y - player.transform.position.y) < height + playerHeight)
            {
                player.applyDamage(damageToPlayer);
                hit = true;
            }
        }
    }
}
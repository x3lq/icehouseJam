using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spike : MonoBehaviour
{
    public Transform originalPos;

    public float lifeTime;
    public float aliveFor;
    public float speedMultiplyer;

    public float maxHeight;
    private float prevHeight = 0;

    public float riseSpeed;
    private float riseDuration;

    public float shakeDuration;
    private float shakeDurationTimer;
    public float maxAngle;

    public float fallSpeed;
    private float fallDuration;

    public Boolean rise;
    public Boolean shake;
    public Boolean fall;

    private void Start()
    {
        rise = true;
        originalPos = transform;
        riseDuration = maxHeight / riseSpeed;
        shakeDurationTimer = shakeDuration;
        fallDuration = maxHeight / fallSpeed;
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

/*
        float newHeight = (float) (maxHeight * Math.Sin(aliveFor * speedMultiplyer));
        float newHeightOffset =  newHeight - prevHeight;
        prevHeight = newHeight;
        
        transform.position += new Vector3(0, newHeightOffset, 0);
*/
        aliveFor += Time.deltaTime;
        lifeTime -= Time.deltaTime;
    }
}
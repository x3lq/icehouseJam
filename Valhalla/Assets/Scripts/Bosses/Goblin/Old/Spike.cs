using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    public float lifeTime;
    public float aliveFor;
    public float speedMultiplyer;

    public float maxHeight;
    private float prevHeight = 0;

    // Update is called once per frame
    void Update()
    {
        if (lifeTime < 0)
        {
            Destroy(transform.gameObject);
        }

        float newHeight = (float) (maxHeight * Math.Sin(aliveFor * speedMultiplyer));
        float newHeightOffset =  newHeight - prevHeight;
        prevHeight = newHeight;
        
        transform.position += new Vector3(0, newHeightOffset, 0);

        aliveFor += Time.deltaTime;
        lifeTime -= Time.deltaTime;
    }
}

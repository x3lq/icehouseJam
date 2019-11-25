using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxtRotation : MonoBehaviour
{
    public Boolean rotate;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (rotate)
        {
            transform.Rotate(new Vector3(0, 0, speed)) ;
        }
    }
}

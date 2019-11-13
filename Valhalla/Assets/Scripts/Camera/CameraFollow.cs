using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraFollow : MonoBehaviour
{
    public float interpVelocity;
    public float movementSpeed;
    public GameObject leftPosition, rightPosition, target;
    public Vector3 offset;
    Vector3 targetPos;

    public Boolean movingLeft;
    // Use this for initialization
    void Start () {
        targetPos = transform.position;
    }

    private void Update()
    {
        if (leftPosition && rightPosition)
        {
            Vector3 posNoZ = transform.position;
            posNoZ.z = leftPosition.transform.position.z;
  
            Vector3 targetDirection = movingLeft ? (leftPosition.transform.position - posNoZ) : (rightPosition.transform.position - posNoZ);
  
            movingLeft = targetDirection.x < 0;
            interpVelocity = targetDirection.magnitude * movementSpeed;
  
            targetPos = transform.position + (targetDirection.normalized * interpVelocity * Time.deltaTime); 
  
            transform.position = Vector3.Lerp( transform.position, targetPos + offset, 0.25f);
  
        }
    }
}

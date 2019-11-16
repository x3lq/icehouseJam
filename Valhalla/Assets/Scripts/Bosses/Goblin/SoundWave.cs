using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundWave : MonoBehaviour
{
    public float lifeTime;
    public float scaleIncreaseFactor;
    
    public Vector3 travelSpeed;

    // Update is called once per frame
    void Update()
    {
        if(lifeTime < 0)
            Destroy(transform.gameObject);

        transform.position += travelSpeed * Time.deltaTime;
        transform.localScale += transform.localScale * scaleIncreaseFactor;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using Random = UnityEngine.Random;

public class CameraShake : MonoBehaviour
{
    //shakes Camera based on duration and magnitude
    //script has to applied to a game object, which has the camera as a child

    /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            shakeFixedNumber(3, 3);
        }
    }*/

    public void shake(float duration, float magnitude)
    {
        StartCoroutine(shakeHelper(duration, magnitude / 100f));
    }

    public void shakeFixedNumber(float amount, float magnitude)
    {
        while (amount > 0)
        {
            amount -= 1;
            StartCoroutine(shakeHelper(0.2f, magnitude / 100f));

        }
    }

    IEnumerator shakeHelper(float duration, float magnitude)
    {
        Vector3 originalPosition = transform.localPosition;

        float elapsed = 0;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            
            transform.localPosition = originalPosition + new Vector3(x, y, 0);
            
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPosition;
    }
}

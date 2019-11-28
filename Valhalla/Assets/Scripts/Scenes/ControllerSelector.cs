using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControllerSelector : MonoBehaviour
{
    public static String type = "XBox";

    public Text topText;
    public Text bottomText;

    public float deadTime;
    private float deadTimer;
    
    void Start()
    {
        topText.text = "XBox";
        bottomText.text = "PS4";
    }

    private void Update()
    {
        if ( (Input.GetAxis("Vertical") < -0.8 || Input.GetAxis("Vertical") > 0.8) && deadTimer < 0)
        {
            String buffer = topText.text;
            topText.text = bottomText.text;
            bottomText.text = buffer;
            type = topText.text;
            deadTimer = deadTime;
        }

        if (Input.GetButtonDown("Jump" + type))
        {
            Debug.Log(ControllerSelector.type);
            SceneManager.LoadScene(1);
        }
        
        deadTimer -= Time.deltaTime;

    }
}

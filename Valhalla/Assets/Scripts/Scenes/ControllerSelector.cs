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

	public int selection = 0;

    private void Update()
    {
        if ( (Input.GetAxis("Vertical") < -0.8 || Input.GetAxis("Vertical") > 0.8) && deadTimer < 0)
        {
			selection = (selection + 1) % 2;
			/*
            String buffer = topText.text;
            topText.text = bottomText.text;
            bottomText.text = buffer;
			*/
			deadTimer = deadTime;

        }

        if (Input.GetButtonDown("Jump" + type))
        {
            Debug.Log(ControllerSelector.type);
            LevelChanger.Instance.fadeToLevel(1);
        }
        
        deadTimer -= Time.deltaTime;

		type = selection == 0 ? "XBox" : "PS4";
		if (selection == 0)
		{
			topText.text = "> XBox <";
			bottomText.text = "PS4";
		}
		else
		{
			topText.text = "XBox";
			bottomText.text = "> PS4 <";
		}
	}
}

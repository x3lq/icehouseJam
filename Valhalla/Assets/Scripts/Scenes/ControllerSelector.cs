using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControllerSelector : MonoBehaviour
{
    public static String type = "PC";

    public Text topText;
    public Text midText;
    public Text bottomText;

    public float deadTime;
    private float deadTimer;

	public int selection = 0;

    private void Update()
    {
        if (Input.GetAxis("Vertical") < -0.8 && deadTimer < 0) 
        {
			selection = (selection + 1) % 3;
			/*
            String buffer = topText.text;
            topText.text = bottomText.text;
            bottomText.text = buffer;
			*/
			deadTimer = deadTime;

        }
        
        if(Input.GetAxis("Vertical") > 0.8 && deadTimer < 0)
        {
	        selection = (selection - 1) % 3;
	        deadTimer = deadTime;
        }

        if (Input.GetButtonDown("Jump" + type) || Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log(ControllerSelector.type);
            LevelChanger.Instance.fadeToLevel(1);
        }
        
        deadTimer -= Time.deltaTime;

        switch (selection)
        {
	        case 0:
		        topText.text = "> XBox <";
		        midText.text = "PS4";
		        bottomText.text = "PC";
		        type = "XBox";
		        break;
	        case 1:
		        topText.text = "XBox";
		        midText.text = "> PS4 <";
		        bottomText.text = "PC";
		        type = "PS4";
		        break;
	        case 2:
		        topText.text = "XBox";
		        midText.text = "PS4";
		        bottomText.text = "> PC <";
		        type = "PC";
		        break;
        }

		/*type = selection == 0 ? "XBox" : "PS4";
		if (selection == 0)
		{
			topText.text = "> XBox <";
			bottomText.text = "PS4";
		}
		else
		{
			topText.text = "XBox";
			bottomText.text = "> PS4 <";
		}*/
	}
}

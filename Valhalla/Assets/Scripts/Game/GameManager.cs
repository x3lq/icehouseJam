using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    void Start()
    {
        instance = this;
    }

    public void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown("Start"))
        {
            //ToDo show Menu
        }*/
    }

    public void startGame()
    {
        SceneManager.LoadScene(1);
    }

    public void playerLost()
    {
        SceneManager.LoadScene(3);
    }

    public void playerWon()
    {
        SceneManager.LoadScene(4);
    }

    public void finalScreen()
    {
        SceneManager.LoadScene(5);
    }
}

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
        LevelChanger.Instance.fadeToLevel(2);
    }

    public void playerLost()
    {
		AudioManager.current.selection = AudioManager.Tracks.ambienceWithMusic;
		AudioManager.current.boss = null;
		AudioManager.current.character = null;
        LevelChanger.Instance.fadeToLevel(3);

    }

    public void playerWon()
    {
        LevelChanger.Instance.fadeToLevel(4);

    }

    public void finalScreen()
    {
        LevelChanger.Instance.fadeToLevel(5);
    }
}

﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    private static LevelChanger instance;
    public Boolean isGameScene;
    public float timeTillEnteringScene;

    public static LevelChanger Instance
    {
        get => instance;
    }

    public Animator animator;
    private int levelToLoad;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        
        if (isGameScene)
        {
            animator.SetTrigger("HoldImage");
            StartCoroutine(fadeInAfterTime());
        }
    }

    public void fadeToLevel(int levelIndex)
    {
        animator.SetTrigger("Fade_Out");
        levelToLoad = levelIndex;
    }

    public void onFadeComplete()
    {
        SceneManager.LoadSceneAsync(levelToLoad);
    }

    IEnumerator fadeInAfterTime()
    {
        yield return new WaitForSeconds(timeTillEnteringScene);
        animator.SetTrigger("Fade_In");
    }
}

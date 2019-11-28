using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    private static LevelChanger instance;

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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevelAfterTime : MonoBehaviour
{
    public float time;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(loadLevel());
    }

    IEnumerator loadLevel()
    {
        yield return new WaitForSeconds(time);
        GameManager.instance.startGame();
    }
}

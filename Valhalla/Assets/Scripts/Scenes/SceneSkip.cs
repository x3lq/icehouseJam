using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSkip : MonoBehaviour
{
    public int levelToLoadTo;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Options" + ControllerSelector.type))
        {
            LevelChanger.Instance.fadeToLevel(levelToLoadTo);
        }
    }
}

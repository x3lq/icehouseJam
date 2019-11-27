using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextSwitcher : MonoBehaviour
{
    public float duration;
    public Transform startPosition;

    public String text;

    public GameObject runesText;
    public GameObject latinText;
    
    private String[] splitText;
    // Start is called before the first frame update
    void Start()
    {
        splitText = text.Split(' ');
        StartCoroutine(writeRuneText());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator writeRuneText()
    {
        float xOffset = 0;
        float yOffset = 0;
        
        foreach (var word in splitText)
        {
            foreach (var letter in word)
            {
                Vector3 position = startPosition.position + new Vector3(xOffset, yOffset, 0);
                GameObject newText = Instantiate(runesText, position, Quaternion.identity);
                newText.GetComponent<Text>().text = letter.ToString();
                newText.transform.SetParent(startPosition);
                xOffset += 20f;
            }
            yield return new WaitForSeconds(0.1f);

            xOffset += 40;
            if (xOffset > 600)
            {
                xOffset = 0;
                yOffset -= 30;
            }
        }
    }
}

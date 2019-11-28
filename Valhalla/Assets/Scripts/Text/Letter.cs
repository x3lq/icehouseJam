using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public class Letter : MonoBehaviour
{
    public GameObject latinPrefab;
    
    public float alphaStep;
    private Text text;

    private float timeTillMorph;
    public float minDisplay;
    public float maxDisplay;
    
    public Boolean alphaFade;
    public Boolean runeMorphing;
    // Start is called before the first frame update
    void Start()
    {
        timeTillMorph = Random.Range(minDisplay, maxDisplay);
        text = GetComponent<Text>();

        Color color = text.color;
        color.a = 0;
        text.color = color;

        alphaFade = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (alphaFade)
        {
            Color color = text.color;
            color.a += alphaStep;
            text.color = color;

            if (color.a > 1)
            {
                alphaFade = false;
            }
            return;
        }

        timeTillMorph -= Time.deltaTime;

        if (timeTillMorph < 0 && !runeMorphing)
        {
            runeMorphing = true;
            StartCoroutine(morphLetter());
        }
    }

    IEnumerator morphLetter()
    {
        GameObject latinCharacter = Instantiate(latinPrefab, transform);
        Text latinText = latinCharacter.GetComponent<Text>();
        latinCharacter.transform.SetParent(transform);
        Color color = latinText.color;
        color.a = 0;
        latinText.color = color;
        latinText.text = text.text.ToString();

        while (latinText.color.a < 1)
        {
            color = latinText.color;
            color.a += alphaStep;
            latinText.color = color;
        
            color = text.color;
            color.a -= alphaStep;
            text.color = color;
            yield return new WaitForEndOfFrame();
        }
        
    }
}

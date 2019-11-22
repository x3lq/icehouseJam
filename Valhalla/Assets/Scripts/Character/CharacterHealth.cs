using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHealth : MonoBehaviour
{

    public float health;
    public float maxHealth;

    public float lastDamage;

    public float regenerationSpeed;

    public float regenerationTime;

    public Image blur;

    public float minAlpha;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        //health regeneration
        if (Time.time - lastDamage > regenerationTime && health < maxHealth)
        {
            health += regenerationSpeed * Time.deltaTime;
            
            if (health > maxHealth)
            {
                health = maxHealth;
            }
        }

        //Adjust damage visuality of blur
        float healthPercentage = health / maxHealth;
        if (healthPercentage < minAlpha)
        {
            healthPercentage = minAlpha;
        }

        //var tmpColor = blur.color;
        //tmpColor.a = healthPercentage;
        //blur.color = tmpColor;
    }

    public void applyDamage(float damage)
    {
        health -= damage;
        lastDamage = Time.time;
    }
}

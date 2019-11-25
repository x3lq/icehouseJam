using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHealth : MonoBehaviour
{
    public int lifes;
    public GameObject spawn;

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
        if (health < 0 && lifes == 0)
        {
            Camera.main.GetComponent<GameManager>().playerLost();
        } else if(health < 0)
        {
            lifes -= 1;
            health = maxHealth;
            resetPlayerToSpawn();
        }
        
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

    private void resetPlayerToSpawn()
    {
        transform.position = spawn.transform.position;
    }
}

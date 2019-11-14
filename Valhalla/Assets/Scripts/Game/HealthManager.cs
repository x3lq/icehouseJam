using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{

    public int lifes;

    public float health, maxHealth;
    public float timeToRegen;

    public float blur;
    public Image blurImage;

    public float healtPerSeconde;

    public DateTime lastDamage;

    public GameManager gameManager;

    
    // Start is called before the first frame update
    void Start()
    {
        health = 100;
        maxHealth = 100;
        lifes = 3;
        applyBlur(0);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            death();
        }
        
        applyBlur(health / maxHealth);
        healthRegen();
    }

    private void healthRegen()
    {
        if (health < maxHealth && DateTime.Now.Subtract(lastDamage).Seconds > timeToRegen)
        {
            health += healtPerSeconde * Time.deltaTime;

            if (health > maxHealth)
            {
                health = maxHealth;
            }
        }
    }

    private void applyBlur(float amount)
    {
        var tempColor = blurImage.color;
        tempColor.a = blur;
        blurImage.color = tempColor;

    }

    private void death()
    {
        if (lifes > 0)
        {
            lifes--;
            respawn();
            return;
        }
        
        //Todo maybe no hardcut of the scene

        gameManager.playerLost();
    }

    private void respawn()
    {
        //ToDo reset character to baseplate
        throw new NotImplementedException("");
    }

    public void applyDamage(float damage)
    {
        health -= damage;
        lastDamage = DateTime.Now;
    }
}

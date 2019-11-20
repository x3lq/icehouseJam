using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GoblinAttack : MonoBehaviour
{

    public GameObject ground;
    public Goblin goblinManager;
    public Boolean attack;

    public float cooldown;


    public abstract void startAttack();
}

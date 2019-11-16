using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roar : GoblinAttack
{

    [Header("Attack Properties")] 
    public float attackDuration;

    public float attackElapsedTime;
    public float roarDuration;
    public float roarElapsedTime;

    [Header("Needed Properties")]
    public GameObject spikePrefab;
    public GameObject head;
    public GameObject soundWavePrefab;

    [Header("Sound Wave")] 
    public float breathingTime;
    public float breathTaken;

    [Header("Sound Wave")] 
    public Boolean firedSpikes;
    public int spikeAmount;
    public float spikeSpawnTimeDiff;
    
    
    // Start is called before the first frame update
    void Start()
    {
        goblinManager = GetComponent<Goblin>();
        spikeSpawnTimeDiff = (attackDuration - roarDuration / 2) / spikeAmount;
    }

    // Update is called once per frame
    void Update()
    {
        if(!attack)
            return;

        if (attackElapsedTime > attackDuration)
        {
            attack = false;
            firedSpikes = false;
            goblinManager.setCooldownTimer(cooldown);
            attackElapsedTime = 0;
            return;
        }

        if (roarElapsedTime < roarDuration)
        {
            roar();
            roarElapsedTime += Time.deltaTime;
        }

        if (!firedSpikes && roarElapsedTime > roarDuration / 2)
        {
            StartCoroutine(spawnSpikes());
            firedSpikes = true;
        }


        attackElapsedTime += Time.deltaTime;
    }

    public void roar()
    {
        //More details needed
        if (breathTaken > 0)
        {
            breathTaken -= Time.deltaTime;
            return;
        }

        breathTaken = breathingTime;
        Instantiate(soundWavePrefab, head.transform.position, Quaternion.identity);
    }
    

    IEnumerator spawnSpikes()
    {
        float width = spikePrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        float xOffset = width / 2;
        float totalXOffset = 0;
        Vector3 startPosition = ground.transform.position;

        for (int i = 0; i < spikeAmount / 2; i++)
        {
            Instantiate(spikePrefab, startPosition + new Vector3(totalXOffset, 0, -1), Quaternion.identity);
            Instantiate(spikePrefab, startPosition - new Vector3(totalXOffset, 0, 1), Quaternion.identity);
            totalXOffset += xOffset;
            Debug.Log(i);
            yield return new WaitForSeconds(spikeSpawnTimeDiff);
        }  
    }

    public override void startAttack()
    {
        roarElapsedTime = 0;
        attack = true;
    }
}

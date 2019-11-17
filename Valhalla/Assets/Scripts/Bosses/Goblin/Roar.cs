using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Roar : GoblinAttack
{
    [Header("Prefabs and GameObjects")]
    public GameObject spikePrefab;
    public GameObject head;
    public GameObject soundWavePrefab;
    public GameObject leftFoot;
    public GameObject rightFoot;

    [Header("Attack Properties")] 
    public float attackDuration;

    public float attackElapsedTime;
    public float roarDuration;
    public float roarElapsedTime;
    
    [Header("Sound Wave")] 
    public float breathingTime;
    public float breathTaken;

    [Header("Spikes")] 
    public Boolean firedSpikes;
    public int spikeAmount;
    public float spikeSpawnTimeDiff;

    [Header("Stomping")]
    public GameObject stompingFoot;
    public float probabilityFoots;
    public float probabilityChange;
    public Boolean rising;
    public Boolean stomp;
    private Vector3 gainedHeight;
    public float risingSpeed;
    public float risingDuration;
    private float risingTimeElapsed;
    private float stompSpeed;
    public float stompDuration;
    private Vector3 originalPosition;
    
    
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
        
        stomping();

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

    public void stomping()
    {
        if (rising)
        {
            if (risingTimeElapsed > risingDuration)
            {
                rising = false;
                stomp = true;
                risingTimeElapsed = 0;
                stompSpeed = gainedHeight.magnitude / stompDuration;
            }
            Vector3 heightOffset = Vector3.up * (risingSpeed * Time.deltaTime);
            gainedHeight += heightOffset;
            stompingFoot.transform.position += heightOffset;

            risingTimeElapsed += Time.deltaTime;
        }

        if (stomp)
        {
            if (stompDuration < 0)
            {
                stomp = false;
                StartCoroutine(resetFoot());
            }
            
            stompingFoot.transform.position += Vector3.down * (stompSpeed * Time.deltaTime);
            stompDuration -= Time.deltaTime;
        }
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
        startPosition.x = stompingFoot.transform.position.x;

        for (int i = 0; i < spikeAmount / 2; i++)
        {
            Instantiate(spikePrefab, startPosition + new Vector3(totalXOffset, 0, -1), Quaternion.identity);
            Instantiate(spikePrefab, startPosition - new Vector3(totalXOffset, 0, 1), Quaternion.identity);
            totalXOffset += xOffset;
            yield return new WaitForSeconds(spikeSpawnTimeDiff);
        }  
    }

    IEnumerator resetFoot()
    {
        float elapsedTime = 0;
        float totalTime = 8;
        Vector3 startPosition = new Vector3(stompingFoot.transform.position.x,
            stompingFoot.transform.position.y,
            stompingFoot.transform.position.z);
        
        while ((stompingFoot.transform.position - originalPosition).magnitude > 0.005)
        {
            float percentageTime = elapsedTime / totalTime;
            stompingFoot.transform.position = Vector3.Lerp(startPosition, originalPosition, percentageTime);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        stompingFoot.transform.position = originalPosition;
    }

    public override void startAttack()
    {
        float random = Random.value;

        if (random < probabilityFoots)
        {
            stompingFoot = leftFoot;
            probabilityFoots -= probabilityChange;
        }
        else
        {
            stompingFoot = rightFoot;
            probabilityFoots += probabilityChange;
        }
        
        originalPosition = new Vector3(stompingFoot.transform.position.x,
            stompingFoot.transform.position.y,
            stompingFoot.transform.position.z);
        
        roarElapsedTime = 0;
        attack = true;
        rising = true;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoarAttack : MonoBehaviour
{
    [Header("Prefabs and GameObjects")]
    public GameObject[] spikePrefabs;

	public float offset;

    public GameObject ground;
    public CharacterHealth characterHealth;
    
    [Header("Spikes")] 
    public int spikeAmount;
    public float spikeSpawnTimeDiff;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void onRoar()
    {
        StartCoroutine(spawnSpikes());
    }
    
    IEnumerator spawnSpikes()
    {
        float width = 0;
        foreach (var spike in spikePrefabs)
        {
            width += spike.GetComponent<SpriteRenderer>().bounds.size.x;
        }
        
        float xOffset = width / (2 * spikePrefabs.Length);
        float totalXOffset = 0;
        Vector3 startPosition = transform.position + Vector3.down * offset;
        startPosition.x = transform.position.x;
        //startPosition.y = characterHealth.transform.position.y;

        for (int i = 0; i < spikeAmount / 2; i++)
        {
            GameObject leftSpike = spikePrefabs[Random.Range(0, spikePrefabs.Length)];
            GameObject rightSpike = spikePrefabs[Random.Range(0, spikePrefabs.Length)];
            
            Instantiate(leftSpike, startPosition + new Vector3(totalXOffset, 0, 0), Quaternion.identity);
            Instantiate(rightSpike, startPosition - new Vector3(totalXOffset, 0, 0), Quaternion.identity);
            totalXOffset += xOffset;
            yield return new WaitForSeconds(spikeSpawnTimeDiff);
        }  
    }
}

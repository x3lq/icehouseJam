using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Hand : MonoBehaviour
{

    [Header("Collision")]
    private BoxCollider2D boxCollider;
    public LayerMask collisionLayers;
    public Collider2D[] hits;
    
    public UnityEvent<float> triggerDamageWithDistance;
    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        hits = Physics2D.OverlapBoxAll(transform.position, boxCollider.size, 0, collisionLayers);
        
        foreach (Collider2D hit in hits)
        {
            if (hit == boxCollider)
                continue;
			
            ColliderDistance2D colliderDistance = hit.Distance(boxCollider);
            
            //TODO differentiate between weapon and playerhit
            triggerDamageWithDistance.Invoke(colliderDistance.distance);
        }
    }
    
}

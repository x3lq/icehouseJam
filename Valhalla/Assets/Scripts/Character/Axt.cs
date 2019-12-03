using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axt : MonoBehaviour
{
    public float coolDownDuration;
    private float coolDownTimer;

    public float maxDistance;
    public float speed;
    private float currentSpeed;
    public GameObject axt;
    public GameObject bottomAnchor;

    public GameObject axtPrefab;
    public GameObject axtChain;

    public Vector3 direction;

    public float slowedTimeSpeed;

    public float durationTime;

    public DateTime startTime;

    public Boolean active;

    public Boolean axtReturns;

	private CharacterAudio audio;

    [Header("Pulling Towards Axt")] public Boolean pullToAxt;
    public float pullDurationBasedOnDistance;
    public float pullDuration;
    public float pullSpeed;
    private float currentPullSpeed;
    private float startedAt;

    public float jumpTimeAfterPull;
    private float jumpTimeAfterPullTimer;
    private Boolean usedAxtJump;
    public Boolean axtJumpTriggerBefore;
    public float distanceJumpEnable;

    [Header("Indicator")] 
    public GameObject rotator;

    [Header("Collision")] public LayerMask collisionLayers;
    public Collider2D[] hits;
    private BoxCollider2D boxCollider;

    private CharacterMovement characterMovment;

    // Start is called before the first frame update
    void Start()
    {
        characterMovment = GetComponent<CharacterMovement>();
        coolDownDuration = jumpTimeAfterPull;
		audio = GetComponent<CharacterAudio>();
	}

    // Update is called once per frame
    void Update()
    {
        if (coolDownTimer > 0)
        {
            pullToAxt = false;

            if (Input.GetButtonDown("Jump" + ControllerSelector.type) && jumpTimeAfterPullTimer > 0 && !usedAxtJump)
            {
                usedAxtJump = true;
                characterMovment.wantsAxtJump = true;
            }

            coolDownTimer -= Time.deltaTime;
            jumpTimeAfterPullTimer -= Time.deltaTime;
            return;
        }

        if (Input.GetButtonDown("LB" + ControllerSelector.type) && !axt)
        {
            startTime = DateTime.Now;
            active = true;
            currentSpeed = speed;
            //measure time and slow time
            StartCoroutine(startTimer());
            Time.timeScale = slowedTimeSpeed;
            
            rotator.SetActive(true);
        }

        if (rotator.active)
        {
            if (ControllerSelector.type == "PC")
            {
                Vector3 mousePosition = Input.mousePosition;
                mousePosition.z = 15;
                mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
                direction = (mousePosition - characterMovment.transform.position).normalized;
            }
            else
            {
                direction = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0).normalized;
            }
            
            rotator.transform.rotation = Quaternion.identity;
            if (direction.x > 0)
            {
                rotator.transform.Rotate(0,0,-Vector3.Angle(Vector3.up, direction));
            }
            else
            {
                rotator.transform.Rotate(0,0,Vector3.Angle(Vector3.up, direction));
            }
        }


        if (Input.GetButtonUp("LB" + ControllerSelector.type) && !axt)
        {
            if (active)
            {
                active = false;
                Time.timeScale = 1;
                //throw axt on release
                direction = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0).normalized;
                
                if (ControllerSelector.type == "PC")
                {
                    Vector3 mousePosition = Input.mousePosition;
                    mousePosition.z = 15;
                    mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
                    direction = (mousePosition - characterMovment.transform.position).normalized;
                }
                
                rotator.SetActive(false);

                if (direction != Vector3.zero)
                {
                    GetComponent<Animator>().SetTrigger("ThrowAxt");
                }
            }
        }

        if (axt)
        {
            //coming back of axt
            if (axtReturns || (axt.transform.position - transform.position).magnitude > maxDistance)
            {
                axtReturns = true;
                direction = (axt.transform.position - transform.position).normalized;
                axt.transform.position += direction * currentSpeed * Time.deltaTime * -1;

                if ((axt.transform.position - transform.position).magnitude < 0.5 ||
                    (axt.transform.position - transform.position).magnitude > 4 * maxDistance)
                {
                    axtReturns = false;
                    Destroy(axt);
                }
            }
            else
            {
                axt.transform.position += direction * currentSpeed * Time.deltaTime;
            }

            if (pullToAxt)
            {
                Vector3 distanceLeft = (axt.transform.position - transform.position);
                float elapsedTimePercentage = (Time.time - startedAt) / pullDurationBasedOnDistance;
                currentPullSpeed = Mathf.SmoothStep(speed / 2, speed, elapsedTimePercentage);

                Vector3 newPlayerPos = transform.position + distanceLeft.normalized * currentPullSpeed * Time.deltaTime;

                if (Input.GetButtonDown("Jump" + ControllerSelector.type) && (newPlayerPos - axt.transform.position).magnitude < distanceJumpEnable && !usedAxtJump)
                {
                    axtJumpTriggerBefore = true;
                }

                if ((newPlayerPos - axt.transform.position).magnitude < 0.6)
                {
                    pullToAxt = false;
                    Destroy(axt);
                    coolDownTimer = coolDownDuration;
                    jumpTimeAfterPullTimer = jumpTimeAfterPull;
                    characterMovment.velocity = Vector2.zero;

                    if (axtJumpTriggerBefore)
                    {
                        axtJumpTriggerBefore = false;
                        usedAxtJump = true;
                        characterMovment.wantsAxtJump = true;
                        jumpTimeAfterPullTimer = 0;
                    }
                }
                else
                {
                    transform.position = newPlayerPos;
                }
            }
        }

        collisionDetection();
        collisionResolution();
    }

    public void throwAxt()
    {

        if (axt)
        {
            Destroy(axt);
        }

        axt = Instantiate(axtPrefab, transform.position, Quaternion.identity);
        axt.GetComponent<AxtRotation>().rotate = true;
        boxCollider = axt.GetComponent<BoxCollider2D>();
    }

    private void collisionDetection()
    {
        if (axt && !axtReturns && !(coolDownTimer > 0))
        {
            hits = Physics2D.OverlapBoxAll(axt.transform.position, boxCollider.size, 0, collisionLayers);
        }
    }

    private void collisionResolution()
    {
        foreach (Collider2D hit in hits)
        {
            if (hit == boxCollider)
                continue;

            if (!axt)
                return;

            ColliderDistance2D colliderDistance = hit.Distance(boxCollider);

            if (colliderDistance.distance < 0.1 && !axtReturns)
            {
				if (currentSpeed != 0)
				{
					audio.PlayAxtHitSound();
				}
                axt.GetComponent<AxtRotation>().rotate = false;
                currentSpeed = 0;
                pullToAxt = true;
                pullDurationBasedOnDistance =
                    pullDuration * ((axt.transform.position - transform.position).magnitude / maxDistance);
                startedAt = Time.time;
                usedAxtJump = false;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, maxDistance);
    }

    IEnumerator startTimer()
    {
        while (DateTime.Now.Subtract(startTime).Seconds < durationTime)
        {
            yield return null;
        }
        
        rotator.SetActive(false);
        if (!axt)
        {
            active = false;
            Time.timeScale = 1;
        }
    }
}
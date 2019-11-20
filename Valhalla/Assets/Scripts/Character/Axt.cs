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

    [Header("Pulling Towards Axt")] public Boolean pullToAxt;
    public float pullDurationBasedOnDistance;
    public float pullDuration;
    public float pullSpeed;
    private float currentPullSpeed;
    private float startedAt;

    public float jumpTimeAfterPull;
    private float jumpTimeAfterPullTimer;
    private Boolean usedAxtJump;

    [Header("Collision")] public LayerMask collisionLayers;
    public Collider2D[] hits;
    private BoxCollider2D boxCollider;

    private LineRenderer lineRenderer;

    private CharacterMovement characterMovment;

    // Start is called before the first frame update
    void Start()
    {
        characterMovment = GetComponent<CharacterMovement>();
        lineRenderer = GetComponent<LineRenderer>();
        coolDownDuration = jumpTimeAfterPull;
    }

    // Update is called once per frame
    void Update()
    {
        if (coolDownTimer > 0)
        {
            pullToAxt = false;

            if (Input.GetButtonDown("Jump") && jumpTimeAfterPullTimer > 0 && !usedAxtJump)
            {
                usedAxtJump = true;
                characterMovment.wantsAxtJump = true;
            }

            coolDownTimer -= Time.deltaTime;
            jumpTimeAfterPullTimer -= Time.deltaTime;
            return;
        }

        if (Input.GetButtonDown("LB") && !axt)
        {
            startTime = DateTime.Now;
            active = true;
            currentSpeed = speed;
            //measure time and slow time
            StartCoroutine(startTimer());
            Time.timeScale = slowedTimeSpeed;

            //show arrow Indicator with direction ?
        }


        if (Input.GetButtonUp("LB") && !axt)
        {
            if (active)
            {
                active = false;
                Time.timeScale = 1;
                //throw axt on release
                direction = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);

                if (direction != Vector3.zero)
                {
                    throwAxt();
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
                    lineRenderer.positionCount = 0;
                    lineRenderer.enabled = false;
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


                if ((newPlayerPos - axt.transform.position).magnitude < 0.6)
                {
                    pullToAxt = false;
                    Destroy(axt);
                    coolDownTimer = coolDownDuration;
                    jumpTimeAfterPullTimer = jumpTimeAfterPull;
                    lineRenderer.positionCount = 0;
                    lineRenderer.enabled = false;
                }
                else
                {
                    transform.position = newPlayerPos;
                }
            }

            //keeping chain link
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, axt.transform.GetChild(0).position);
        }

        collisionDetection();
        collisionResolution();
    }

    private void throwAxt()
    {
        lineRenderer.enabled = false;

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

        if (!axt)
        {
            active = false;
            Time.timeScale = 1;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Enemy
{
    [SerializeField] private int maxHealth = 1;
    [SerializeField] private Transform leftWaypoint;
    [SerializeField] private Transform rightWaypoint;
    [SerializeField] private float attackCooldown = 3f;
    [SerializeField] private float aggressionCooldown = 3f;
    [SerializeField] private float visionRange = 5f;
    [SerializeField] private float speed;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask groundLayer;
    private Transform target;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer graphics;
    private int currentHealth;
    private bool chasing = false;
    private float timeSinceLastSeen;
    private Transform playerPos;
    private float timeSinceLastHit;

    public override void EnemyHit(float damageValue)
    {
        currentHealth--;
        if (currentHealth <= 0)
            StartCoroutine(PlayDeath());
    }

    private IEnumerator PlayDeath()
    {
        anim.SetTrigger("Death");
        yield return new WaitUntil(() => Utility.AnimationFinished(anim, "ZombieDeath"));
        Destroy(gameObject);
    }

    public bool AnimationFinished(Animator animator, string animation)
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(animation) &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1;
    }

    private void Awake()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        graphics = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponent<Animator>();
        transform.position = leftWaypoint.position;
        target = rightWaypoint;
        timeSinceLastSeen = aggressionCooldown;
        timeSinceLastHit = attackCooldown;
    }

    private void Update()
    {
        if (currentHealth == 0)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        ChaseCheck();
        if (!chasing)
        {
            if (Vector2.Distance(transform.position, target.position) < 0.2f)
            {
                transform.position = target.position;
                target = leftWaypoint == target ? rightWaypoint : leftWaypoint;
            }
            else
            {
                rb.velocity = new Vector2(speed * (target.position.x > transform.position.x ? 1f : -1f), 0f);
            }
        }
        else
        {
            rb.velocity = new Vector2(speed * (playerPos.position.x > transform.position.x ? 1f : -1f), 0f);
            LedgeCheck();
        }

        timeSinceLastHit = Mathf.Min(timeSinceLastHit + Time.deltaTime, attackCooldown);
        anim.SetBool("Moving", rb.velocity.x != 0f);
        if (rb.velocity.x != 0f)
        {
            if (!(rb.velocity.x > 0f))
                graphics.flipX = true;
            else
                graphics.flipX = false;
        }
    }

    private void ChaseCheck()
    {
        timeSinceLastSeen = Mathf.Min(timeSinceLastSeen + Time.deltaTime, aggressionCooldown);
        Collider2D hit = Physics2D.OverlapBox(new(transform.position.x + (((visionRange / 2) + 0.5f) * (graphics.flipX ? -1f : 1f)), transform.position.y), new(visionRange, 2f), 0, playerLayer);
        if (hit)
        {
            chasing = true;
            timeSinceLastSeen = 0f;
            playerPos = hit.transform;
        }
        else if (timeSinceLastSeen < aggressionCooldown)
        {
            chasing = true;
        }
        else if (chasing)
        {
            chasing = false;
            target = Vector2.Distance(leftWaypoint.transform.position, transform.position) < Vector2.Distance(rightWaypoint.transform.position, transform.position) ?
                leftWaypoint : rightWaypoint;
        }

        anim.SetBool("Chasing", chasing);
        anim.SetBool("InVision", timeSinceLastSeen == 0f);
    }

    private void LedgeCheck()
    {
        float direction = rb.velocity.x == 0f ? graphics.flipX ? -1f : 1f : rb.velocity.x > 0f ? 1f : -1f;
        RaycastHit2D feetCheck = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, groundLayer);
        RaycastHit2D directionCheck = Physics2D.Raycast(new(transform.position.x + (0.55f * direction), transform.position.y), Vector2.down, 1.1f, groundLayer);
        if ((!feetCheck || !directionCheck) && feetCheck.distance != directionCheck.distance)
            rb.velocity = new(0f, rb.velocity.y);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (currentHealth != 0 && collision.gameObject.CompareTag("Player") && timeSinceLastHit == attackCooldown)
        {
            // TODO Player hurt sound and animation
            timeSinceLastHit = 0f;
            Player.Instance.Manager.AdjustHealth(-1);
        }
    }
}

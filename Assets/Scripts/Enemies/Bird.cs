using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Bird : Enemy
{
    [SerializeField] private int maxHealth = 1;
    [SerializeField] private Transform leftPoint;
    [SerializeField] private Transform rightPoint;
    [SerializeField] private float height = 5f;
    [SerializeField] private float duration = 10f;
    [SerializeField] private float visionRange = 5;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private AudioClip caw;
    private int currentHealth;
    private float animTime;
    private Vector3 start;
    private Vector3 end;
    private Animator anim;
    private SpriteRenderer graphics;
    private bool isFlying = false;
    private bool alerted = false;
    private bool spawningEgg = false;
    private AsyncOperationHandle<GameObject> opHandle;

    public override void EnemyHit(float damageValue)
    {
        currentHealth--;
        if (currentHealth <= 0)
        {
            StartCoroutine(PlayDeath());
        }
    }

    private Vector3 TimeBasedParabola(Vector3 start, Vector3 end, float height, float time)
    {
        float parabolicT = (time * 2) - 1;
        if (Mathf.Abs(start.y - end.y) < 0.1f)
        {
            Vector3 result = start + (time * (end - start));
            result.y += ((-parabolicT * parabolicT) + 1) * height;
            return result;
        }
        else
        {
            Vector3 travelDirection = end - start;
            Vector3 up = Vector3.Cross(Vector3.Cross(travelDirection, end - new Vector3(start.x, end.y, start.z)), travelDirection);
            if (end.y > start.y) up = -up;
            Vector3 result = start + (time * travelDirection);
            result += (((-parabolicT * parabolicT) + 1) * height) * up.normalized;
            return result;
        }
    }

    private void Update()
    {
        if (isFlying)
        {
            if (Vector3.Distance(transform.position, end) < 0.2f)
            {
                transform.position = end;
                start = start == leftPoint.position ? rightPoint.position : leftPoint.position;
                end = end == rightPoint.position ? leftPoint.position : rightPoint.position;
                animTime = 0f;
                isFlying = false;
                anim.SetTrigger("Landing");
            }
            else
            {
                transform.position = TimeBasedParabola(start, end, height, animTime / duration);
                animTime += Time.deltaTime;
            }

            if (spawningEgg == false && animTime / duration >= 0.5f)
            {
                spawningEgg = true;
                StartCoroutine(DropEgg());
            }
        }
        else if (!alerted)
        {
            Collider2D hit = Physics2D.OverlapBox(transform.position, new Vector2(visionRange * 2f, 2f), 0, playerLayer);
            if (hit)
            {
                spawningEgg = false;
                graphics.flipX = hit.transform.position.x < transform.position.x;
                StartCoroutine(DelayFlying());
            }
        }
    }

    private IEnumerator DropEgg()
    {
        opHandle = Addressables.LoadAssetAsync<GameObject>("EggProjectile");
        yield return opHandle;

        if (opHandle.Status == AsyncOperationStatus.Succeeded)
            Instantiate(opHandle.Result, transform.position, Quaternion.identity);
    }

    private IEnumerator DelayFlying()
    {
        alerted = true;
        anim.SetTrigger("Alerted");
        SoundManager.Instance.PlaySound(caw);
        yield return new WaitUntil(() => Utility.AnimationFinished(anim, "BirdAlerted"));
        graphics.flipX = end.x < transform.position.x;
        anim.SetTrigger("Fly");
        isFlying = true;
        alerted = false;
    }

    private IEnumerator PlayDeath()
    {
        anim.SetTrigger("Death");
        yield return new WaitUntil(() => Utility.AnimationFinished(anim, "BirdDeath"));
        Destroy(gameObject);
    }

    private void Awake()
    {
        currentHealth = maxHealth;
        animTime = 0;
        transform.position = leftPoint.position;
        start = leftPoint.position;
        end = rightPoint.position;
        anim = GetComponent<Animator>();
        graphics = GetComponentInChildren<SpriteRenderer>();
    }
}

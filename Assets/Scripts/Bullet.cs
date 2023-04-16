using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 0.5f;
    private float direction = 1f;
    private ParticleSystem ps;

    public void SetDirection(float direction)
    {
        this.direction = direction;
        ps.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, direction == 1f ? -90f : 90f, transform.rotation.eulerAngles.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().EnemyHit(1f);
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x + (Time.deltaTime * direction * speed), transform.position.y, transform.position.z);
    }

    private void Awake()
    {
        ps = GetComponentInChildren<ParticleSystem>();
    }
}

using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 0.5f;
    public float Direction = 1f;
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
        transform.position = new Vector3(transform.position.x + (Time.deltaTime * Direction * speed), transform.position.y, transform.position.z);
    }
}

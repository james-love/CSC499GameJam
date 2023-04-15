using System.Collections;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class OneWayPlatformController : MonoBehaviour
{
    private GameObject platform;
    private CircleCollider2D player;

    public void Down(CallbackContext context)
    {
        if (context.started && platform != null)
            StartCoroutine(DisableCollision());
    }

    private void Awake()
    {
        player = GetComponent<CircleCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWay"))
            platform = collision.gameObject;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWay"))
            platform = null;
    }

    private IEnumerator DisableCollision()
    {
        BoxCollider2D platformCollider = platform.GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(player, platformCollider);
        yield return new WaitForSeconds(0.25f);
        Physics2D.IgnoreCollision(player, platformCollider, false);
    }
}

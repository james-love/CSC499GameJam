using UnityEngine;

public class Egg : MonoBehaviour
{
    [SerializeField] private LayerMask ground;
    [SerializeField] private LayerMask player;
    [SerializeField] private AudioClip sound;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == 9)
        {
            
            Player.Instance.Manager.AdjustHealth(-1);
            Destroy(gameObject);
        }
        SoundManager.Instance.PlaySound(sound);
    }
}

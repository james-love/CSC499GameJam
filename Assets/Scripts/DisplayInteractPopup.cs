using UnityEngine;

public class DisplayInteractPopup : MonoBehaviour
{
    private void Reset()
    {
        this.hideFlags = HideFlags.HideInInspector;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.CompareTag("Player"))
            // TODO
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //if (collision.CompareTag("Player"))
            //TODO
    }
}

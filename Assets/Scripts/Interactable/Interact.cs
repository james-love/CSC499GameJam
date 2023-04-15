using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Interact : MonoBehaviour
{
    [SerializeField] private LayerMask interactMask;
    public void InteractPressed(CallbackContext context)
    {
        if (context.started)
        {
            Collider2D hit = Physics2D.OverlapBox(transform.position, new Vector2(1f, 2f), 0, interactMask);
            if (hit)
            {
                hit.gameObject.GetComponent<Interactable>().Interact();
            }
        }
    }
}

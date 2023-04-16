using UnityEngine;

public class Lock : MonoBehaviour
{
    [SerializeField] private Ability key;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!Player.Instance.State.Current.Abilities.Contains(key))
            Player.Instance.Input.DisableAction("Interact");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Player.Instance.Input.EnableAction("Interact");
    }
}

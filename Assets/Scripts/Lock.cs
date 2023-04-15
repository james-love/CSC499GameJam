using UnityEngine;

// TODO Temp class for confirming ability state saving
public class Lock : MonoBehaviour
{
    [SerializeField] private Ability key;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!PlayerState.Instance.State.Abilities.Contains(key))
            InputManager.Instance.DisableAction("Interact");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        InputManager.Instance.EnableAction("Interact");
    }
}

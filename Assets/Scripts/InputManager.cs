using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput inputs;

    public void Enable()
    {
        inputs.currentActionMap.Enable();
    }

    public void Disable()
    {
        inputs.currentActionMap.Disable();
    }

    public void EnableAction(string action)
    {
        inputs.currentActionMap.FindAction(action).Enable();
    }

    public void DisableAction(string action)
    {
        inputs.currentActionMap.FindAction(action).Disable();
    }

    private void Awake()
    {
        // TODO Remove this
        inputs = GetComponent<PlayerInput>();
        Enable();
        DisableAction("Jump");
        DisableAction("MeleeAttack");
        DisableAction("RangeAttack");
    }
}

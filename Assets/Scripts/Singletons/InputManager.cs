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

    public bool IsEnabled()
    {
        return inputs.currentActionMap.enabled;
    }

    public bool IsEnabled(string action)
    {
        return inputs.currentActionMap.FindAction(action).enabled;
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

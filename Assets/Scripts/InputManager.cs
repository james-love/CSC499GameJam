using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
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
        if (Instance == null)
        {
            // TODO Remove this
            inputs = GetComponent<PlayerInput>();
            Enable();
            DisableAction("Jump");
            DisableAction("MeleeAttack");
            DisableAction("RangeAttack");

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

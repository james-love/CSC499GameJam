using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // TODO Remove this
            GetComponent<PlayerInput>().currentActionMap.Enable();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

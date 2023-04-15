using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    public PlayerState State { get; private set; }
    public PlayerManager Manager { get; private set; }
    public InputManager Input { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            State = GetComponentInChildren<PlayerState>();
            Manager = GetComponentInChildren<PlayerManager>();
            Input = GetComponentInChildren<InputManager>();

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

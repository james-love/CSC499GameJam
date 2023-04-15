using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public static PlayerState Instance;
    [HideInInspector] public bool AlwaysRun = false;
    private SaveState state;

    public void SetAlwaysRun(bool newValue)
    {
        AlwaysRun = newValue;
        PlayerPrefs.SetInt("AlwaysRun", AlwaysRun ? 1 : 0);
    }

    public void SetSaveState(SaveState state)
    {
        this.state = state;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            if (PlayerPrefs.HasKey("AlwaysRun"))
                AlwaysRun = PlayerPrefs.GetInt("AlwaysRun") == 1;

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        foreach (GameObject point in GameObject.FindGameObjectsWithTag("SpawnPoint"))
        {
            if (point.GetComponent<SpawnPoint>().SpawnPointIndex == 1)
            {
                transform.position = point.transform.position;
                break;
            }
        }
    }
}

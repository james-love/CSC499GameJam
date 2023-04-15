using System.Linq;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public static PlayerState Instance { get; private set; }
    public SaveState State { get; private set; }
    [HideInInspector] public bool AlwaysRun { get; private set; } = false;

    public void SetAlwaysRun(bool newValue)
    {
        AlwaysRun = newValue;
        PlayerPrefs.SetInt("AlwaysRun", AlwaysRun ? 1 : 0);
    }

    public void AddAbility(Ability ability)
    {
        if (!State.Abilities.Contains(ability))
        {
            State.Abilities.Add(ability);

            // TODO Logic to enable/disable sprites for animation
            switch (ability)
            {
                case Ability.MeleeAttack:
                    InputManager.Instance.EnableAction("MeleeAttack");
                    break;
                case Ability.RangeAttack:
                    InputManager.Instance.EnableAction("RangeAttack");
                    break;
                case Ability.Jump:
                    InputManager.Instance.EnableAction("Jump");
                    break;
                default:
                    break;
            }
        }
    }

    public void AddFlag(Flag flag)
    {
        if (!State.Flags.Contains(flag))
            State.Flags.Add(flag);
    }

    private void Awake()
    {
        if (Instance == null)
        {
            if (PlayerPrefs.HasKey("AlwaysRun"))
                AlwaysRun = PlayerPrefs.GetInt("AlwaysRun") == 1;

            State = new() { LevelIndex = 2, SpawnPoint = 1, Abilities = new(), Flags = new() };

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

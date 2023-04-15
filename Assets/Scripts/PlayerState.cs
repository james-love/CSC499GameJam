using System.Linq;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public SaveState Current { get; private set; }
    [HideInInspector] public bool AlwaysRun { get; private set; } = false;

    public void SetAlwaysRun(bool newValue)
    {
        AlwaysRun = newValue;
        PlayerPrefs.SetInt("AlwaysRun", AlwaysRun ? 1 : 0);
    }

    public void AddAbility(Ability ability)
    {
        if (!Current.Abilities.Contains(ability))
        {
            Current.Abilities.Add(ability);

            EnableAbility(ability);
        }
    }

    public void AddFlag(Flag flag)
    {
        if (!Current.Flags.Contains(flag))
            Current.Flags.Add(flag);
    }

    public void SaveGame(int levelIndex, int spawnPoint)
    {
        Current.LevelIndex = levelIndex;
        Current.SpawnPoint = spawnPoint;
        PlayerPrefs.SetString("SavedGame", JsonUtility.ToJson(Current));
    }

    private void EnableAbility(Ability ability)
    {
        // TODO Logic to enable/disable sprites for animation
        switch (ability)
        {
            case Ability.MeleeAttack:
                Player.Instance.Input.EnableAction("MeleeAttack");
                break;
            case Ability.RangeAttack:
                Player.Instance.Input.EnableAction("RangeAttack");
                break;
            case Ability.Jump:
                Player.Instance.Input.EnableAction("Jump");
                break;
            default:
                break;
        }
    }

    private void Awake()
    {
        if (PlayerPrefs.HasKey("AlwaysRun"))
            AlwaysRun = PlayerPrefs.GetInt("AlwaysRun") == 1;

        Current = LevelManager.Instance.IsContinuedGame ?
            JsonUtility.FromJson<SaveState>(PlayerPrefs.GetString("SavedGame")) :
            new() { LevelIndex = 2, SpawnPoint = 1, Abilities = new(), Flags = new() };
    }

    private void Start()
    {
        if (LevelManager.Instance.IsContinuedGame)
        {
            foreach (GameObject point in GameObject.FindGameObjectsWithTag("SpawnPoint"))
            {
                if (point.GetComponents<MonoBehaviour>().OfType<ISpawnPoint>().ToArray()?[0]?.SpawnPointIndex == Current.SpawnPoint)
                {
                    GameObject.FindGameObjectWithTag("Player").transform.position = point.transform.position;
                    break;
                }
            }

            foreach (Ability ability in Current.Abilities)
            {
                EnableAbility(ability);
            }
        } 
        else
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
}

using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Animator wipeTransition;
    [SerializeField] private Animator circleTransition;
    [SerializeField] private RectTransform circleMask;
    [SerializeField] private GameObject player;
    [SerializeField] private AudioClip music;
    public static LevelManager Instance { get; private set; }
    public bool Loading { get; private set; }
    public bool IsContinuedGame { get; private set; } = false;

    public void ReloadMainMenu()
    {
        Destroy(Player.Instance.gameObject);
        SoundManager.Instance.PlayMusic(music);

        LoadLevel(0, wipeTransition);
    }

    public void ContinueGame()
    {
        IsContinuedGame = true;
        SaveState savedGame = JsonUtility.FromJson<SaveState>(PlayerPrefs.GetString("SavedGame"));
        LoadLevel(savedGame.LevelIndex, wipeTransition, savedGame.SpawnPoint);
    }

    public void StartNewGame()
    {
        IsContinuedGame = false;
        LoadLevelWipe(1);
    }

    public void LoadLevelWipe(int levelIndex)
    {
        LoadLevel(levelIndex, wipeTransition);
    }

    public void LoadLevelCircle(int levelIndex, int locationIndex = 1)
    {
        circleMask.anchoredPosition = Camera.main.WorldToScreenPoint(GameObject.FindGameObjectWithTag("Player").transform.position);
        LoadLevel(levelIndex, circleTransition, locationIndex);
    }

    private void LoadLevel(int levelIndex, Animator transition, int locationIndex = 1)
    {
        StartCoroutine(LoadAsync(levelIndex, transition, locationIndex));
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator LoadAsync(int levelIndex, Animator transition, int locationIndex = 1)
    {
        Loading = true;
        Time.timeScale = 0;
        transition.SetTrigger("Start");
        yield return new WaitUntil(() => Utility.AnimationFinished(transition, "TransitionStart"));

        AsyncOperation operation = SceneManager.LoadSceneAsync(levelIndex);
        while (!operation.isDone)
            yield return null;

        foreach (GameObject point in GameObject.FindGameObjectsWithTag("SpawnPoint"))
        {
            if (point.GetComponents<MonoBehaviour>().OfType<ISpawnPoint>().ToArray()?[0]?.SpawnPointIndex == locationIndex)
            {
                GameObject.FindGameObjectWithTag("Player").transform.position = point.transform.position;
                if (transition == circleTransition)
                {
                    yield return null;
                    circleMask.anchoredPosition = Camera.main.WorldToScreenPoint(point.transform.position);
                }

                break;
            }
        }

        transition.SetTrigger("Loaded");
        Loading = false;
        Time.timeScale = 1;
    }
}

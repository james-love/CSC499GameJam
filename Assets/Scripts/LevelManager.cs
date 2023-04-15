using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    [SerializeField] private Animator wipeTransition;
    [SerializeField] private Animator circleTransition;
    [SerializeField] private RectTransform circleMask;
    public bool Loading { get; private set; }

    public void ReloadMainMenu()
    {
        Destroy(PlayerManager.Instance.gameObject);
        // Set menu music here?

        LoadLevel(0, wipeTransition);
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
        yield return new WaitUntil(() => AnimationFinished(transition, "TransitionStart"));

        AsyncOperation operation = SceneManager.LoadSceneAsync(levelIndex);
        while (!operation.isDone)
            yield return null;

        if (transition == circleTransition)
        {
            foreach (GameObject point in GameObject.FindGameObjectsWithTag("SpawnPoint"))
            {
                if (point.GetComponents<MonoBehaviour>().OfType<ISpawnPoint>().ToArray()?[0]?.SpawnPointIndex == locationIndex)
                {
                    GameObject.FindGameObjectWithTag("Player").transform.position = point.transform.position;
                    yield return null;
                    circleMask.anchoredPosition = Camera.main.WorldToScreenPoint(point.transform.position);
                    break;
                }
            }
        }

        transition.SetTrigger("Loaded");
        Loading = false;
        Time.timeScale = 1;
    }

    private bool AnimationFinished(Animator animator, string animation)
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(animation) &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1;
    }
}

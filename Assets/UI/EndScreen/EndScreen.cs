using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class EndScreen : MonoBehaviour
{
    private VisualElement root;

    private void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        root.style.display = DisplayStyle.None;

        Button mainMenu = root.Query<Button>("MainMenu");
        mainMenu.RegisterCallback<ClickEvent>(_ =>
        {
            root.style.display = DisplayStyle.None;
            LevelManager.Instance.ReloadMainMenu();
        });

        Button reloadSave = root.Query<Button>("ReloadSave");
        reloadSave.RegisterCallback<ClickEvent>(_ =>
        {
            root.style.display = DisplayStyle.None;
            SceneManager.MoveGameObjectToScene(Player.Instance.gameObject, SceneManager.GetActiveScene());
            LevelManager.Instance.ContinueGame();
        });

        Button quit = root.Query<Button>("Quit");
        quit.RegisterCallback<ClickEvent>(_ =>
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        });
    }
}

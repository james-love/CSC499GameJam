using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class DeathScreen : MonoBehaviour
{
    private void Start()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        root.style.display = DisplayStyle.None;

        Button mainMenu = root.Q<Button>("MainMenu");
        mainMenu.RegisterCallback<ClickEvent>(_ => LevelManager.Instance.ReloadMainMenu());

        Button reloadSave = root.Q<Button>("ReloadSave");
        reloadSave.RegisterCallback<ClickEvent>(_ =>
        {
            SceneManager.MoveGameObjectToScene(Player.Instance.gameObject, SceneManager.GetActiveScene());
            LevelManager.Instance.ContinueGame();
        });
    }
}

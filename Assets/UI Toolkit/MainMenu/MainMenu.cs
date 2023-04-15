using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    private VisualElement root;

    private void Awake()
    {
        Time.timeScale = 0f;
        root = GetComponent<UIDocument>().rootVisualElement;

        Button start = root.Q<Button>("StartGame");
        start.RegisterCallback<ClickEvent>(_ => LevelManager.Instance.StartNewGame());

        VisualElement continueContainer = root.Q("ContinueContainer");
        continueContainer.style.display = PlayerPrefs.HasKey("SavedGame") ? DisplayStyle.Flex : DisplayStyle.None;

        Button continueBtn = root.Q<Button>("ContinueGame");
        continueBtn.RegisterCallback<ClickEvent>(_ => LevelManager.Instance.ContinueGame());

        Button deleteSave = root.Q<Button>("DeleteGame");
        deleteSave.RegisterCallback<ClickEvent>(_ => root.Q("ConfirmDelete").style.display = DisplayStyle.Flex);

        Button confirmDelete = root.Q<Button>("ConfirmDelete");
        confirmDelete.RegisterCallback<ClickEvent>(_ =>
        {
            PlayerPrefs.DeleteKey("SavedGame");
            continueContainer.style.display = DisplayStyle.None;
            root.Q("ConfirmDelete").style.display = DisplayStyle.None;
        });

        Button cancelDelete = root.Q<Button>("CancelDelete");
        cancelDelete.RegisterCallback<ClickEvent>(_ => root.Q("ConfirmDelete").style.display = DisplayStyle.None);

        Button settings = root.Q<Button>("Settings");
        settings.RegisterCallback<ClickEvent>(_ => print("open settings"));

        Button quit = root.Q<Button>("QuitGame");
        quit.RegisterCallback<ClickEvent>(_ =>
        {
            Application.Quit();
            EditorApplication.isPlaying = false;
        });
    }
}

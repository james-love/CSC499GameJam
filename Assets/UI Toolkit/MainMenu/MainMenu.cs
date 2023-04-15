using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    private VisualElement root;

    private void Awake()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        Button start = root.Q<Button>("StartGame");
        start.RegisterCallback<ClickEvent>(_ => LevelManager.Instance.LoadLevelWipe(2));

        VisualElement continueContainer = root.Q("ContinueContainer");
        continueContainer.style.display = PlayerPrefs.HasKey("SavedGame") ? DisplayStyle.Flex : DisplayStyle.None;

        Button continueBtn = root.Q<Button>("ContinueGame");
        continueBtn.RegisterCallback<ClickEvent>(_ => print("load the save game"));

        Button deleteSave = root.Q<Button>("DeleteGame");
        deleteSave.RegisterCallback<ClickEvent>(_ => print("delete the save game"));

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

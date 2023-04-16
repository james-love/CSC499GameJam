using UnityEngine;
using UnityEngine.UIElements;

public class EndGame : Interactable
{
    [SerializeField] private UIDocument endScreen;
    private VisualElement root;

    public override void Interact()
    {
        Time.timeScale = 0;
        root.Q<Button>("ReloadSave").style.display = PlayerPrefs.HasKey("SavedGame") ? DisplayStyle.Flex : DisplayStyle.None;
        root.style.display = DisplayStyle.Flex;
    }
}

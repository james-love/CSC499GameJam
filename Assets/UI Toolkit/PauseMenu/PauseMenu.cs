using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.InputSystem.InputAction;

public class PauseMenu : MonoBehaviour
{
    private VisualElement root;
    [SerializeField] private Sprite muteMusicIcon;
    [SerializeField] private Sprite unMuteMusicIcon;

    [SerializeField] private Sprite muteSFXIcon;
    [SerializeField] private Sprite unMuteSFXIcon;

    private void Awake()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        root.style.display = DisplayStyle.None;

        Button resume = root.Q<Button>("Resume");
        resume.RegisterCallback<ClickEvent>(_ => Resume());

        Button muteMusic = root.Q<Button>("MuteMusic");
        muteMusic.RegisterCallback<ClickEvent>(_ => ToggleMusic());

        Button muteSFX = root.Q<Button>("MuteSFX");
        muteSFX.RegisterCallback<ClickEvent>(_ => ToggleSFX());

        Slider volumeSlider = root.Q<Slider>("VolumeLevel");
        volumeSlider.RegisterValueChangedCallback(e => SoundManager.Instance.SetVolume(e.newValue));

        Button mainMenu = root.Query<Button>("MainMenu");
        mainMenu.RegisterCallback<ClickEvent>(_ =>
        {
            root.style.display = DisplayStyle.None;
            LevelManager.Instance.ReloadMainMenu();
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

    private void ToggleMusic()
    {
        SoundManager.Instance.ToggleMusic();
        UpdateVolumeDisplay();
    }

    private void ToggleSFX()
    {
        SoundManager.Instance.ToggleSFX();
        UpdateVolumeDisplay();
    }

    private void UpdateVolumeDisplay()
    {
        root.Q<Button>("MuteMusic").style.backgroundImage = new StyleBackground(SoundManager.Instance.MusicMuted ? muteMusicIcon : unMuteMusicIcon);
        root.Q<Button>("MuteSFX").style.backgroundImage = new StyleBackground(SoundManager.Instance.SFXMuted ? muteSFXIcon : unMuteSFXIcon);
        root.Q<Slider>("VolumeLevel").value = AudioListener.volume;
    }

    public void Pause(CallbackContext context)
    {
        if (context.started)
        {
            Time.timeScale = 0;
            UpdateVolumeDisplay();
            root.style.display = DisplayStyle.Flex;
        }
    }

    private void Resume()
    {
        Time.timeScale = 1;
        root.style.display = DisplayStyle.None;
    }
}

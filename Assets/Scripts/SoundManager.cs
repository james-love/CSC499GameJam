using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource effectsSource;

    public bool MusicMuted => musicSource.mute;
    public bool SFXMuted => effectsSource.mute;

    public void PlaySound(AudioClip clip)
    {
        effectsSource.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
        PlayerPrefs.SetInt("musicmute", MusicMuted ? 1 : 0);
    }

    public void ToggleSFX()
    {
        effectsSource.mute = !effectsSource.mute;
        PlayerPrefs.SetInt("sfxmute", SFXMuted ? 1 : 0);
    }

    public void SetVolume(float newVolume)
    {
        AudioListener.volume = newVolume;
        PlayerPrefs.SetFloat("volume", newVolume);
    }

    private void OnEnable()
    {
        if (PlayerPrefs.HasKey("volume"))
            AudioListener.volume = PlayerPrefs.GetFloat("volume");

        if (PlayerPrefs.HasKey("musicmute"))
            musicSource.mute = PlayerPrefs.GetInt("musicmute") == 1;

        if (PlayerPrefs.HasKey("sfxmute"))
            effectsSource.mute = PlayerPrefs.GetInt("sfxmute") == 1;
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
}

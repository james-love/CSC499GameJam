using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

// TODO: Split into a player manager and a HUD manager
public class PlayerManager : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;

    [SerializeField] private Sprite healthSprite;
    [SerializeField] private Sprite damagedSprite;

    [SerializeField] private AudioClip hurt;

    private List<Image> healthImages = new();
    private Image healthContainer;

    [SerializeField] private UIDocument deathScreen;

    [HideInInspector] public bool ShowHUD { get; set; }

    public int AdjustHealth(int adjustment)
    {
        currentHealth = Mathf.Clamp(currentHealth + adjustment, 0, maxHealth);

        if (currentHealth == 0)
            StartCoroutine(DeathAnimation());

        return currentHealth;
    }

    private IEnumerator DeathAnimation()
    {
        SoundManager.Instance.PlaySound(hurt);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Animator playerAnim = player.GetComponentInChildren<Animator>();
        player.GetComponentInChildren<PlayerInput>().currentActionMap.Disable();
        playerAnim.SetTrigger("Death");
        yield return new WaitUntil(() => Utility.AnimationFinished(playerAnim, "PlayerDeath"));
        Time.timeScale = 0;
        deathScreen.rootVisualElement.Q<Button>("ReloadSave").style.display = PlayerPrefs.HasKey("SavedGame") ? DisplayStyle.Flex : DisplayStyle.None;
        deathScreen.rootVisualElement.style.display = DisplayStyle.Flex;
    }

    private void Awake()
    {
        healthContainer = gameObject.GetComponentInChildren<Image>();

        for (int i = 0; i < maxHealth; i++)
        {
            Image temp = Instantiate(healthContainer, gameObject.transform);
            temp.rectTransform.position = new Vector3(temp.rectTransform.position.x + 225 + (80 * i), temp.rectTransform.position.y, temp.rectTransform.position.z);
            healthImages.Add(temp);
        }

        ShowHUD = true; // TODO change back to false, enable somewhere else
    }

    private void Update()
    {
        healthImages.ForEach(heart =>
        {
            if (!ShowHUD)
            {
                heart.enabled = false;
            }
            else
            {
                heart.enabled = true;
                if (healthImages.IndexOf(heart) < currentHealth)
                    heart.sprite = healthSprite;
                else
                    heart.sprite = damagedSprite;
            }
        });
    }
}

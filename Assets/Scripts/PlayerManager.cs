using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

// TODO: Split into a player manager and a HUD manager
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
    [HideInInspector] public bool ShowHUD;

    [SerializeField] private Sprite healthSprite;
    [SerializeField] private Sprite damagedSprite;

    private List<Image> healthImages = new();
    private Image healthContainer;

    [HideInInspector] public bool AlwaysRun = false;

    [SerializeField] private UIDocument deathScreen;

    public int AdjustHealth(int adjustment)
    {
        currentHealth = Mathf.Clamp(currentHealth + adjustment, 0, maxHealth);

        if (currentHealth == 0)
            StartCoroutine(DeathAnimation());

        return currentHealth;
    }

    public void SetAlwaysRun(bool newValue)
    {
        AlwaysRun = newValue;
        PlayerPrefs.SetInt("AlwaysRun", AlwaysRun ? 1 : 0);
    }

    private IEnumerator DeathAnimation()
    {
        // TODO: Play Death Sound
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Animator playerAnim = player.GetComponentInChildren<Animator>();
        player.GetComponentInChildren<PlayerInput>().currentActionMap.Disable();
        playerAnim.SetTrigger("Death");
        yield return new WaitUntil(() => AnimationFinished(playerAnim, "jimmy_dies"));
        Time.timeScale = 0;
        deathScreen.rootVisualElement.style.display = DisplayStyle.Flex;
    }

    private bool AnimationFinished(Animator animator, string animation)
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(animation) &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            healthContainer = gameObject.GetComponentInChildren<Image>();

            for (int i = 0; i < maxHealth; i++)
            {
                Image temp = Instantiate(healthContainer, gameObject.transform);
                temp.rectTransform.position = new Vector3(temp.rectTransform.position.x + 175 + (125 * i), temp.rectTransform.position.y, temp.rectTransform.position.z);
                healthImages.Add(temp);
            }

            ShowHUD = true; // TODO change back to false, enable somewhere else
            if (PlayerPrefs.HasKey("AlwaysRun"))
                AlwaysRun = PlayerPrefs.GetInt("AlwaysRun") == 1;
            //GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerInput>().currentActionMap.Disable();
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
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

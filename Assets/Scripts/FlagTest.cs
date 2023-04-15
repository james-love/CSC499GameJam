using UnityEngine;

public class FlagTest : Interactable
{
    [SerializeField] private Flag flag;
    private SpriteRenderer sprite;
    public override void Interact()
    {
        PlayerState.Instance.AddFlag(flag);
        sprite.color = Color.green;
    }

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (PlayerState.Instance.State.Flags.Contains(flag))
            sprite.color = Color.green;
    }
}
